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
		public FixProcsDlg()
		{
			InitializeComponent();
			toolDescription.Text = Resources.FixProcsToolDescription;
		}
	}
}
