using Artech.Architecture.Common.Objects;
using Artech.Architecture.Common.Services;
using Artech.Architecture.UI.Framework.Services;
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
	}
}
