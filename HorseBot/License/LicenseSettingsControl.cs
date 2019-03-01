using System;
using System.Security;
using System.Windows.Forms;

namespace QLicense.Windows.Controls
{
    public delegate void LicenseSettingsValidatingHandler(object sender, LicenseSettingsValidatingEventArgs e);
    public delegate void LicenseGeneratedHandler(object sender, LicenseGeneratedEventArgs e);

    public partial class LicenseSettingsControl : UserControl
    {

        public event LicenseSettingsValidatingHandler OnLicenseSettingsValidating;
        public event LicenseGeneratedHandler OnLicenseGenerated;

        protected GITLicense.GITLicense _lic;

        public GITLicense.GITLicense License
        {
            set
            {
                _lic = value;
                pgLicenseSettings.SelectedObject = _lic;
            }
        }

        public byte[] CertificatePrivateKeyData { set; private get; }

        public SecureString CertificatePassword { set; private get; }

       
        public LicenseSettingsControl()
        {
            InitializeComponent();
        }


        private void LicenseTypeRadioButtons_CheckedChanged(object sender, EventArgs e)
        {
            txtUID.Text = string.Empty;
            txtUID.Enabled = true;
        }

        private void btnGenLicense_Click(object sender, EventArgs e)
        {
            if (_lic == null) throw new ArgumentException("LicenseEntity is invalid");

            if (LicenseHandler.ValidateUIDFormat(txtUID.Text.Trim()))
            {
                _lic.Type = LicenseTypes.Single;
                _lic.UID = txtUID.Text.Trim();
            }
            else
            {
                MessageBox.Show("License UID is blank or invalid", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (chkExpireType.Checked)
                _lic.ExpireDate = new DateTime(2100, 1, 1);
            else
                _lic.ExpireDate = expireDateTime.Value;

            _lic.CreateDateTime = DateTime.Now;
            if (OnLicenseSettingsValidating != null)
            {
                LicenseSettingsValidatingEventArgs _args = new LicenseSettingsValidatingEventArgs() { License = _lic, CancelGenerating = false };

                OnLicenseSettingsValidating(this, _args);

                if (_args.CancelGenerating)
                {
                    return;
                }
            }

            if (OnLicenseGenerated != null)
            {
                string _licStr = LicenseHandler.GenerateLicenseBASE64String(_lic, CertificatePrivateKeyData, CertificatePassword);
                OnLicenseGenerated(this, new LicenseGeneratedEventArgs() { LicenseBASE64String = _licStr });
            }
        }

        private void grpbxLicenseType_Enter(object sender, EventArgs e)
        {

        }

        private void chkExpireType_CheckedChanged(object sender, EventArgs e)
        {
            if (chkExpireType.Checked)
                expireDateTime.Enabled = false;
            else
                expireDateTime.Enabled = true;
        }
    }
}
