using Artech.Architecture.Common.Objects;
using Artech.Architecture.Common.Services;

namespace GeneXus.Packages.SupportTools.FixingProcs
{
	public class FixProcs
	{
		private const string ProcsFixingSection = "FixProcs";

		public static bool Execute(KBModel model)
		{
			if (model == null)
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
