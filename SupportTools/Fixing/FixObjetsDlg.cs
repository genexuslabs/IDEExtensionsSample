using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GeneXus.Packages.SupportTools.Fixing
{
	public partial class FixObjetsDlg : Form
	{

		private string ObjectsSpecification
		{
			get => editObjects.Text;
			set => editObjects.Text = value;
		}


		public FixObjetsDlg(string title, string description)
		{
			InitializeComponent();
			Text = title;
			toolDescription.Text = description;
		}

		private int lastConvertedHash = 0;
		private string[] objectNames = new string[] { };

		public IEnumerable<string> ObjectNames
		{
			get
			{
				if (ObjectsSpecification.GetHashCode() == lastConvertedHash)
				{
					return objectNames;
				}

				objectNames = ObjectsSpecification.Split(new char[] { ' ', ',', ';', '\r', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries);
				lastConvertedHash = ObjectsSpecification.GetHashCode();

				return objectNames;
			}
		}

		public IEnumerable<Tuple<string, string>> ObjectItems
		{
			get
			{
				var enumerator = ObjectNames.GetEnumerator();
				while (enumerator.MoveNext())
				{
					var objectName = enumerator.Current;
					if (enumerator.MoveNext())
					{
						var itemName = enumerator.Current;
						yield return new Tuple<string, string>(objectName, itemName);
					}
					else
					{
						yield return new Tuple<string, string>(objectName, string.Empty);
					}
				}
		
			}
		}
	}
}
