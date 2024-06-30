using Artech.Architecture.Common.Deployment;
using Artech.Architecture.Common.Helpers.Check;
using Artech.Architecture.Common.Objects;
using Artech.Architecture.Common.Services;
using Artech.Common.Collections;
using Artech.Genexus.Common;
using Artech.Genexus.Common.Objects;
using Artech.Genexus.Common.Parts.Layout;
using Artech.Genexus.Common.Parts.Providers;
using Artech.Genexus.Common.Parts.SDT;
using Artech.Udm.Framework;
using Artech.Udm.Framework.References;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using static Artech.Genexus.Common.Properties;
using Attribute = Artech.Genexus.Common.Objects.Attribute;

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

		private int CheckImpact(KBObject kbObject)
		{
			IOutputService output = CommonServices.Output;
			int problems = 0;

			// Get VariablesPart
			var infoProvider = new ObjectInfoProvider(kbObject);
			foreach (var v in infoProvider.Variables)
			{
				if (
					v.Length == 7 &&
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
					output.AddLine($"{v.Name} in {kbObject.Name} is {v.Type.ToString()}(7.0)");
					problems++;
				}
			}

			return problems;
		}
	}
}
