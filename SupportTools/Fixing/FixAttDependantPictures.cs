using Artech.Architecture.Common.Objects;
using Artech.Architecture.Common.Services;
using Artech.Genexus.Common;
using Artech.Genexus.Common.CustomTypes;
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
		struct FixStats
		{
			public FixStats()
			{
				fixDomains = 0;
				totalDomains = 0;
				fixAtts = 0;
				totalAtts = 0;
			}

			public int fixDomains;
			public int totalDomains;
			public int fixAtts;
			public int totalAtts;
		}

		struct VarStats
		{
			public VarStats()
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
			var objKeys = new HashSet<EntityKey>();

			if (attOrDom is Attribute att)
				atts[att.Id] = att;
			else if (attOrDom is Domain domain)
			{
				atts[domain.Id] = domain;
				// Get all atributes (and procs) that are based on this domain
				AddRecursiveBasedOn(attOrDom.Model, domain, atts, domain);
			}

			var fixes = FixAttPictures(atts);
			output.AddLine($"Fixed {fixes.fixDomains}/{fixes.totalDomains} Domains and {fixes.fixAtts}/{fixes.totalAtts} Attributes");
		}

		private void AddRecursiveBasedOn(KBModel model, Domain domain, Dictionary<int, KBObject> atts, KBObject baseAtt)
		{
			// needs to be recursive because subtypes, although based on a domain, may not always have
			// a direct reference (it may be through the supertype)
			foreach (var obj in baseAtt.GetReferencesTo(LinkType.UsedObject))
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

		private FixStats FixAttPictures(Dictionary<int, KBObject> atts)
		{
			var stats = new FixStats();
			IOutputService output = CommonServices.Output;
			foreach (var att in atts.Values)
			{
				output.AddLine($"{att.Name}");

				if (att is Domain)
					stats.totalDomains++;
				else
					stats.totalAtts++;

				var type = (eDBType) att.GetPropertyValue<AttCustomType>("ATTCUSTOMTYPE").DataType;
				if (type != eDBType.NUMERIC)
					continue;

				if (att.IsPropertyDefault(Properties.ATT.Picture))
					continue;

				string oldPicture = att.GetPropertyValueString(Properties.ATT.Picture);

				att.ResetProperty(Properties.ATT.Picture);
				att.Save();
				if (att is Domain)
					stats.fixDomains++;
				else
					stats.fixAtts++;

				string newPicture = att.GetPropertyValueString(Properties.ATT.Picture);
				output.AddLine($"Set default picture for, Attribute, '{att.Name}', '{oldPicture}', '{newPicture}'");
			}

			return stats;
		}

	}
}
