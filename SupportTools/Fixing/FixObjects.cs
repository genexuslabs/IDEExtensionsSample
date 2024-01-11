using Artech.Architecture.Common.Objects;
using Artech.Architecture.Common.Services;
using Artech.Architecture.UI.Framework.Services;
using System.Collections.Generic;
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

		protected abstract KBObject GetSingleObject(KBModel model, string name);

		protected abstract IEnumerable<KBObject> GetAllObjects(KBModel model);

		protected abstract void ProcessObject(KBObject obj, bool warnIfNoNeed = false);

		protected virtual void ProcessObjectName(KBModel model, string name)
		{
			IOutputService output = CommonServices.Output;

			if (name.Equals("*"))
			{
				foreach (var objItem in GetAllObjects(model))
				{
					ProcessObject(objItem);
				}
				return;
			}

			var obj = GetSingleObject(model, name);
			if (obj == null)
			{
				output.AddWarningLine($"Could not find Object {name}");
				return;
			}

			ProcessObject(obj, true);
		}
	}
}
