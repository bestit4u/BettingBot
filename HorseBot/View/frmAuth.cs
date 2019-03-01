using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Betburger.View
{
    public partial class frmAuth : Form
    {
        public bool bAuth = false;
        private Point _ptPrevPoint = new Point(0, 0);

        private int nCount = 0;

        public string username = string.Empty;
        public string password = string.Empty;

        public frmAuth()
        {
            InitializeComponent();
            initControl();
        }

        private void initControl()
        {
            btnKey.Parent = picTitle;
            lblTitle.Parent = picTitle;
            btnClose.Parent = picTitle;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            doWebAuth();
        }

        private void doWebAuth()
        {
            if (string.IsNullOrEmpty(txtUsername.Text))
            {
                MessageBox.Show("Please enter the username!");
                txtUsername.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("Please enter the password!");
                txtPassword.Focus();
                return;
            }

            try
            {
                username = txtUsername.Text;
                password = txtPassword.Text;

                if (keepService(username, password))
                {
                    bAuth = true;
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Invalid account!");
                    if (nCount < 2)
                    {
                        nCount++;
                        txtPassword.SelectAll();
                        return;
                    }
                    else
                    {
                        this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                        this.Close();
                    }
                }

            }
            catch (Exception)
            {

            }
        }

        private bool keepService(string user, string pass)
        {
            try
            {
                HttpClient httpClientKeep = new HttpClient();
                HttpResponseMessage responseMessageKeep = httpClientKeep.PostAsync("http://176.74.20.79:8080/horse/keep.php", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)new KeyValuePair<string, string>[3]{
                    new KeyValuePair<string, string>("user", user),
                    new KeyValuePair<string, string>("pass", pass),
                    new KeyValuePair<string, string>("mac", getMacAddress())
                })).Result;
                responseMessageKeep.EnsureSuccessStatusCode();

                string responseMessageKeepString = responseMessageKeep.Content.ReadAsStringAsync().Result;
                if (string.IsNullOrEmpty(responseMessageKeepString))
                    return false;

                if (responseMessageKeepString != "success")
                    return false;

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private string getMacAddress()
        {
            return NetworkInterface.GetAllNetworkInterfaces()[0].GetPhysicalAddress().ToString();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void txtKey_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                doWebAuth();
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
