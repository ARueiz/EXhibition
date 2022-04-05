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
            //MailMessage mail = new MailMessage();
            //mail.From = new MailAddress("tt29334@gmail.com","tt29334");
            //mail.To.Add("dveasia558@gmail.com");
            //mail.Priority = MailPriority.Normal;
            //mail.Subject = "密碼重設";
            //mail.Body = "<a href='https://localhost:44378/home/ResetPassword/?uuid='"+ uuid +">重設</a>";
            //mail.IsBodyHtml = true;
            //SmtpClient smtp = new SmtpClient("smtp.gmail.com",587);
            //smtp.Credentials = new System.Net.NetworkCredential("tt29334@gmail.com","");
            //smtp.EnableSsl = true;
            //smtp.Send(mail);
            //smtp = null;
            //smtp.Dispose();
        }
    }
}