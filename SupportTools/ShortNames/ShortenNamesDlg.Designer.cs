namespace Artech.Packages.SupportTools.ShortNames
{
    partial class ShortenNamesDlg
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.toolDescription = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.chkShortenAttributes = new System.Windows.Forms.CheckBox();
            this.chkShortenTables = new System.Windows.Forms.CheckBox();
            this.chkShortenObjects = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // toolDescription
            // 
            this.toolDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.toolDescription.Location = new System.Drawing.Point(12, 9);
            this.toolDescription.Name = "toolDescription";
            this.toolDescription.Size = new System.Drawing.Size(577, 124);
            this.toolDescription.TabIndex = 0;
            this.toolDescription.Text = "Description taken from Resources.ShortenNamesToolDescription";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(514, 225);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(433, 225);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // chkShortenAttributes
            // 
            this.chkShortenAttributes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkShortenAttributes.AutoSize = true;
            this.chkShortenAttributes.Location = new System.Drawing.Point(28, 146);
            this.chkShortenAttributes.Name = "chkShortenAttributes";
            this.chkShortenAttributes.Size = new System.Drawing.Size(334, 17);
            this.chkShortenAttributes.TabIndex = 3;
            this.chkShortenAttributes.Text = "Description taken from Resources.ShortenNamesAttributesOption";
            this.chkShortenAttributes.UseVisualStyleBackColor = true;
            this.chkShortenAttributes.CheckedChanged += new System.EventHandler(this.chkShortenAttributes_CheckedChanged);
            // 
            // chkShortenTables
            // 
            this.chkShortenTables.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkShortenTables.AutoSize = true;
            this.chkShortenTables.Location = new System.Drawing.Point(28, 169);
            this.chkShortenTables.Name = "chkShortenTables";
            this.chkShortenTables.Size = new System.Drawing.Size(322, 17);
            this.chkShortenTables.TabIndex = 3;
            this.chkShortenTables.Text = "Description taken from Resources.ShortenNamesTablesOption";
            this.chkShortenTables.UseVisualStyleBackColor = true;
            this.chkShortenTables.CheckedChanged += new System.EventHandler(this.chkShortenTables_CheckedChanged);
            // 
            // chkShortenObjects
            // 
            this.chkShortenObjects.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkShortenObjects.AutoSize = true;
            this.chkShortenObjects.Location = new System.Drawing.Point(28, 192);
            this.chkShortenObjects.Name = "chkShortenObjects";
            this.chkShortenObjects.Size = new System.Drawing.Size(326, 17);
            this.chkShortenObjects.TabIndex = 3;
            this.chkShortenObjects.Text = "Description taken from Resources.ShortenNamesObjectsOption";
            this.chkShortenObjects.UseVisualStyleBackColor = true;
            this.chkShortenObjects.CheckedChanged += new System.EventHandler(this.chkShortenObjects_CheckedChanged);
            // 
            // ShortenNamesDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(601, 260);
            this.Controls.Add(this.chkShortenObjects);
            this.Controls.Add(this.chkShortenTables);
            this.Controls.Add(this.chkShortenAttributes);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.toolDescription);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ShortenNamesDlg";
            this.ShowIcon = false;
            this.Text = "Shorten Names";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label toolDescription;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.CheckBox chkShortenAttributes;
        private System.Windows.Forms.CheckBox chkShortenTables;
        private System.Windows.Forms.CheckBox chkShortenObjects;
    }
}