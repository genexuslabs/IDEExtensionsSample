using Artech.Architecture.Common.Objects;
using Artech.Architecture.Common.Services;
using Artech.Genexus.Common;
using Artech.Genexus.Common.Objects;
using Artech.Udm.Framework;
using System.Collections.Generic;

namespace GeneXus.Packages.SupportTools.Fixing
{
	public class SetObjetsToNotGenerate : FixObjects
	{
		public static bool ExecuteTool(KBModel model)
		{
			var instance = new SetObjetsToNotGenerate();
			return instance.Execute(model);
		}

		protected int maxObjDescLen;

		public SetObjetsToNotGenerate()
			: base(Resources.SetObjectsToNotGenerate, Resources.SetObjectsToNotGenerateDescription)
		{
		}

		protected override KBObject GetSingleObject(KBModel model, string name)
		{
			return model.Objects.Get(EntityType.DEFAULT_NAMESPACE, new QualifiedName(name));
		}

		protected override IEnumerable<KBObject> GetAllObjects(KBModel model)
		{
			foreach (var obj in Transaction.GetAll(model))
				yield return obj;

			foreach (var obj in Procedure.GetAll(model))
				yield return obj;

			foreach (var obj in WorkPanel.GetAll(model))
				yield return obj;
		}

		protected override void ProcessObject(KBObject obj, bool warnIfNoNeed = false)
		{
			IOutputService output = CommonServices.Output;
			string typeName = $"{obj.TypeDescriptor.Description} '{obj.Name}'";

			bool doGenerate = obj.GetPropertyValue<bool>(Properties.TRN.GenerateObject);

			if (!doGenerate)
			{
				if (warnIfNoNeed)
				{
					output.AddWarningLine($"{typeName} is already set to not generate");
				}
				return;
			}

			obj.SetPropertyValue(Properties.TRN.GenerateObject, false);
			obj.Save();
			output.AddLine($"{typeName} was adjusted");
		}
	}
}
