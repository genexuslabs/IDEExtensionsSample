using Artech.Architecture.Common.Objects;
using Artech.Architecture.Common.Services;
using Artech.Architecture.UI.Framework.Services;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GeneXus.Packages.SupportTools.Fixing
{
	public abstract class FixObjectItems
	{
		public class ObjectAndItems
		{
			public string Name { get; set; }
			public List<string> Items { get; set; } = new List<string>();

			public ObjectAndItems(string name)
			{
				Name = name.Trim();
			}
		}

		protected string Title { get; set; }
		protected string Description { get; set; }

		public FixObjectItems(string title, string description)
		{
			Title = title;
			Description = description;
		}

		public virtual bool Execute(KBModel model)
		{
			if (model == null)
				return false;

			using FixObjectsDlg dlg = new FixObjectsDlg(Title, Description);
			if (dlg.ShowDialog(UIServices.Environment.MainWindow) != DialogResult.OK)
				return false;

			IOutputService output = CommonServices.Output;
			string outputId = output.GetDefaultOutputId();
			output.SelectOutput(outputId);
			output.Show(outputId);
			output.StartSection(Title, Title);
			bool success = false;

			try
			{
				using (KnowledgeBase.Transaction transaction = model.KB.BeginTransaction())
				{
					ObjectAndItems currentObject = null;
					foreach (var objVar in dlg.ObjectItems)
					{
						if (currentObject == null || objVar.Item1 != currentObject.Name)
						{
							if (currentObject != null)
							{
								ProcessObjectAndItems(model, currentObject);
							}
							currentObject = new ObjectAndItems(objVar.Item1);
						}
						currentObject.Items.Add(objVar.Item2);
					}

					if (currentObject != null)
					{
						ProcessObjectAndItems(model, currentObject);
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

		protected abstract void ProcessObjectAndItems(KBModel model, ObjectAndItems obj);
	}
}
