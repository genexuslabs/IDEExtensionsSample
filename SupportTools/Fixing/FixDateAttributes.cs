using Artech.Architecture.Common.Objects;
using Artech.Architecture.Common.Services;
using Artech.Genexus.Common;
using Artech.Genexus.Common.Objects;
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

		protected override void ProcessObjectName(KBModel model, string name)
		{
			IOutputService output = CommonServices.Output;
			output.AddLine($"Processing {name}");

			string typeName = $"Attribute '{name}'";
			KBObject attOrDom = null;
			if (name.StartsWith("_"))
			{
				name = name.Substring(1);
				typeName = $"Domain '{name}'";
				attOrDom = Domain.Get(model, new QualifiedName(name));
			}
			else
			{
				typeName = $"Attribute '{name}'";
				attOrDom = Attribute.Get(model, name);
			}

			if (attOrDom == null)
			{
				output.AddWarningLine($"Could not find {typeName}");
				return;
			}

			var dateFormat = attOrDom.GetPropertyValue<string>(Properties.ATT.DateFormat);
			if (string.IsNullOrEmpty(dateFormat))
			{
				output.AddWarningLine($"Could not gate DateFormat for {typeName}");
				return;
			}

			if (dateFormat == Properties.ATT.DateFormat_Values.YearWithFourDigits99999999)
			{
				output.AddWarningLine($"{typeName} does not need to be fixed");
				return;
			}

			attOrDom.SetPropertyValue(Properties.ATT.DateFormat, Properties.ATT.DateFormat_Values.YearWithFourDigits99999999);
			attOrDom.Save();
			output.AddLine($"{typeName} was adjusted");
		}
	}
}
