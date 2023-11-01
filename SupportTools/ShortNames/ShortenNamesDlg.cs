using System.Windows.Forms;

namespace Artech.Packages.SupportTools.ShortNames
{
    public partial class ShortenNamesDlg : Form
    {
        public bool ShortenAttributeNames
        {
            get { return chkShortenAttributes.Checked; }
            set { chkShortenAttributes.Checked = value; }
        }

        public bool ShortenTableNames
        {
            get { return chkShortenTables.Checked; }
            set { chkShortenTables.Checked = value; }
        }

        public bool ShortenObjectNames
        {
            get { return chkShortenObjects.Checked; }
            set { chkShortenObjects.Checked = value; }
        }

        private int m_nMaxAttriNameLength;
        private int m_nMaxTableNameLength;
        private int m_nMaxObjectNameLength;

        public ShortenNamesDlg(int nMaxAttriNameLength, int nMaxTableNameLength, int nMaxObjectNameLength)
        {
            m_nMaxAttriNameLength = nMaxAttriNameLength;
            m_nMaxTableNameLength = nMaxTableNameLength;
            m_nMaxObjectNameLength = nMaxObjectNameLength;

            InitializeComponent();

            toolDescription.Text = string.Format(Resources.ShortenNamesToolDescription, m_nMaxAttriNameLength);
            chkShortenAttributes.Text = string.Format(Resources.ShortenNamesAttributesOption, m_nMaxAttriNameLength);
            chkShortenTables.Text = string.Format(Resources.ShortenNamesTablesOption, m_nMaxTableNameLength);
            chkShortenObjects.Text = string.Format(Resources.ShortenNamesObjectsOption, m_nMaxObjectNameLength);

            ShortenAttributeNames = true;
            ShortenTableNames = true;
            ShortenObjectNames = false;
            EnableOrDisableOkButton();
        }

        private bool IsAnyChekboxSet()
        {
            return ShortenAttributeNames || ShortenTableNames || ShortenObjectNames;
        }

        private void chkShortenAttributes_CheckedChanged(object sender, System.EventArgs e)
        {
            EnableOrDisableOkButton();
        }

        private void chkShortenTables_CheckedChanged(object sender, System.EventArgs e)
        {
            EnableOrDisableOkButton();
        }

        private void chkShortenObjects_CheckedChanged(object sender, System.EventArgs e)
        {
            EnableOrDisableOkButton();
        }

        private void EnableOrDisableOkButton()
        {
            btnOK.Enabled = IsAnyChekboxSet();
        }
    }
}
