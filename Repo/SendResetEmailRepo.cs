using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace EXhibition.Repo
{
    public class SendResetEmailRepo
    {
        static public void SendResetEmail(string email ,string uuid)
        {

            string account = System.Environment.GetEnvironmentVariable("EMAIL_ACCOUNT");
            string password = System.Environment.GetEnvironmentVariable("EMAIL_PASSWORD");

            if (account == null)
            {
                return; 
            }
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(account, "E展鴻圖");
            mail.To.Add(email);
            mail.Priority = MailPriority.Normal;
            mail.Subject = "密碼重設";
            mail.Body = $"<h1>如要密碼重設!請點擊下列連結</h1>" +
                        $"<a href='https://exhibition.azurewebsites.net/home/ResetPassword/?uuid={uuid}'>重設密碼</a>";

            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new System.Net.NetworkCredential(account, password);
            smtp.EnableSsl = true;
            smtp.Send(mail);
            smtp = null;
            mail.Dispose();

            //MailMessage mail = new MailMessage();
            //mail.From = new MailAddress("tt29334@gmail.com", "tt29334");
            //mail.To.Add("dveasia558@gmail.com");
            //mail.Priority = MailPriority.Normal;
            //mail.Subject = "密碼重設";
            //mail.Body = "<a href='https://localhost:44378/home/ResetPassword/?uuid=" + uuid + "'>重設</a>";
            //mail.IsBodyHtml = true;
            //SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            //smtp.Credentials = new System.Net.NetworkCredential("tt29334@gmail.com", "");
            //smtp.EnableSsl = true;
            //smtp.Send(mail);
            //smtp = null;
            //mail.Dispose();

        }
    }
}