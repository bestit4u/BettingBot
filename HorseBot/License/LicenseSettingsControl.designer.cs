namespace QLicense.Windows.Controls
{
    partial class LicenseSettingsControl
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LicenseSettingsControl));
            this.grpbxLicenseType = new System.Windows.Forms.GroupBox();
            this.chkExpireType = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.expireDateTime = new System.Windows.Forms.DateTimePicker();
            this.grpbxUniqueID = new System.Windows.Forms.GroupBox();
            this.txtUID = new System.Windows.Forms.TextBox();
            this.grpbxLicenseInfo = new System.Windows.Forms.GroupBox();
            this.pgLicenseSettings = new System.Windows.Forms.PropertyGrid();
            this.btnGenLicense = new System.Windows.Forms.Button();
            this.grpbxLicenseType.SuspendLayout();
            this.grpbxUniqueID.SuspendLayout();
            this.grpbxLicenseInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpbxLicenseType
            // 
            resources.ApplyResources(this.grpbxLicenseType, "grpbxLicenseType");
            this.grpbxLicenseType.Controls.Add(this.chkExpireType);
            this.grpbxLicenseType.Controls.Add(this.label1);
            this.grpbxLicenseType.Controls.Add(this.expireDateTime);
            this.grpbxLicenseType.Name = "grpbxLicenseType";
            this.grpbxLicenseType.TabStop = false;
            this.grpbxLicenseType.Enter += new System.EventHandler(this.grpbxLicenseType_Enter);
            // 
            // chkExpireType
            // 
            resources.ApplyResources(this.chkExpireType, "chkExpireType");
            this.chkExpireType.Name = "chkExpireType";
            this.chkExpireType.UseVisualStyleBackColor = true;
            this.chkExpireType.CheckedChanged += new System.EventHandler(this.chkExpireType_CheckedChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // expireDateTime
            // 
            resources.ApplyResources(this.expireDateTime, "expireDateTime");
            this.expireDateTime.Name = "expireDateTime";
            // 
            // grpbxUniqueID
            // 
            resources.ApplyResources(this.grpbxUniqueID, "grpbxUniqueID");
            this.grpbxUniqueID.Controls.Add(this.txtUID);
            this.grpbxUniqueID.Name = "grpbxUniqueID";
            this.grpbxUniqueID.TabStop = false;
            // 
            // txtUID
            // 
            resources.ApplyResources(this.txtUID, "txtUID");
            this.txtUID.Name = "txtUID";
            // 
            // grpbxLicenseInfo
            // 
            resources.ApplyResources(this.grpbxLicenseInfo, "grpbxLicenseInfo");
            this.grpbxLicenseInfo.Controls.Add(this.pgLicenseSettings);
            this.grpbxLicenseInfo.Name = "grpbxLicenseInfo";
            this.grpbxLicenseInfo.TabStop = false;
            // 
            // pgLicenseSettings
            // 
            resources.ApplyResources(this.pgLicenseSettings, "pgLicenseSettings");
            this.pgLicenseSettings.Name = "pgLicenseSettings";
            this.pgLicenseSettings.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            // 
            // btnGenLicense
            // 
            resources.ApplyResources(this.btnGenLicense, "btnGenLicense");
            this.btnGenLicense.Name = "btnGenLicense";
            this.btnGenLicense.UseVisualStyleBackColor = true;
            this.btnGenLicense.Click += new System.EventHandler(this.btnGenLicense_Click);
            // 
            // LicenseSettingsControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpbxLicenseType);
            this.Controls.Add(this.grpbxUniqueID);
            this.Controls.Add(this.grpbxLicenseInfo);
            this.Controls.Add(this.btnGenLicense);
            this.Name = "LicenseSettingsControl";
            this.grpbxLicenseType.ResumeLayout(false);
            this.grpbxLicenseType.PerformLayout();
            this.grpbxUniqueID.ResumeLayout(false);
            this.grpbxUniqueID.PerformLayout();
            this.grpbxLicenseInfo.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpbxLicenseType;
        private System.Windows.Forms.GroupBox grpbxUniqueID;
        private System.Windows.Forms.TextBox txtUID;
        private System.Windows.Forms.GroupBox grpbxLicenseInfo;
        private System.Windows.Forms.PropertyGrid pgLicenseSettings;
        private System.Windows.Forms.Button btnGenLicense;
        private System.Windows.Forms.CheckBox chkExpireType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker expireDateTime;


    }
}
