using Artech.Architecture.Common.Helpers.Check;
using Artech.Architecture.Common.Objects;
using Artech.Architecture.Common.Services;
using Artech.Genexus.Common;
using Artech.Genexus.Common.Objects;
using Artech.Udm.Framework;
using System.Collections.Generic;
using Attribute = Artech.Genexus.Common.Objects.Attribute;

namespace GeneXus.Packages.SupportTools.Fixing
{
	public class FixDateAttributes : FixObjects
	{
		public FixDateAttributes()
			: base(Resources.FixDateAttributesTitle, Resources.FixDateAttributesDescription)
		{ }

		public static bool ExecuteTool(KBModel model)
		{
			var instance = new FixDateAttributes();
			return instance.Execute(model);
		}

		protected override KBObject GetSingleObject(KBModel model, string name)
		{
			return name.StartsWith("_")?
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
			var dateFormat = attOrDom.GetPropertyValue<string>(Properties.ATT.DateFormat);
			string typeName = $"{attOrDom.TypeDescriptor.Description} '{attOrDom.Name}'";

			if (string.IsNullOrEmpty(dateFormat))
			{
				output.AddWarningLine($"Could not get DateFormat for {typeName}");
				return;
			}

			if (dateFormat == Properties.ATT.DateFormat_Values.YearWithFourDigits99999999)
			{
				if (warnIfNoNeed)
				{
					output.AddWarningLine($"{typeName} does not need to be fixed");
				}

				return;
			}

			attOrDom.SetPropertyValue(Properties.ATT.DateFormat, Properties.ATT.DateFormat_Values.YearWithFourDigits99999999);
			attOrDom.Save();
			output.AddLine($"{typeName} was adjusted");
		}
	}
}
