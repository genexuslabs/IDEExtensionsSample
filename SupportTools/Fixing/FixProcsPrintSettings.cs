using Artech.Architecture.Common.Objects;
using Artech.Architecture.Common.Services;
using Artech.Genexus.Common;
using Artech.Genexus.Common.Objects;
using Artech.Genexus.Common.Parts.Layout;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneXus.Packages.SupportTools.Fixing
{
	public class FixProcsPrintSettings : FixObjects
	{
		public FixProcsPrintSettings()
			: base (Resources.FixProcsTitle, Resources.FixProcsToolDescription)
		{}

		public static bool ExecuteTool(KBModel model)
		{
			var instance = new FixProcsPrintSettings();
			return instance.Execute(model);
		}

		protected override KBObject GetSingleObject(KBModel model, string name)
		{
			return Procedure.Get(model, new QualifiedName(name));
		}

		protected override IEnumerable<KBObject> GetAllObjects(KBModel model)
		{
			return Procedure.GetAll(model);
		}

		protected override void ProcessObject(KBObject obj, bool warnIfNoNeed = false)
		{
			IOutputService output = CommonServices.Output;
			string typeName = $"{obj.TypeDescriptor.Description} '{obj.Name}'";

			Procedure proc = obj as Procedure;
			if (proc == null)
			{
				output.AddWarningLine($"{typeName} is not a Procedure");
				return;
			}

			PrintBlock firstPrintBlock = proc.Layout.Layout.ReportBands.FirstOrDefault() as PrintBlock;
			if (firstPrintBlock == null)
			{
				if (warnIfNoNeed)
				{
					output.AddWarningLine($"{typeName} has no print blocks");
				}
				return;
			}

			int columns = firstPrintBlock.GetPropertyValue<int>(Properties.RPT_PRINTB.Width);
			// safety extra column
			// columns++

			double twips = Convert.ToDouble((columns) * 120);
			int paperWidth = Convert.ToInt32(Math.Ceiling(twips * 100 / 1440));
			output.AddLine($"{typeName} has {columns} columns, {twips} twips, {paperWidth} paperWidth");

			if (paperWidth == proc.Layout.Layout.PaperWidth)
			{
				if (warnIfNoNeed)
				{
					output.AddLine($"{typeName} does not need to be adjusted");
				}
				return;
			}

			proc.Layout.Layout.RightMargin = 0;
			proc.Layout.Layout.PaperSize = Properties.RPT_LAYOUT.PaperSize_Enum.Custom;
			proc.Layout.Layout.PaperWidth = paperWidth;
			proc.Layout.Dirty = true;
			proc.Save();
			output.AddLine($"{typeName} adjusted");
		}
	}
}
