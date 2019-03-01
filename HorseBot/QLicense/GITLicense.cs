using QLicense;
using System.ComponentModel;
using System.Xml.Serialization;
using System;
using System.IO;
using System.Net.Sockets;
using System.Globalization;

namespace GITLicense
{
    public class GITLicense : QLicense.LicenseEntity
    {
        [DisplayName("Expire Date")]
        [Category("License Options")]
        [XmlElement("ExpireDate")]
        [ShowInLicenseInfo(true, "Expire Date", ShowInLicenseInfoAttribute.FormatType.String)]
        public DateTime ExpireDate { get; set; }

        public override LicenseStatus DoExtraValidation(out string validationMsg)
        {
            if (getLocalDateTime() > ExpireDate || getOnlineDateTime() > ExpireDate)
            {
                validationMsg = "Your license has been expired";
                return LicenseStatus.INVALID;
            }

            validationMsg = string.Empty;
            return LicenseStatus.VALID;
        }

        private DateTime getLocalDateTime()
        {
            return DateTime.Now;
        }

        private DateTime getOnlineDateTime()
        {
            try
            {
                var client = new TcpClient("time.nist.gov", 13);
                using (var streamReader = new StreamReader(client.GetStream()))
                {
                    var response = streamReader.ReadToEnd();
                    var utcDateTimeString = response.Substring(7, 17);
                    var localDateTime = DateTime.ParseExact(utcDateTimeString, "yy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
                    return localDateTime;
                }
            }
            catch(Exception)
            {
                return DateTime.Now;
            }
        }
    }
}
