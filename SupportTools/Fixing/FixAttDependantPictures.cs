using Artech.Architecture.Common.Objects;
using Artech.Architecture.Common.Services;
using Artech.Genexus.Common;
using Artech.Genexus.Common.Objects;
using Artech.Genexus.Common.Parts.Layout;
using Artech.Genexus.Common.Parts.Providers;
using Artech.Udm.Framework;
using Artech.Udm.Framework.References;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using static Artech.Genexus.Common.Properties;
using Attribute = Artech.Genexus.Common.Objects.Attribute;

namespace GeneXus.Packages.SupportTools.Fixing
{
	public class FixAttDependantPictures : FixObjects
	{
		struct AttStats
		{
			public int fixedAtts;
			public int totalAtts;
		}

		struct VarStats
		{
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

			Domain domain = attOrDom as Domain;
			var atts = new Dictionary<int, Attribute>();

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
				}
			}


			var attFixes = FixAttPictures(atts);
			output.AddLine($"Fixed {attFixes.fixedAtts}/{attFixes.totalAtts} Attributes");
		}

		private void AddRecursive(KBModel model, Dictionary<int, Attribute> atts, Attribute baseAtt)
		{
			throw new NotImplementedException();
		}

		private AttStats FixAttPictures(Dictionary<int, Attribute> atts)
		{
			AttStats stats = new AttStats();
			stats.fixedAtts = 0;
			stats.totalAtts = 0;
			IOutputService output = CommonServices.Output;
			foreach (var att in atts.Values)
			{
				output.AddLine($"{att.Name}");
				stats.totalAtts++;
				if (att.Type != eDBType.NUMERIC)
					continue;

				if (att.IsPropertyDefault(Properties.ATT.Picture))
					continue;

				string oldPicture = att.GetPropertyValueString(Properties.ATT.Picture);

				att.ResetProperty(Properties.ATT.Picture);
				att.Save();
				stats.fixedAtts++;

				string newPicture = att.GetPropertyValueString(Properties.ATT.Picture);
				output.AddLine($"Set default picture for, Attribute, '{att.Name}', '{oldPicture}', '{newPicture}'");
			}

			return stats;
		}

	}
}
