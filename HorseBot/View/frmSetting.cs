using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Betburger.Controller;
using Betburger.Constant;

namespace Betburger.View
{
    public partial class frmSetting : Form
    {
        public frmSetting()
        {
            InitializeComponent();
        }

        private void frmSetting_Load(object sender, EventArgs e)
        {
            initValues();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!canSetting())
                return;

            setValues();

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private bool canSetting()
        {
            if(string.IsNullOrEmpty(txtStake.Text))
            {
                MessageBox.Show("Please enter the stake!");
                txtStake.Focus();
                return false;
            }

            return true;
        }

        private void initValues()
        {
            
        }

        private void setValues()
        {
            
        }

        private void txtStake_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back) || e.KeyChar == 46))
                e.Handled = true;
        }
    }
}
