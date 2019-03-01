using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Betburger.Constant;
using Betburger.Controller;

namespace Betburger.View
{
    public partial class frmBookies : Form
    {
        public frmBookies()
        {
            InitializeComponent();
        }

        private void frmBookies_Load(object sender, EventArgs e)
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
            if(chkBet365.Checked)
            {
                if(string.IsNullOrEmpty(txtUserBet365.Text))
                {
                    MessageBox.Show("Please enter bet365 username!");
                    txtUserBet365.Focus();
                    return false;
                }

                if(string.IsNullOrEmpty(txtPassBet365.Text))
                {
                    MessageBox.Show("Please enter bet365 password!");
                    txtPassBet365.Focus();
                    return false;
                }
            }

            if(chkBetfair.Checked)
            {
                if(string.IsNullOrEmpty(txtUserBetfair.Text))
                {
                    MessageBox.Show("Please enter betfair username!");
                    txtUserBetfair.Focus();
                    return false;
                }

                if(string.IsNullOrEmpty(txtPassBetfair.Text))
                {
                    MessageBox.Show("Please enter betfair password!");
                    txtPassBetfair.Focus();
                    return false;
                }

                if(string.IsNullOrEmpty(txtDelayKey.Text))
                {
                    MessageBox.Show("Please enter the betfair delay key!");
                    txtDelayKey.Focus();
                    return false;
                }

                if(string.IsNullOrEmpty(txtRealKey.Text))
                {
                    MessageBox.Show("Please enter the betfair real key!");
                    txtRealKey.Focus();
                    return false;
                }
            }

            if(chkLadbrokes.Checked)
            {
                if(string.IsNullOrEmpty(txtUserLadbrokes.Text))
                {
                    MessageBox.Show("Please enter ladbrokes username!");
                    txtUserLadbrokes.Focus();
                    return false;
                }

                if(string.IsNullOrEmpty(txtPassLadbrokes.Text))
                {
                    MessageBox.Show("Please enter ladbrokes password!");
                    txtPassLadbrokes.Focus();
                    return false;
                }
            }

            if(chkBetdaq.Checked)
            {
                if(string.IsNullOrEmpty(txtUserBetdaq.Text))
                {
                    MessageBox.Show("Please enter pinnacle username!");
                    txtUserBetdaq.Focus();
                    return false;
                }

                if(string.IsNullOrEmpty(txtPassBetdaq.Text))
                {
                    MessageBox.Show("Please enter pinnacle password!");
                    txtPassBetdaq.Focus();
                    return false;
                }
            }

            if(chkMatchbook.Checked)
            {
                if(string.IsNullOrEmpty(txtUserMatchbook.Text))
                {
                    MessageBox.Show("Please enter matchbook username!");
                    txtUserMatchbook.Focus();
                    return false;
                }

                if(string.IsNullOrEmpty(txtPassMatchbook.Text))
                {
                    MessageBox.Show("Please enter matchbook password!");
                    txtPassMatchbook.Focus();
                    return false;
                }
            }

            return true;
        }

        private void initValues()
        {
            chkBet365.Checked = Setting.Instance.bBet365;
            txtUserBet365.Text = Setting.Instance.usernameBet365;
            txtPassBet365.Text = Setting.Instance.passwordBet365;
            chkBetfair.Checked = Setting.Instance.bBetfair;
            txtUserBetfair.Text = Setting.Instance.usernameBetfair;
            txtPassBetfair.Text = Setting.Instance.passwordBetfair;
            txtDelayKey.Text = Setting.Instance.delayKey;
            txtRealKey.Text = Setting.Instance.realKey;
            chkLadbrokes.Checked = Setting.Instance.bLadbrokes;
            txtUserLadbrokes.Text = Setting.Instance.usernameLadbrokes;
            txtPassLadbrokes.Text = Setting.Instance.passwordLadbrokes;
            chkBetdaq.Checked = Setting.Instance.bBetdaq;
            txtUserBetdaq.Text = Setting.Instance.usernameBetdaq;
            txtPassBetdaq.Text = Setting.Instance.passwordBetdaq;
            chkMatchbook.Checked = Setting.Instance.bMatchbook;
            txtUserMatchbook.Text = Setting.Instance.usernameMatchbook;
            txtPassMatchbook.Text = Setting.Instance.passwordMatchbook;

            refreshBet365(chkBet365.Checked);
            refreshBetfair(chkBetfair.Checked);
            refreshBetdaq(chkBetdaq.Checked);
            refreshLadbrokes(chkLadbrokes.Checked);
            refreshMatchbook(chkMatchbook.Checked);
        }

        private void setValues()
        {
            Setting.Instance.bBet365 = chkBet365.Checked;
            Setting.Instance.usernameBet365 = txtUserBet365.Text;
            Setting.Instance.passwordBet365 = txtPassBet365.Text;
            Setting.Instance.bBetfair = chkBetfair.Checked;
            Setting.Instance.usernameBetfair = txtUserBetfair.Text;
            Setting.Instance.passwordBetfair = txtPassBetfair.Text;
            Setting.Instance.delayKey = txtDelayKey.Text;
            Setting.Instance.realKey = txtRealKey.Text;
            Setting.Instance.bLadbrokes = chkLadbrokes.Checked;
            Setting.Instance.usernameLadbrokes = txtUserLadbrokes.Text;
            Setting.Instance.passwordLadbrokes = txtPassLadbrokes.Text;
            Setting.Instance.bBetdaq = chkBetdaq.Checked;
            Setting.Instance.usernameBetdaq = txtUserBetdaq.Text;
            Setting.Instance.passwordBetdaq = txtPassBetdaq.Text;
            Setting.Instance.bMatchbook = chkMatchbook.Checked;
            Setting.Instance.usernameMatchbook = txtUserMatchbook.Text;
            Setting.Instance.passwordMatchbook = txtPassMatchbook.Text;
        }

        private void refreshBet365(bool bState)
        {
            txtUserBet365.Enabled = bState;
            txtPassBet365.Enabled = bState;
        }

        private void refreshBetfair(bool bState)
        {
            txtUserBetfair.Enabled = bState;
            txtPassBetfair.Enabled = bState;
            txtDelayKey.Enabled = bState;
            txtRealKey.Enabled = bState;
        }

        private void refreshBetdaq(bool bState)
        {
            txtUserBetdaq.Enabled = bState;
            txtPassBetdaq.Enabled = bState;
        }

        private void refreshLadbrokes(bool bState)
        {
            txtUserLadbrokes.Enabled = bState;
            txtPassLadbrokes.Enabled = bState;
        }

        private void refreshMatchbook(bool bState)
        {
            txtUserMatchbook.Enabled = bState;
            txtPassMatchbook.Enabled = bState;
        }

        private void chkLadbrokes_CheckedChanged(object sender, EventArgs e)
        {
            refreshLadbrokes(chkLadbrokes.Checked);
        }

        private void chkBet365_CheckedChanged(object sender, EventArgs e)
        {
            refreshBet365(chkBet365.Checked);
        }

        private void chkBetfair_CheckedChanged(object sender, EventArgs e)
        {
            refreshBetfair(chkBetfair.Checked);
        }

        private void chkBetdaq_CheckedChanged(object sender, EventArgs e)
        {
            refreshBetdaq(chkBetdaq.Checked);
        }

        private void chkMatchbook_CheckedChanged(object sender, EventArgs e)
        {
            refreshMatchbook(chkMatchbook.Checked);
        }
    }
}
