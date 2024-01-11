using Artech.Architecture.Common.Objects;
using Artech.Architecture.Common.Services;
using Artech.Genexus.Common;
using Artech.Genexus.Common.Objects;
using Artech.Udm.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GeneXus.Packages.SupportTools.Fixing
{
	public class FixObjectDescriptions : FixObjects
	{
		static readonly int defaultMaxObjDescLen = 37;
		public static bool ExecuteTool(KBModel model)
		{
			var instance = new FixObjectDescriptions();
			return instance.Execute(model);
		}

		protected int maxObjDescLen;

		public FixObjectDescriptions()
			: this(FixObjectDescriptions.defaultMaxObjDescLen) { }

		public FixObjectDescriptions(int maxObjDescLen)
			: base(Resources.FixObjDescsTitle, Resources.FixObjDescsDescriptions)
		{
			this.maxObjDescLen = maxObjDescLen;
		}

		protected virtual KBObject GetSingleObject(KBModel model, string name)
		{
			return model.Objects.Get(EntityType.DEFAULT_NAMESPACE, new QualifiedName(name));
		}

		protected virtual IEnumerable<KBObject> GetAllObjects(KBModel model)
		{
			foreach (var obj in Transaction.GetAll(model))
				yield return obj;

			foreach (var obj in Procedure.GetAll(model))
				yield return obj;

			foreach (var obj in WorkPanel.GetAll(model))
				yield return obj;
		}

		protected override void ProcessObjectName(KBModel model, string name)
		{
			IOutputService output = CommonServices.Output;
			output.AddLine($"Processing {name}");

			if (name.Equals("*"))
			{
				foreach (var objItem in GetAllObjects(model))
				{
					ProcessObject(output, objItem);
				}
				return;
			}

			var obj = GetSingleObject(model, name);
			if (obj == null)
			{
				output.AddWarningLine($"Could not find Object {name}");
				return;
			}

			ProcessObject(output, obj, true);
		}

		private void ProcessObject(IOutputService output, KBObject obj, bool warnIfNoNeed = false)
		{
			var objDescription = obj.Description;
			if (objDescription.Length <= maxObjDescLen)
			{
				if (warnIfNoNeed)
				{
					output.AddWarningLine($"{obj.TypeDescriptor} {obj.Name} description does not need to be fixed");
				}
				return;
			}

			obj.SetPropertyValue(Properties.TRN.Description, objDescription.Substring(0, maxObjDescLen));
			obj.Save();
			output.AddLine($"{obj.TypeDescriptor} {obj.Name} was adjusted");
		}
	}
}
