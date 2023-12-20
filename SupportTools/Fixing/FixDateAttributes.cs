using Artech.Architecture.Common.Objects;
using Artech.Architecture.Common.Services;
using Artech.Genexus.Common;
using Artech.Genexus.Common.Parts.Layout;
using System;
using System.Linq;
using static Artech.Genexus.Common.Properties;
using Attribute = Artech.Genexus.Common.Objects.Attribute;

namespace GeneXus.Packages.SupportTools.Fixing
{
	public class FixDateAttributes : FixObjects
	{
		public FixDateAttributes()
			: base(Resources.FixDateAttributesTitle, Resources.FixDateAtributesDescription)
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

			var att = Attribute.Get(model,name);
			if (att == null)
			{
				output.AddWarningLine($"Could not find Attribute '{name}'");
				return;
			}

			var dateFormat = att.GetProperty(Properties.ATT.DateFormat);
			if (dateFormat == null) { }
			{
				output.AddWarningLine($"Could not gate DateFormat for Attribute '{name}'");
				return;
			}

			if ((string)dateFormat.Value == Properties.ATT.DateFormat_Values.YearWithFourDigits99999999)
			{
				output.AddWarningLine($"Attribute '{name}' does not need to be fixed");
				return;
			}

			att.SetPropertyValue(Properties.ATT.DateFormat, Properties.ATT.DateFormat_Values.YearWithFourDigits99999999);
			att.Save();
			output.AddLine($"Attribue '{name}' was adjusted");
		}
	}
}
