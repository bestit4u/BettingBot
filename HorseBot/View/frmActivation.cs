using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Betburger.View
{
    public partial class frmActivation : Form
    {
        public byte[] CertificatePublicKeyData { private get; set; }
        private Point _ptPrevPoint = new Point(0, 0);

        public frmActivation()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure to cancel?", string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void initControls()
        {
            lblTitle.Parent = picTitle;
            btnClose.Parent = picTitle;
            btnKey.Parent = picTitle;
        }

        private void frmActivation_Load(object sender, EventArgs e)
        {
            //
            initControls();

            //Assign the application information values to the license control
            licActCtrl.AppName = "Horse Racing";
            licActCtrl.LicenseObjectType = typeof(GITLicense.GITLicense);
            licActCtrl.CertificatePublicKeyData = this.CertificatePublicKeyData;
            //Display the device unique ID
            licActCtrl.ShowUID();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            //Call license control to validate the license string
            if (licActCtrl.ValidateLicense())
            {
                //If license if valid, save the license string into a local file
                File.WriteAllText(Path.Combine(Application.StartupPath, "license.lic"), licActCtrl.LicenseBASE64String);
                MessageBox.Show("License accepted, the application will be close. Please restart it later", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void picTitle_MouseDown(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Default;
            this._ptPrevPoint.X = e.X;
            this._ptPrevPoint.Y = e.Y;
        }

        private void picTitle_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            Point point = new Point(e.X - this._ptPrevPoint.X, e.Y - this._ptPrevPoint.Y);
            point.X = this.Location.X + point.X;
            point.Y = this.Location.Y + point.Y;
            this.Location = point;
        }
    }
}
