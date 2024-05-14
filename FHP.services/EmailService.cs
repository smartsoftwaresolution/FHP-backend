using FHP.entity.UserManagement;
using FHP.infrastructure.Service;
using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace FHP.services
{
    public class EmailService : IEmailService
    {
       

        public async Task SendverificationEmail(string email,int userId)
        {
            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress("   "));
            message.To.Add(new MailboxAddress(email));
            message.Subject = "Email Verfication";
            string emailBody = "http://localhost:3000/email-verification/" + userId;
            BodyBuilder bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = emailBody;

            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync("sabeel.softw@gmail.com", "wzsjhtpnmvocmrfs");
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
           
        }
    }
}
