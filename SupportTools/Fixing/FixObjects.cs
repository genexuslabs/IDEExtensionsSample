using Artech.Architecture.Common.Objects;
using Artech.Architecture.Common.Services;
using Artech.Architecture.UI.Framework.Services;
using System.Windows.Forms;

namespace GeneXus.Packages.SupportTools.Fixing
{
	public abstract class FixObjects
	{
		protected string Title { get; set; }
		protected string Description { get; set; }

		public FixObjects(string title, string description)
		{
			Title = title;
			Description = description;
		}

		public bool Execute(KBModel model)
		{
			if (model == null)
				return false;

			using FixObjectsDlg dlg = new FixObjectsDlg(Title, Description);
			if (dlg.ShowDialog(UIServices.Environment.MainWindow) != DialogResult.OK)
				return false;

			IOutputService output = CommonServices.Output;
			output.Show(output.GetDefaultOutputId());
			output.StartSection(Title, Title);
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
				output.EndSection(Title, Title, success);
			}

			return success;
		}

		protected abstract void ProcessObjectName(KBModel model, string name);
	}
}
