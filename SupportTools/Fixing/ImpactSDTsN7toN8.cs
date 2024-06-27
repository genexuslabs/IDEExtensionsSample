using Artech.Architecture.Common.Deployment;
using Artech.Architecture.Common.Objects;
using Artech.Architecture.Common.Services;
using Artech.Common.Collections;
using Artech.Genexus.Common;
using Artech.Genexus.Common.Objects;
using Artech.Genexus.Common.Parts.Layout;
using Artech.Genexus.Common.Parts.SDT;
using Artech.Udm.Framework;
using Artech.Udm.Framework.References;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Attribute = Artech.Genexus.Common.Objects.Attribute;

namespace GeneXus.Packages.SupportTools.Fixing
{
	public class ImpactSDTsN7toN8 
	{
		public ImpactSDTsN7toN8()
		{ }

		public static bool ExecuteTool(KBModel model)
		{
			var instance = new ImpactSDTsN7toN8();
			return instance.CheckImpact(model);
		}

		protected bool CheckImpact(KBModel model)
		{
			IOutputService output = CommonServices.Output;

			int total = 0;
			int impactCount = 0;
			int problems = 0;
			foreach (var sdt in SDT.GetAll(model))
			{
				total++;
				int sdtProblems = CheckImpact(sdt);
				if (sdtProblems > 0)
				{
					problems += sdtProblems;
					impactCount++;
				}
			}

			output.AddLine($"Found {problems} problems impacting {impactCount} SDTs from a total {total}");
			return true;
		}

		private int CheckImpact(SDT sdt)
		{
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
					item.Length == 7 &&
					item.Decimals == 0
				)
				{
					output.AddLine($"{item.Name} in {sdt.Name} is {item.Type.ToString()}(7.0)");
				}
			}
			return 0;
		}
	}
}
