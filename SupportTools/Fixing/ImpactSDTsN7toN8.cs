using Artech.Architecture.Common.Objects;
using Artech.Architecture.Common.Services;
using Artech.Genexus.Common;
using Artech.Genexus.Common.Objects;
using Artech.Genexus.Common.Parts.SDT;

namespace GeneXus.Packages.SupportTools.Fixing
{
	public class ImpactSDTsN7toN8 
	{
		public ImpactSDTsN7toN8()
		{ }

		public static bool ExecuteTool(KBModel model)
		{
			var instance = new ImpactSDTsN7toN8();
			return instance.CheckImpact(model, 7) && instance.CheckImpact(model, 4);
		}

		protected bool CheckImpact(KBModel model, int length)
		{
			IOutputService output = CommonServices.Output;

			int total = 0;
			int impactCount = 0;
			int problems = 0;
			foreach (var sdt in SDT.GetAll(model))
			{
				total++;
				int sdtProblems = CheckImpact(sdt, length);
				if (sdtProblems > 0)
				{
					problems += sdtProblems;
					impactCount++;
				}
			}

			output.AddLine($"Length {length}: Found {problems} problems impacting {impactCount} SDTs from a total {total}");
			return true;
		}

		private int CheckImpact(SDT sdt, int length)
		{
			int impactCount = 0;
			IOutputService output = CommonServices.Output;
			foreach (var item in sdt.SDTStructure.Root.GetAllItems<SDTItem>())
			{
				if (
					item.BasedOn == null &&
					(
						item.Type == eDBType.CHARACTER ||
						item.Type == eDBType.VARCHAR ||
						item.Type == eDBType.LONGVARCHAR ||
						item.Type == eDBType.NUMERIC
					) &&
					item.Length == length &&
					item.Decimals == 0
				)
				{
					impactCount++;
					output.AddLine($"{item.Name} in {sdt.Name} is {item.Type}({length}.0)");
				}
			}
			return impactCount;
		}
	}
}
