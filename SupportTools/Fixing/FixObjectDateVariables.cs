using Artech.Architecture.Common.Objects;
using Artech.Architecture.Common.Services;
using Artech.Architecture.Language.Parser.Data;
using Artech.Genexus.Common;
using Artech.Genexus.Common.Parts;

namespace GeneXus.Packages.SupportTools.Fixing
{
	public class FixObjectDateVariables : FixObjectItems
	{
		public FixObjectDateVariables()
			: base(Resources.FixDateVariablesTitle, Resources.FixDateVariablesDescription)
		{ }

		public static bool ExecuteTool(KBModel model)
		{
			var instance = new FixObjectDateVariables();
			return instance.Execute(model);
		}

		protected override void ProcessObjectAndItems(KBModel model, ObjectAndItems obj)
		{
			IOutputService output = CommonServices.Output;

			KBObject kbObject = KBObject.ResolveName(Module.GetRoot(model), null, obj.Name);
			if (kbObject == null)
			{
				output.AddWarningLine($"Could not find '{obj.Name}' object");
				return;
			}

			output.AddLine($"{kbObject.Name}...");

			var part = kbObject.Parts.Get<VariablesPart>();
			foreach (string variableName in obj.Items)
			{
				var variable = part?.GetVariable(variableName);
				if (variable == null)
				{
					output.AddWarningLine($"   Could not find '&{variableName}' in '{kbObject.Name}' object");
					continue;
				}

				var dateFormat = variable.GetPropertyValue<string>(Properties.ATT.DateFormat);
				if (string.IsNullOrEmpty(dateFormat))
				{
					output.AddWarningLine($"   Could not get DateFormat property for &{variable.Name}");
					continue;
				}

				if (dateFormat == Properties.ATT.DateFormat_Values.YearWithFourDigits99999999)
				{
					output.AddWarningLine($"   &{variable.Name} does not need to be fixed");
					continue;
				}

				variable.SetPropertyValue(Properties.ATT.DateFormat, Properties.ATT.DateFormat_Values.YearWithFourDigits99999999);
				output.AddLine($"   &{variable.Name} was adjusted");
			}

			if (kbObject.Dirty)
			{
				kbObject.Save();
				output.AddLine($"{kbObject.Name} was saved");
			}
		}
	}
}
