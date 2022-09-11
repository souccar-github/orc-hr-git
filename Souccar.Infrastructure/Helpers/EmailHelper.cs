using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Net;

namespace Souccar.Infrastructure.Helpers
{
    /// <summary>
    /// Author: Yaseen Alrefaee
    /// </summary>
    public static class EmailHelper
    {
        public static SendState SendMail( string subject, string body, IList<string> to, IList<string> cc = null, IList<string> bcc = null, IList<Attachment> attachments=null)
        {
            try
            {
                var message = new MailMessage();
                var smtpClient = new SmtpClient();
               
                if (to != null)
                    foreach (var mail in to)
                        message.To.Add(mail);
                if (cc != null)
                    foreach (var mail in cc)
                        message.CC.Add(mail);
                if (bcc != null)
                    foreach (var mail in bcc)
                        message.Bcc.Add(mail);
                if (attachments != null)
                    foreach (var attachment in attachments)
                        message.Attachments.Add(attachment);

                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;


                smtpClient.EnableSsl = true;

             //   ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
             
                smtpClient.Send(message);



                return SendState.Successful;
            }
            catch (Exception e)
            {
               return SendState.UnSuccessful;
            }
        }

    }
    public enum SendState
    {
        Successful,
        UnSuccessful
    }
}
