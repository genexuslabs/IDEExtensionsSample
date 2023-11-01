using System.Windows.Forms;

namespace GeneXus.Packages.SupportTools.ShortNames
{
    public partial class ShortenNamesDlg : Form
    {
        public bool ShortenAttributeNames
        {
            get => chkShortenAttributes.Checked;
            set => chkShortenAttributes.Checked = value;
        }

        public bool ShortenTableNames
        {
            get => chkShortenTables.Checked;
            set => chkShortenTables.Checked = value;
        }

        public bool ShortenObjectNames
        {
            get => chkShortenObjects.Checked;
            set => chkShortenObjects.Checked = value;
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

        private bool IsAnyCheckboxSet()
        {
            return ShortenAttributeNames || ShortenTableNames || ShortenObjectNames;
        }

        private void chk_CheckedChanged(object sender, System.EventArgs e)
        {
            EnableOrDisableOkButton();
        }

        private void EnableOrDisableOkButton()
        {
            btnOK.Enabled = IsAnyCheckboxSet();
        }
    }
}
