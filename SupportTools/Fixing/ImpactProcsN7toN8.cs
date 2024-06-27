using Artech.Architecture.Common.Objects;
using Artech.Architecture.Common.Services;
using Artech.Genexus.Common;
using Artech.Genexus.Common.Objects;
using Artech.Genexus.Common.Parts.Layout;
using Artech.Udm.Framework;
using Artech.Udm.Framework.References;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Attribute = Artech.Genexus.Common.Objects.Attribute;

namespace GeneXus.Packages.SupportTools.Fixing
{
	public class ImpactProcsN7toN8 : FixObjects
	{
		public ImpactProcsN7toN8()
			: base(Resources.ImpactProcsN7toN8Title, Resources.ImpactProcsN7toN8Description)
		{ }

		public static bool ExecuteTool(KBModel model)
		{
			var instance = new ImpactProcsN7toN8();
			return instance.Execute(model);
		}

		protected override KBObject GetSingleObject(KBModel model, string name)
		{
			return name.StartsWith("_") ?
				Domain.Get(model, new QualifiedName(name.Substring(1))) :
				Attribute.Get(model, name);
		}

		protected override IEnumerable<KBObject> GetAllObjects(KBModel model)
		{
			foreach (var obj in Domain.GetAll(model))
				yield return obj;

			foreach (var obj in Attribute.GetAll(model))
				yield return obj;
		}

		protected override void ProcessObject(KBObject attOrDom, bool warnIfNoNeed = false)
		{
			IOutputService output = CommonServices.Output;

			Domain domain = attOrDom as Domain;
			var atts = new Dictionary<int, Attribute>();
			var procKeys = new HashSet<EntityKey>();

			if (attOrDom is Attribute)
				atts[attOrDom.Id] = attOrDom as Attribute;
			else if (domain != null)
			{
				// Get all atributes (and procs) that are based on (or reference) this domain
				foreach (var obj in domain.GetReferencesTo(LinkType.UsedObject))
				{
					if (obj.From.Type == typeof(Attribute).GUID)
					{
						var att = Attribute.Get(attOrDom.Model, obj.From) as Attribute;
						if (att != null)
							atts[att.Id] = att;
					}
					else if (obj.From.Type == typeof(Procedure).GUID)
					{
						procKeys.Add(obj.From);
					}
				}
			}

			// Get all procedures that use these attributes
			procKeys.UnionWith(
				atts.Values.SelectMany(att => att.GetReferencesTo(LinkType.UsedObject))
					.Where(r => r.From.Type == typeof(Procedure).GUID)
					.Select(r => r.From));

			int impactCount = 0;
			int problems = 0;
			foreach (var key in procKeys)
			{
				int procProblems = CheckImpact(attOrDom.Model, key, domain, atts);
				if (procProblems > 0)
				{
					problems += procProblems;
					impactCount++;
				}
			}

			output.AddLine($"Found {problems} problems impacting {impactCount} procedures from a total {procKeys.Count} that use '{attOrDom.Name}'");
		}

		private int CheckImpact(KBModel model, EntityKey key, Domain domain, Dictionary<int, Attribute> atts)
		{
			IOutputService output = CommonServices.Output;
			var proc = Procedure.Get(model, key) as Procedure;
			if (proc == null)
			{
				output.AddWarningLine($"Could not find Procedure {key}");
				return 0;
			}

			return CheckImpact(proc, domain, atts);
		}

		private int CheckImpact(Procedure proc, Domain domain, Dictionary<int, Attribute> atts)
		{
			IOutputService output = CommonServices.Output;
			if (!ProcHasPrintStatements(proc))
			{
				return 0;
			}

			List<Variable> vars = new List<Variable>();
			// get all varibles based on Domain or any attribute
			foreach (var variItem in proc.Variables.Variables)
			{
				if (variItem.AttributeBasedOn != null && atts.Any(att => att.Id == variItem.AttributeBasedOn.Id) ||
					variItem.DomainBasedOn != null && variItem.DomainBasedOn.Id == domain.Id ||
					// variItem.Type == eDBType.NUMERIC && variItem.Length == 7 && variItem.Decimals == 0 ||
					false
					) 
				{
					vars.Add(variItem);
				}
			}

			return CheckPrintblocks(proc, domain, atts, vars);
		}

		private static bool ProcHasPrintStatements(Procedure proc)
		{
			IOutputService output = CommonServices.Output;
			var procedurePart = proc.ProcedurePart;
			if (procedurePart == null)
			{
				output.AddWarningLine($"Could not get Source for '{proc.Name}'");
				return false;
			}

			// check if procedurePart.Source contains any 'print' as whole word (case insensitive)
			if (!Regex.IsMatch(procedurePart.Source, @"\bprint\b", RegexOptions.IgnoreCase))
			{
				return false;
			}

			// The source does include the word 'print'. It may be a false positive when, for example,
			// the word 'print' is part of a comment, but we conside it good enough because it's a quick check.

			return true;
		}

		private static int CheckPrintblocks(Procedure proc, Domain domain, Dictionary<int, Attribute> atts, IEnumerable<Variable> vars)
		{
			IOutputService output = CommonServices.Output;
			var layoutPart = proc.Layout;
			if (layoutPart == null)
			{
				output.AddWarningLine($"Could not get Layout for '{proc.Name}'");
				return 0;
			}

			int problems = 0;
			foreach (ReportBand band in layoutPart.Layout.ReportBands)
			{
				foreach (ReportAttribute attControl in band.Controls.OfType<ReportAttribute>())
				{
					ITypedObject typedObject = attControl.AttributeReference?.TypedObject;
					if (typedObject == null)
					{
						output.AddWarningLine($"Could not get TypedObject for '{attControl.AttributeReference.Name}' in ReportBand '{band.Name}'");
						continue;
					}

					var attId = attControl.AttributeId;
					if (
						(typedObject is Attribute && atts.ContainsKey(attId)) ||
						vars.Any(v => v.Id == attId)
					)
					{
						if (CheckAttControlInPrintblock(band, attControl))
						{
							problems++;
						}
					}
				}
			}

			return problems;
		}

		private static bool CheckAttControlInPrintblock(ReportBand band, ReportAttribute attControl)
		{
			IOutputService output = CommonServices.Output;
			int rightBorder = attControl.X + attControl.Width - 1;
			foreach (var control in band.Controls)
			{
				if (
					// not the same control
					control.Name != attControl.Name &&

					// sharing a line
					control.Y <= attControl.Y &&
					control.Y + control.Height - 1 >= attControl.Y &&

					// starts after attControl
					control.X > attControl.X &&
					// but not enough to the right
					control.X - rightBorder < 3 // must start at least in rightborder + 3
												// 1 is needed to allow grow, 1 for a blank space.
				)
				{
					string message = $"Proc {band.KBObject.Name}: on {band.Name}, {attControl.Name} takes columns {attControl.X}-{rightBorder} and {control.Name} starts on column {control.X}";
					output.AddLine(message);

					// Append the message to a file named "output.txt" in the current working directory
					string filePath = System.IO.Path.Combine(band.KBObject.KB.Location, "check_procs.txt");
					System.IO.File.AppendAllText(filePath, message + System.Environment.NewLine);

					return true;
				}
			}
			return false;
		}
	}
}
