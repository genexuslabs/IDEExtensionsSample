using Artech.Architecture.Common.Objects;
using Artech.Architecture.Common.Services;
using Artech.Architecture.UI.Framework.Services;
using Artech.Genexus.Common;
using Artech.Genexus.Common.Objects;
using Artech.Genexus.Common.Parts.Layout;
using System;
using System.Linq;
using System.Windows.Forms;

namespace GeneXus.Packages.SupportTools.FixingProcs
{
	public class FixProcs
	{
		private const string ProcsFixingSection = "FixProcs";

		public static bool Execute(KBModel model)
		{
			if (model == null)
				return false;

			using FixProcsDlg dlg = new FixProcsDlg();
			if (dlg.ShowDialog(UIServices.Environment.MainWindow) != DialogResult.OK)
				return false;

			IOutputService output = CommonServices.Output;
			output.Show(output.GetDefaultOutputId());
			output.StartSection(ProcsFixingSection, Resources.FixProcsSections);
			bool success = false;

			try
			{
				using (KnowledgeBase.Transaction transaction = model.KB.BeginTransaction())
				{
					foreach (string name in dlg.ObjectNames)
					{
						ProcessObjectName(model, name);
					}

					transaction.Commit();
					success = true;
				}
			}
			catch (System.Exception exception)
			{
				output.AddErrorLine(exception);
			}
			finally
			{
				output.EndSection(ProcsFixingSection, Resources.FixProcsSections, success);
			}

			return success;
		}

		private static void ProcessObjectName(KBModel model, string name)
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
