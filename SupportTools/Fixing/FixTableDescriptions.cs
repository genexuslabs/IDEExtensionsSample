using Artech.Architecture.Common.Objects;
using Artech.Genexus.Common.Objects;
using System.Collections.Generic;

namespace GeneXus.Packages.SupportTools.Fixing
{
	public class FixTableDescriptions : FixObjectDescriptions
	{
		static readonly int defaultMaxTblDescLen = 27;

		public static new bool ExecuteTool(KBModel model)
		{
			var instance = new FixTableDescriptions();
			return instance.Execute(model);
		}

		public FixTableDescriptions()
			: this(FixTableDescriptions.defaultMaxTblDescLen)
		{ }

		public FixTableDescriptions(int maxTblDescLen)
			: base(maxTblDescLen)
		{
			Title = Resources.FixTblDescsTitle;
			Description = Resources.FixTblDescsDescriptions;
		}

		protected override KBObject GetSingleObject(KBModel model, string name)
		{
			return Table.Get(model, name);
		}

		protected override IEnumerable<KBObject> GetAllObjects(KBModel model)
		{
			foreach (Table tbl in Table.GetAll(model))
				yield return tbl;
		}
	}
}
