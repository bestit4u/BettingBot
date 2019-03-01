using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using Betburger.Model;

namespace Betburger.Constant
{
    public class Utils
    {
        private static NumberStyles style = NumberStyles.Number | NumberStyles.AllowCurrencySymbol | NumberStyles.AllowDecimalPoint;
        private static CultureInfo culture = CultureInfo.CreateSpecificCulture("en-GB");

        private static int id { get; set; }

        public static int getId()
        {
            return id++;
        }

        private static string[] currencies = new string[]{
            "$", "USD", "€", "EUR", "£", "GBP"
        };

        public static double getCurrentMoney(string moneyString)
        {
            double moneyValue = 0.0;
            string moneyStringEx = moneyString;

            foreach(string currency in currencies)
            {
                if (moneyStringEx.Contains(currency))
                {
                    moneyStringEx = moneyString.Replace(currency, "").Trim();
                    break;
                }
            }

            if (moneyStringEx.Contains(","))
            {
                if (!moneyStringEx.Contains("."))
                    moneyStringEx = moneyStringEx.Replace(",", ".");
                else
                {
                    if (moneyStringEx.IndexOf(".") < moneyStringEx.IndexOf(","))
                    {
                        moneyStringEx = moneyStringEx.Replace(".", "#");
                        moneyStringEx = moneyStringEx.Replace(",", ".");
                        moneyStringEx = moneyStringEx.Replace("#", ",");
                    }
                }
            }

            double.TryParse(moneyStringEx.Trim(), out moneyValue);

            return moneyValue;
        }

        public static double getOdds(string src)
        {
            if (src.Contains("/"))
            {
                string[] srcs = src.Split(new char[] { '/' }, StringSplitOptions.None);
                if (srcs != null && srcs.Length == 2)
                {
                    double a = 0, b = 0;
                    if (double.TryParse(srcs[0], out a) && double.TryParse(srcs[1], out b) && b != 0)
                        return a / b;
                }
            }

            return ParseToDouble(src) - 1;
        }

        public static void getWinRisk(double stake, ref double win, ref double risk, double odds)
        {
            var j = stake * 1.0;
            var k = 1.0 + odds;
            var o = Math.Round(((j * (k - 1) * 100) / 100), 2);
            win = o;
            risk = stake;
        }

        public static long getTick()
        {
            TimeSpan t = (DateTime.UtcNow - new DateTime(1970, 1, 1));
            long timestamp = (long)t.TotalMilliseconds;
            return timestamp;
        }

        #region Math Function

        public static int ParseToInt(string str)
        {
            int val = 0;
            int.TryParse(str, out val);
            return val;
        }

        public static double ParseToDouble(string str)
        {
            double val = 0;
            double.TryParse(str, style, culture, out val);
            return val;
        }

        public static long ParseToLong(string str)
        {
            long val = 0;
            long.TryParse(str, out val);
            return val;
        }

        public static DateTime ParseToDateTime(string str)
        {
            DateTime val = DateTime.Now;
            DateTime.TryParse(str, out val);
            return val;
        }

        #endregion

        public static void CopyTo(Stream src, Stream dest)
        {
            byte[] bytes = new byte[4096];

            int cnt;

            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
            {
                dest.Write(bytes, 0, cnt);
            }
        }

        public static string Unzip(byte[] bytes)
        {
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                {
                    CopyTo(gs, mso);
                }

                return Encoding.UTF8.GetString(mso.ToArray());
            }
        }

        public static string getRandomNumberString()
        {
            string struid = "";
            Random random = new Random();
            for (int i = 0; i < 17; i++)
            {
                struid += random.Next(0, 10);
            }

            return struid;
        }

        public static string getRandomKey()
        {
            Random rnd = new Random();
            return GetRandomString(rnd, 8);
        }

        public static string GetRandomString(Random rnd, int length)
        {
            string charPool = "abcdefghijklmnopqrstuvwxyz012345";
            return GetRandomString(rnd, length, charPool);
        }

        public static int GetRandomServer()
        {
            Random rnd = new Random();
            return rnd.Next(100, 999);
        }

        public static string GetRandomString(Random rnd, int length, string charPool)
        {
            StringBuilder rs = new StringBuilder();

            while (length-- != 0)
                rs.Append(charPool[(int)(rnd.NextDouble() * charPool.Length)]);

            return rs.ToString();
        }

        public static bool isCheckArbitrage(string odds1Str, string odds2Str, ref double percent)
        {
            double c2 = getOdds(odds1Str) + 1;
            double c3 = getOdds(odds2Str) + 1;

            bool bArb = 1 < (c2 - 1) * (c3 - 1);
            if (!bArb)
                return false;

            double b2 = 1;
            double b3 = b2 * c2 / c3;

            double profit = (c2 - 1) * b2 - b3;

            percent = Math.Round(profit / (b2 + b3) * 100, 2);

            return true;
        }
    }
}
