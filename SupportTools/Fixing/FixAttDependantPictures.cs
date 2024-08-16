using Artech.Architecture.Common.Objects;
using Artech.Architecture.Common.Services;
using Artech.Genexus.Common;
using Artech.Genexus.Common.CustomTypes;
using Artech.Genexus.Common.Objects;
using Artech.Genexus.Common.Parts;
using Artech.Udm.Framework;
using Artech.Udm.Framework.References;
using System;
using System.Collections.Generic;
using static Artech.Genexus.Common.Properties;
using Attribute = Artech.Genexus.Common.Objects.Attribute;

namespace GeneXus.Packages.SupportTools.Fixing
{
	public class FixAttDependantPictures : FixObjects
	{
		struct FixStats
		{
			public FixStats()
			{
				fixes = 0;
				total = 0;
			}

			public int fixes;
			public int total;
		}

		struct ObjectVarsStats
		{
			public ObjectVarsStats()
			{
				fixedVars = 0;
				totalVars = 0;
				fixedObjects = 0;
				totalObjects = 0;
			}

			public int fixedVars;
			public int totalVars;
			public int fixedObjects;
			public int totalObjects;
		}


		public FixAttDependantPictures()
			: base(Resources.FixAttDependantPicturesTitle, Resources.FixAttDependantPicturesDescription)
		{ }

		public static bool ExecuteTool(KBModel model)
		{
			var instance = new FixAttDependantPictures();
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

			var atts = new Dictionary<int, KBObject>();
			var domains = new Dictionary<int, KBObject>();

			if (attOrDom is Attribute att)
				atts[att.Id] = att;
			else if (attOrDom is Domain domain)
			{
				domains[domain.Id] = domain;
				// Get all atributes (and procs) that are based on this domain
				AddRecursiveBasedOn(attOrDom.Model, domain, atts, domain);
			}

			var domainFixes = EnsureDefaultPicture(domains);
			output.AddLine($"Fixed {domainFixes.fixes}/{domainFixes.total} Domains");

			var attFixes = EnsureDefaultPicture(atts);
			output.AddLine($"Fixed {attFixes.fixes}/{attFixes.total} Attributes");

			var dependantObjects = GetDependantObjects(atts);
			var varStats = FixObjVarPictures(attOrDom.Model, domains, atts, dependantObjects);
			output.AddLine($"Fixed {varStats.fixedVars}/{varStats.totalVars} Variables in {varStats.fixedObjects}/{varStats.totalObjects} Objects");
		}

		private ObjectVarsStats FixObjVarPictures(
			KBModel model,
			Dictionary<int, KBObject> domains,
			Dictionary<int, KBObject> atts,
			IEnumerable<EntityKey> dependantObjects)
		{
			IOutputService output = CommonServices.Output;

			var stats = new ObjectVarsStats();
			foreach (var key in dependantObjects)
			{
				stats.totalObjects++;

				var obj = KBObject.Get(model, key);
				var varsPart = obj.Parts[PartType.Variables] as VariablesPart;
				if (varsPart == null)
				{
					output.AddWarningLine($"Could not load variables from Object {obj.Name}");
					continue;
				}

				bool fixedObject = false;
				foreach (Variable var in varsPart.Variables)
				{
					if (var.Type != eDBType.NUMERIC)
						continue;

					if (
						(var.AttributeBasedOn != null && atts.ContainsKey(var.AttributeBasedOn.Id)) ||
						(var.DomainBasedOn != null && domains.ContainsKey(var.DomainBasedOn.Id))
					)
					{
						stats.totalVars++;

						if (var.IsPropertyDefault(ATT.Picture))
							continue;

						string oldPicture = var.GetPropertyValueString(ATT.Picture);

						var.ResetProperty(ATT.Picture);
						fixedObject = true;
						stats.fixedVars++;

						string newPicture = var.GetPropertyValueString(ATT.Picture);
						output.AddLine($"Set default picture for: {obj.TypeDescriptor.Name}, {obj.Name}, &{var.Name}, {oldPicture}, {newPicture}");
					}
				}
				if (fixedObject)
				{
					obj.Save();
					stats.fixedObjects++;
				}
			}
			return stats;
		}

		private static IEnumerable<EntityKey> GetDependantObjects(Dictionary<int, KBObject> atts)
		{
			var objTypes = new List<Guid> {
				typeof(Transaction).GUID,
				typeof(WorkPanel).GUID,
				typeof(Procedure).GUID,
				typeof(Report).GUID
				};

			var objKeys = new HashSet<EntityKey>();
			foreach (var attri in atts.Values)
			{
				foreach (var obj in attri.GetReferencesTo(LinkType.UsedObject))
				{
					if (objTypes.Contains(obj.From.Type) && !objKeys.Contains(obj.From))
					{
						objKeys.Add(obj.From);
						yield return obj.From;
					}
				}
			}
		}

		private void AddRecursiveBasedOn(KBModel model, Domain domain, Dictionary<int, KBObject> atts, KBObject baseObj)
		{
			// needs to be recursive because subtypes, although based on a domain, may not always have
			// a direct reference (it may be through the supertype)
			foreach (var obj in baseObj.GetReferencesTo(LinkType.UsedObject))
			{
				if (obj.From.Type == typeof(Attribute).GUID)
				{
					if (!atts.ContainsKey(obj.From.Id))
					{
						var att = Attribute.Get(model, obj.From) as Attribute;
						if (att != null && att.DomainBasedOn == domain)
						{
							atts[att.Id] = att;
							AddRecursiveBasedOn(model, domain, atts, att);
						}
					}
				}
			}
		}

		private FixStats EnsureDefaultPicture(Dictionary<int, KBObject> atts)
		{
			var stats = new FixStats();
			IOutputService output = CommonServices.Output;
			foreach (var att in atts.Values)
			{
				// output.AddLine($"{att.Name}");

				stats.total++;

				var type = (eDBType)att.GetPropertyValue<AttCustomType>("ATTCUSTOMTYPE").DataType;
				if (type != eDBType.NUMERIC)
					continue;

				if (att.IsPropertyDefault(ATT.Picture))
					continue;

				string oldPicture = att.GetPropertyValueString(ATT.Picture);

				att.ResetProperty(ATT.Picture);
				att.Save();
				stats.fixes++;

				string newPicture = att.GetPropertyValueString(ATT.Picture);
				output.AddLine($"Set default picture for: Attribute, '{att.Name}', '{oldPicture}', '{newPicture}'");
			}

			return stats;

		}

	}
}
