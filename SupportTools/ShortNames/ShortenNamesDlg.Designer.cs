namespace GeneXus.Packages.SupportTools.ShortNames
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShortenNamesDlg));
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
            resources.ApplyResources(this.toolDescription, "toolDescription");
            this.toolDescription.Name = "toolDescription";
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // chkShortenAttributes
            // 
            resources.ApplyResources(this.chkShortenAttributes, "chkShortenAttributes");
            this.chkShortenAttributes.Name = "chkShortenAttributes";
            this.chkShortenAttributes.UseVisualStyleBackColor = true;
            this.chkShortenAttributes.CheckedChanged += new System.EventHandler(this.chk_CheckedChanged);
            // 
            // chkShortenTables
            // 
            resources.ApplyResources(this.chkShortenTables, "chkShortenTables");
            this.chkShortenTables.Name = "chkShortenTables";
            this.chkShortenTables.UseVisualStyleBackColor = true;
            this.chkShortenTables.CheckedChanged += new System.EventHandler(this.chk_CheckedChanged);
            // 
            // chkShortenObjects
            // 
            resources.ApplyResources(this.chkShortenObjects, "chkShortenObjects");
            this.chkShortenObjects.Name = "chkShortenObjects";
            this.chkShortenObjects.UseVisualStyleBackColor = true;
            this.chkShortenObjects.CheckedChanged += new System.EventHandler(this.chk_CheckedChanged);
            // 
            // ShortenNamesDlg
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
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