using FHP.infrastructure.Service;
using MailKit.Security;
using MimeKit;

namespace FHP.services
{
    public class EmailService : IEmailService
    {
       
        public async Task SendverificationEmail(string email,int userId, string origin)
        {
            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress("sabeel.softw@gmail.com"));
            message.To.Add(new MailboxAddress(email));
            message.Subject = "Email Verfication";
            string emailBody = $"{origin}" + userId;
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


        public async Task SendContractEmail(string email,string employerEmail, int userId, int employerId,string htmlBody, string subject)
        {

            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress("sabeel.softw@gmail.com"));
            message.To.Add(new MailboxAddress(email));
            message.To.Add(new MailboxAddress(employerEmail));
            message.Subject = subject;
            /* string emailBody = "" + userId;
             BodyBuilder bodyBuilder = new BodyBuilder();
             bodyBuilder.HtmlBody = emailBody;*/
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = htmlBody // Set the HTML body
            };

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
