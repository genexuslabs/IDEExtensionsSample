using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeneXus.Packages.SupportTools.FixingProcs
{
	public partial class FixProcsDlg : Form
	{

		private string ObjectsSpecification
		{
			get => editObjects.Text;
			set => editObjects.Text = value;
		}


		public FixProcsDlg()
		{
			InitializeComponent();
			toolDescription.Text = Resources.FixProcsToolDescription;
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
	}
}
