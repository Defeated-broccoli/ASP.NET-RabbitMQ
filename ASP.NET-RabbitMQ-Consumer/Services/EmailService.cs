using System.Net.Mail;
using System.Net;
using ASP.NET_RabbitMQ_Consumer.Models;
using ASP.NET_RabbitMQ_Consumer.Interfaces;

namespace ASP.NET_RabbitMQ_Consumer.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(Mail mail)
        {
            //link to outlook server with dummy accout
            var senderMail = "devtesting84@outlook.com";
            var senderPassword = "Testing1234!";

            var client = new SmtpClient("smtp-mail.outlook.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(senderMail, senderPassword)
            };

            //send mail
            await client.SendMailAsync(new MailMessage(senderMail, mail.Receiver, mail.Subject, mail.Body));
        }
    }
}
