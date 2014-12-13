using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.Configuration;

namespace FlyableHours
{
    class GmailMailService
    {
        public GmailMailService ()
	    {

	    }

        // Note: need to enable less secure access to the gmail account at
        // https://www.google.com/settings/security/lesssecureapps
        public void senedGmailMail(String body)
        {
            var fromAddress = new MailAddress(ConfigurationManager.AppSettings["gmailFromAddress"], "Flyable Hours");
            var toAddress = new MailAddress(ConfigurationManager.AppSettings["mailToAddress"], "");
            #region mail-password
            string gmailPassword = ConfigurationManager.AppSettings["gmailPassword"];
            #endregion
            const string subject = "Flygtimmar!";
            NetworkCredential googleCreds = new NetworkCredential(fromAddress.Address, gmailPassword);
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, gmailPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }


        }
    }
}
