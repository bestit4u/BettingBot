using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Betburger.Constant;

namespace Betburger.Controller
{
    public class Setting
    {
        private static Setting _instance = null;

        public static Setting Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Setting();
                }

                return _instance;
            }
        }


        // Bookie Use or UseLess
        public bool bBet365 { get; set; }
        public string usernameBet365 { get; set; }
        public string passwordBet365 { get; set; }
        public bool bBetfair { get; set; }
        public string usernameBetfair { get; set; }
        public string passwordBetfair { get; set; }
        public string delayKey { get; set; }
        public string realKey { get; set; }
        public bool bLadbrokes { get; set; }
        public string usernameLadbrokes { get; set; }
        public string passwordLadbrokes { get; set; }
        public bool bBetdaq { get; set; }
        public string usernameBetdaq { get; set; }
        public string passwordBetdaq { get; set; }
        public bool bMatchbook { get; set; }
        public string usernameMatchbook { get; set; }
        public string passwordMatchbook { get; set; }

        public Setting()
        {
            
        }

        private string ReadRegistry(string KeyName)
        {
            return Registry.CurrentUser.CreateSubKey("SoftWare").CreateSubKey("HorseLive").GetValue(KeyName, (object)"").ToString();
        }

        private void WriteRegistry(string KeyName, string KeyValue)
        {
            Registry.CurrentUser.CreateSubKey("SoftWare").CreateSubKey("HorseLive").SetValue(KeyName, (object)KeyValue);
        }

        public void saveInfo()
        {
            try
            {
                WriteRegistry("bBet365", bBet365.ToString());
                WriteRegistry("usernameBet365", usernameBet365);
                WriteRegistry("passwordBet365", passwordBet365);
                WriteRegistry("bBetfair", bBetfair.ToString());
                WriteRegistry("usernameBetfair", usernameBetfair);
                WriteRegistry("passwordBetfair", passwordBetfair);
                WriteRegistry("delayKey", delayKey);
                WriteRegistry("realKey", realKey);
                WriteRegistry("bLadbrokes", bLadbrokes.ToString());
                WriteRegistry("usernameLadbrokes", usernameLadbrokes);
                WriteRegistry("passwordLadbrokes", passwordLadbrokes);
                WriteRegistry("bBetdaq", bBetdaq.ToString());
                WriteRegistry("usernameBetdaq", usernameBetdaq);
                WriteRegistry("passwordBetdaq", passwordBetdaq);
                WriteRegistry("bMatchbook", bMatchbook.ToString());
                WriteRegistry("usernameMatchbook", usernameMatchbook);
                WriteRegistry("passwordMatchbook", passwordMatchbook);
            }
            catch(Exception e)
            {

            }
        }

        public void loadInfo()
        {
            try
            {
                bBet365 = ReadRegistry("bBet365") == "True";
                usernameBet365 = ReadRegistry("usernameBet365");
                passwordBet365 = ReadRegistry("passwordBet365");
                bBetfair = ReadRegistry("bBetfair") == "True";
                usernameBetfair = ReadRegistry("usernameBetfair");
                passwordBetfair = ReadRegistry("passwordBetfair");
                delayKey = ReadRegistry("delayKey");
                realKey = ReadRegistry("realKey");
                bLadbrokes = ReadRegistry("bLadbrokes") == "True";
                usernameLadbrokes = ReadRegistry("usernameLadbrokes");
                passwordLadbrokes = ReadRegistry("passwordLadbrokes");
                bBetdaq = ReadRegistry("bBetdaq") == "True";
                usernameBetdaq = ReadRegistry("usernameBetdaq");
                passwordBetdaq = ReadRegistry("passwordBetdaq");
                bMatchbook = ReadRegistry("bMatchbook") == "True";
                usernameMatchbook = ReadRegistry("usernameMatchbook");
                passwordMatchbook = ReadRegistry("passwordMatchbook");
            }
            catch(Exception e)
            {

            }
        }
    }
}
