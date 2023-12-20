using Artech.Architecture.Common.Objects;
using Artech.Architecture.Common.Services;
using Artech.Genexus.Common;
using Artech.Genexus.Common.Objects;
using Artech.Genexus.Common.Parts.Layout;
using System;
using System.Linq;

namespace GeneXus.Packages.SupportTools.Fixing
{
	public class FixProcs : FixObjects
	{
		public FixProcs()
			: base (Resources.FixProcsTitle, Resources.FixProcsToolDescription)
		{}

		public static bool ExecuteTool(KBModel model)
		{
			var instance = new FixProcs();
			return instance.Execute(model);
		}

		protected override void ProcessObjectName(KBModel model, string name)
		{
			IOutputService output = CommonServices.Output;
			output.AddLine($"Processing {name}");

			Procedure proc = Procedure.Get(model, new QualifiedName(name));
			if (proc == null)
			{
				output.AddWarningLine($"Could not find Procedure '{name}'");
				return;
			}

			PrintBlock firstPrintBlock = proc.Layout.Layout.ReportBands.FirstOrDefault() as PrintBlock;
			if (firstPrintBlock == null)
			{
				output.AddWarningLine($"Procedure '{name}' has no print blocks");
				return;
			}

			int columns = firstPrintBlock.GetPropertyValue<int>(Properties.RPT_PRINTB.Width);
			// safety extra column
			// columns++

			double twips = Convert.ToDouble((columns) * 120);
			int paperWidth = Convert.ToInt32(Math.Ceiling(twips * 100 / 1440));
			output.AddLine($"Procedure '{name}' has {columns} columns, {twips} twips, {paperWidth} paperWidth");

			if (paperWidth == proc.Layout.Layout.PaperWidth)
			{
				output.AddLine($"Procedure '{name}' does not need to be adjusted");
				return;
			}

			proc.Layout.Layout.RightMargin = 0;
			proc.Layout.Layout.PaperSize = Properties.RPT_LAYOUT.PaperSize_Enum.Custom;
			proc.Layout.Layout.PaperWidth = paperWidth;
			proc.Layout.Dirty = true;
			proc.Save();
			output.AddLine($"Procedure '{name}' adjusted");
		}
	}
}
