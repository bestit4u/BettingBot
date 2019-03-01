using System;
using System.Globalization;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Net;
using System.IO;
using System.Net.Http;
using System.Net.Mail;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

using HtmlAgilityPack;

using Betburger.View;
using Betburger.Controller;
using Betburger.Constant;
using Betburger.Model;
using QLicense;
using System.Reflection;
using System.Net.NetworkInformation;

namespace Betburger
{
    public delegate void onWriteStatusEvent(string status);
    public delegate void onWriteLogEvent(string strLog);

    public delegate void onSendMatchEvent(List<MatchInfo> infos);
    public delegate void onSendArbPairListEvent(List<KeyValuePair<ArbInfo, ArbInfo>> arbPairList);

    public partial class frmMain : Form
    {
        //Main Class Variables
        private Point _ptPrevPoint = new Point(0, 0);
        private byte[] _certPubicKeyData;
    
        //log
        public event onWriteStatusEvent onWriteStatus;
        public event onWriteLogEvent onWriteLog;

        // 
        public event onSendMatchEvent onSendMatchBet365;
        public event onSendMatchEvent onSendMatchLadbrokes;
        public event onSendMatchEvent onSendMatchBetdaq;
        public event onSendMatchEvent onSendMatchBetfair;
        public event onSendMatchEvent onSendMatchMatchbook;

        public event onSendArbPairListEvent onSendArbPairList;

        //Betting threads
        private Thread threadBet365;
        private Thread threadLadbrokes;
        private Thread threadBetdaq;
        private Thread threadBetfair;
        private Thread threadMatchbook;

        private Thread threadArbitrage;
        private string filename;

        //variables
        
        public frmMain()
        {
            InitializeComponent();

            onWriteStatus += writeStatus;
            onWriteLog += LogToFile;
            onSendMatchBet365 += sendMatchBet365;
            onSendMatchLadbrokes += sendMatchLadbrokes;

            onSendArbPairList += sendArbPairList;
        }

        private void initControls()
        {
            btnIcon.Parent = picTitle;
            lblTitle.Parent = picTitle;
            btnMin.Parent = picTitle;
            btnMax.Parent = picTitle;
            btnClose.Parent = picTitle;
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

        #region license
        private void checkLicense()
        {
            //Initialize variables with default values
            GITLicense.GITLicense _lic = null;
            string _msg = string.Empty;
            LicenseStatus _status = LicenseStatus.UNDEFINED;

            //Read public key from assembly
            Assembly _assembly = Assembly.GetExecutingAssembly();
            using (MemoryStream _mem = new MemoryStream())
            {
                _assembly.GetManifestResourceStream("Betburger.License.LicenseVerify.cer").CopyTo(_mem);

                _certPubicKeyData = _mem.ToArray();
            }

            //Check if the XML license file exists
            if (File.Exists("license.lic"))
            {
                _lic = (GITLicense.GITLicense)LicenseHandler.ParseLicenseFromBASE64String(
                    typeof(GITLicense.GITLicense),
                    File.ReadAllText("license.lic"),
                    _certPubicKeyData,
                    out _status,
                    out _msg);
            }
            else
            {
                _status = LicenseStatus.INVALID;
                _msg = "Your copy of this application is not activated";
            }

            switch (_status)
            {
                case LicenseStatus.VALID:
                    return;

                default:
                    MessageBox.Show(_msg, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    using (frmActivation frm = new frmActivation())
                    {
                        frm.CertificatePublicKeyData = _certPubicKeyData;
                        frm.ShowDialog();
                        Application.Exit();
                    }
                    break;
            }
        }
        #endregion

        private bool keepService(string user, string pass)
        {
            return true;
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

        private void frmMain_Load(object sender, EventArgs e)
        {
            initControls();

            Setting.Instance.loadInfo();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Log

        private void writeStatus(string status)
        {
            if (rtLog.InvokeRequired)
                rtLog.Invoke(onWriteStatus, status);
            else
            {
                rtLog.AppendText(((string.IsNullOrEmpty(rtLog.Text) ? "" : "\r\n") + string.Format("{1}{0}", status, getLogTitle())));
                rtLog.ScrollToCaret();
            }
        }

        private string getLogTitle()
        {
            string date = string.Format("[{0}] ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            return date;
        }

        private void LogToFile(string result)
        {
            try
            {
                StreamWriter streamWriter = new StreamWriter((Stream)System.IO.File.Open(filename, FileMode.Append, FileAccess.Write, FileShare.Read), Encoding.UTF8);
                if (!string.IsNullOrEmpty(result))
                    streamWriter.WriteLine(result);
                streamWriter.Close();
            }
            catch (System.Exception ex)
            {
            }
        }

        private void writeArbInfo(string timeStamp)
        {
            string fileName = string.Format("arb_{0}.csv", timeStamp);

            string formatString = "\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\"";
            List<string> lines = new List<string>() 
            {
                string.Format(formatString, "Bookmaker", "Meeting", "Race Time", "Horse", "Odds", "Stake", "Started")
            };

            try
            {
                foreach(DataGridViewRow row in tblSureBet.Rows)
                {
                    DataGridViewCellCollection cells = row.Cells;
                    if (cells == null || cells.Count < 7)
                        continue;

                    lines.Add(string.Format(formatString, cells[0].Value, cells[1].Value, cells[2].Value, cells[3].Value, cells[4].Value, cells[5].Value, cells[6].Value));
                }

                File.WriteAllLines(fileName, lines);
            }
            catch(Exception e)
            {

            }
        }

        
        private void btnMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnMax_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                btnMax.BackgroundImage = Betburger.Properties.Resources.restore;
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                btnMax.BackgroundImage = Betburger.Properties.Resources.max;
                this.WindowState = FormWindowState.Normal;
            }
        }

        private void btnBookies_Click(object sender, EventArgs e)
        {
            frmBookies frm = new frmBookies();
            if(frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Setting.Instance.saveInfo();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void picTitle_DoubleClick(object sender, EventArgs e)
        {

        }

        private bool canStart()
        {
            int nBookies = 0;

            if (Setting.Instance.bBet365)
            {
                if (string.IsNullOrEmpty(Setting.Instance.usernameBet365))
                {
                    MessageBox.Show("Please enter Bet365 username!");
                    return false;
                }

                if (string.IsNullOrEmpty(Setting.Instance.passwordBet365))
                {
                    MessageBox.Show("Please enter Bet365 password!");
                    return false;
                }

                nBookies++;
            }

            if (Setting.Instance.bBetfair)
            {
                if (string.IsNullOrEmpty(Setting.Instance.usernameBetfair))
                {
                    MessageBox.Show("Please enter Betfair username!");
                    return false;
                }

                if (string.IsNullOrEmpty(Setting.Instance.passwordBetfair))
                {
                    MessageBox.Show("Please enter Betfair password!");
                    return false;
                }

                nBookies++;
            }

            if(Setting.Instance.bLadbrokes)
            {
                if (string.IsNullOrEmpty(Setting.Instance.usernameLadbrokes))
                {
                    MessageBox.Show("Please enter Ladbrokes username!");
                    return false;
                }

                if (string.IsNullOrEmpty(Setting.Instance.passwordLadbrokes))
                {
                    MessageBox.Show("Please enter Ladbrokes password!");
                    return false;
                }

                nBookies++;
            }

            if(Setting.Instance.bBetdaq)
            {
                if(string.IsNullOrEmpty(Setting.Instance.usernameBetdaq))
                {
                    MessageBox.Show("Please enter betdaq username!");
                    return false;
                }

                if(string.IsNullOrEmpty(Setting.Instance.passwordBetdaq))
                {
                    MessageBox.Show("Please enter betdaq password!");
                    return false;
                }

                nBookies++;
            }

            if (Setting.Instance.bMatchbook)
            {
                if (string.IsNullOrEmpty(Setting.Instance.usernameMatchbook))
                {
                    MessageBox.Show("Please enter Matchbook username!");
                    return false;
                }

                if (string.IsNullOrEmpty(Setting.Instance.passwordMatchbook))
                {
                    MessageBox.Show("Please enter Matchbook password!");
                    return false;
                }

                nBookies++;
            }

            if (nBookies < 1)
            {
                MessageBox.Show("You have to check 2 bookies at least!");
                return false;
            }
            
            return true;
        }

        private void refreshControls(bool bState)
        {
            btnBookies.Enabled = bState;
            btnStart.Enabled = bState;
            btnStop.Enabled = !bState;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (!canStart())
                return;

            Setting.Instance.saveInfo();

            Constants.bRun = true;
            onWriteStatus("The bot has been started!");
            refreshControls(false);
            filename = string.Format("{0}.txt", DateTime.Now.ToString("yyyyMMddHHmmss"));

            if(Setting.Instance.bBet365)
            {
                threadBet365 = new Thread(funcBet365);
                threadBet365.Start();
            }

            if(Setting.Instance.bLadbrokes)
            {
                threadLadbrokes = new Thread(funcLadbrokes);
                threadLadbrokes.Start();
            }

            if(Setting.Instance.bBetdaq)
            {
                threadBetdaq = new Thread(funcBetdaq);
                threadBetdaq.Start();
            }

            if(Setting.Instance.bBetfair)
            {
                threadBetfair = new Thread(funcBetfair);
                threadBetfair.Start();
            }

            if(Setting.Instance.bMatchbook)
            {
                threadMatchbook = new Thread(funcMatchbook);
                threadMatchbook.Start();
            }

            threadArbitrage = new Thread(funcArbitrage);
            //threadArbitrage.Start();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Setting.Instance.saveInfo();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            Constants.bRun = false;

            try
            {
                if (threadBet365 != null)
                    threadBet365.Abort();

                if (threadLadbrokes != null)
                    threadLadbrokes.Abort();

                if (threadArbitrage != null)
                    threadArbitrage.Abort();
            }
            catch(Exception)
            {

            }

            onWriteStatus("The bot has been stopped!");
            refreshControls(true);
        }

        private void funcBet365()
        {
            Bet365Ctrl task = new Bet365Ctrl(onWriteStatus, onSendMatchBet365, onWriteLog);
            task.doWork();
        }

        private void funcLadbrokes()
        {
            LadbrokesCtrl task = new LadbrokesCtrl(onWriteStatus, onSendMatchLadbrokes, onWriteLog);
            task.doWork();
        }

        private void funcBetdaq()
        {
            BetdaqCtrl task = new BetdaqCtrl(onWriteStatus, onSendMatchBetdaq);
            task.doWork();
        }

        private void funcBetfair()
        {
            BetfairCtrl task = new BetfairCtrl(onWriteStatus, onSendMatchBetfair);
            task.doWork();
        }

        private void funcMatchbook()
        {
            MatchbookCtrl task = new MatchbookCtrl(onWriteStatus, onSendMatchMatchbook);
            task.doWork();
        }

        private void sendMatchBet365(List<MatchInfo> infos)
        {
            lock(ArbitrageMng.Instance.infoBet365)
            {
                ArbitrageMng.Instance.setBet365List(infos);
            }
        }

        private void sendMatchLadbrokes(List<MatchInfo> infos)
        {
            lock (ArbitrageMng.Instance.infoLadbrokes)
            {
                ArbitrageMng.Instance.setLadbrokesList(infos);
            }
        }

        private void sendMatchBetdaq(List<MatchInfo> infos)
        {
            lock(ArbitrageMng.Instance.infoBet365)
            {
                ArbitrageMng.Instance.setBetdaqList(infos);
            }
        }

        private void sendMatchBetfair(List<MatchInfo> infos)
        {
            lock(ArbitrageMng.Instance.infoBetfair)
            {
                ArbitrageMng.Instance.setBetfairList(infos);
            }
        }

        private void sendMatchMatchbook(List<MatchInfo> infos)
        {
            lock(ArbitrageMng.Instance.infoMatchbook)
            {
                ArbitrageMng.Instance.setMatchbookList(infos);
            }
        }

        private void sendArbPairList(List<KeyValuePair<ArbInfo, ArbInfo>> arbPairList)
        {
            List<ArbInfo> arbInfoList = new List<ArbInfo>();

            foreach(KeyValuePair<ArbInfo, ArbInfo> arbPair in arbPairList)
            {
                arbInfoList.Add(arbPair.Key);
                arbInfoList.Add(arbPair.Value);
            }

            displayArbPairTable(arbInfoList.OrderByDescending(o => o.percent).ToList());
        }

        private void displayArbPairTable(List<ArbInfo> infoList)
        {
            try
            {
                this.Invoke(new Action(() =>
                {
                    bindingSourceArb.DataSource = infoList;
                    bindingSourceArb.ResetBindings(false);
                }));
            }
            catch(Exception e)
            {

            }
        }

        private void funcArbitrage()
        {
            ArbitrageMng.Instance.setEvent(onSendArbPairList);

            while(Constants.bRun)
            {
                Thread.Sleep(10000);
                ArbitrageMng.Instance.doWork();
            }
        }
    }
}
