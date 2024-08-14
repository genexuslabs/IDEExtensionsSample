using Artech.Architecture.Common.Objects;
using Artech.Architecture.Common.Services;
using Artech.Genexus.Common;
using Artech.Genexus.Common.Objects;
using Artech.Genexus.Common.Parts.Providers;
using System;
using System.IO;
using System.Collections.Generic;

namespace GeneXus.Packages.SupportTools.Fixing
{
	public class ImpactObjVarsN7toN8 
	{
		public ImpactObjVarsN7toN8()
		{ }

		public static bool ExecuteTool(KBModel model)
		{
			var instance = new ImpactObjVarsN7toN8();
			return instance.CheckImpact(model);
		}

		protected bool CheckImpact(KBModel model)
		{
			IOutputService output = CommonServices.Output;

			CheckObjectsImpact(model, Procedure.GetAll, "Procedures");
			CheckObjectsImpact(model, Transaction.GetAll, "Transactionss");
			CheckObjectsImpact(model, WorkPanel.GetAll, "Work Panels");
			return true;
		}

		protected void CheckObjectsImpact(KBModel model, Func<KBModel, IEnumerable<KBObject>> getObjectsFunc, string collectiveName)
		{
			IOutputService output = CommonServices.Output;
			int total = 0;
			int impactCount = 0;
			int problems = 0;
			foreach (var obj in getObjectsFunc(model))
			{
				total++;
				int objProblems = CheckImpact(obj);
				if (objProblems > 0)
				{
					problems += objProblems;
					impactCount++;
				}
			}

			output.AddLine($"Found {problems} problems impacting {impactCount} {collectiveName} from a total {total}");
		}

		private const int NumLengthToCheck = 4;

		private int CheckImpact(KBObject kbObject)
		{
			IOutputService output = CommonServices.Output;
			string filePath = Path.Combine(kbObject.KB.Location, "check_allvars.txt");
			int problems = 0;

			// Get VariablesPart
			var infoProvider = new ObjectInfoProvider(kbObject);
			foreach (var v in infoProvider.Variables)
			{
				if (
					v.Length == NumLengthToCheck &&
					v.Decimals == 0 &&
					(
						v.Type == eDBType.CHARACTER ||
						v.Type == eDBType.VARCHAR ||
						v.Type == eDBType.LONGVARCHAR ||
						v.Type == eDBType.NUMERIC
					) &&
					v.AttributeBasedOn == null &&
					v.DomainBasedOn == null &&
					true
				)
				{
					string message = $"&{v.Name} in {kbObject.Name} is {v.Type.ToString()}({NumLengthToCheck}.0)";
					output.AddLine(message);

					string csvMessage = $"{kbObject.Name},{v.Name},{v.Type.ToString()}({NumLengthToCheck}.0){Environment.NewLine}";
					File.AppendAllText(filePath, csvMessage);
					problems++;
				}
			}

			return problems;
		}
	}
}
