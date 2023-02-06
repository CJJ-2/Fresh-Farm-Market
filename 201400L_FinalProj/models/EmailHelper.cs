using Microsoft.AspNetCore.Hosting.Server;
using System.Net;
using System.Net.Mail;

namespace _201400L_FinalProj.models
{
    public class EmailHelper
    {
        public bool SendEmail(string userEmail, string code)
        {
            
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("soysaucewhite@gmail.com");
            mailMessage.To.Add(new MailAddress(userEmail));

            mailMessage.Subject = "2 Factor Code";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = code;

            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            //client.Port = 587;
            client.UseDefaultCredentials = false;
            
            client.Credentials = new System.Net.NetworkCredential("soysaucewhite@gmail.com", "ugftjchgwwnuayjs");
            //client.Host = "smtpout.secureserver.net";
            client.EnableSsl = true;
            //client.DeliveryMethod = SmtpDeliveryMethod.Network;

            

            try
            {
                client.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return false;
        }

        public bool SendEmailConfirm(string userEmail, string code)
        {

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("soysaucewhite@gmail.com");
            mailMessage.To.Add(new MailAddress(userEmail));

            mailMessage.Subject = "Confirm Email";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = code;

            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            //client.Port = 587;
            client.UseDefaultCredentials = false;

            client.Credentials = new System.Net.NetworkCredential("soysaucewhite@gmail.com", "ugftjchgwwnuayjs");
            //client.Host = "smtpout.secureserver.net";
            client.EnableSsl = true;
            //client.DeliveryMethod = SmtpDeliveryMethod.Network;



            try
            {
                client.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return false;
        }

        public bool SendEmailForgetPassword(string userEmail, string code)
        {

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("soysaucewhite@gmail.com");
            mailMessage.To.Add(new MailAddress(userEmail));

            mailMessage.Subject = "Reset Password";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = code;

            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            //client.Port = 587;
            client.UseDefaultCredentials = false;

            client.Credentials = new System.Net.NetworkCredential("soysaucewhite@gmail.com", "ugftjchgwwnuayjs");
            //client.Host = "smtpout.secureserver.net";
            client.EnableSsl = true;
            //client.DeliveryMethod = SmtpDeliveryMethod.Network;



            try
            {
                client.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return false;
        }
    }
}
