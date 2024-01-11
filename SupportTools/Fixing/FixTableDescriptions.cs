using Artech.Architecture.Common.Objects;
using Artech.Architecture.Common.Services;
using Artech.Genexus.Common;
using Artech.Genexus.Common.Objects;

namespace GeneXus.Packages.SupportTools.Fixing
{
	public class FixTableDescriptions : FixObjects
	{
		static readonly int defaultMaxTblDescLen = 27;
		public static bool ExecuteTool(KBModel model)
		{
			var instance = new FixTableDescriptions();
			return instance.Execute(model);
		}

		private readonly int maxTblDescLen;

		public FixTableDescriptions()
			: this(FixTableDescriptions.defaultMaxTblDescLen) { }

		public FixTableDescriptions(int maxTblDescLen)
			: base(Resources.FixTblDescsTitle, Resources.FixTblDescsDescriptions)
		{
			this.maxTblDescLen = maxTblDescLen;
		}

		protected override void ProcessObjectName(KBModel model, string name)
		{
			IOutputService output = CommonServices.Output;
			output.AddLine($"Processing {name}");

			if (name.Equals("*"))
			{
				foreach (Table tbl in Table.GetAll(model))
				{
					ProcessTable(output, tbl);
				}
				return;
			}

			KBObject table = Table.Get(model, name);
			if (table == null)
			{
				output.AddWarningLine($"Could not find Table {name}");
				return;
			}

			ProcessTable(output, table);
		}

		private void ProcessTable(IOutputService output, KBObject table)
		{
			var tblDescription = table.Description;
			if (tblDescription.Length <= maxTblDescLen)
			{
				output.AddWarningLine($"Table {table.Name} description does not need to be fixed");
				return;
			}

			table.SetPropertyValue(Properties.TBL.Description, tblDescription.Substring(0, maxTblDescLen));
			table.Save();
			output.AddLine($"Table {table.Name} was adjusted");
		}
	}
}
