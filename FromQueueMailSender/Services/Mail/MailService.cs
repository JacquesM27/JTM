using MimeKit;
using MailKit.Net.Smtp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using FromQueueMailSender.Services.Mail.Formats;

namespace FromQueueMailSender.Services.Mail
{
    internal class MailService : IMailService
    {
        public async Task SendActivationEmailAsync(string receiverName, string receiverEmail,  string url)
        {
            string mailContent = ActivateAccountContent.GetContent(receiverName, url);
            await SendEmailAsync(receiverName, receiverEmail, "JTM - Aktywacja konta", mailContent);
        }

        public async Task SendPasswordReminderEmailAsync(string receiverName, string receiverEmail,  string url)
        {
            string mailContent = ResetPasswordContent.GetContent(receiverName, url);
            await SendEmailAsync(receiverName, receiverEmail, "JTM - Resetowanie hasła", mailContent);
        }

        private async Task SendEmailAsync(string receiverName, string receiverEmail, string subject, string htmlBody)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("JTM NoReply", "jm.jtm@outlook.com"));
            message.To.Add(new MailboxAddress(receiverName, receiverEmail));
            message.Subject = subject;
            message.Body = new TextPart("html")
            {
                Text = htmlBody
            };

            using var client = new SmtpClient();
            await client.ConnectAsync("smtp-mail.outlook.com", 587, false);
            await client.AuthenticateAsync("jm.jtm@outlook.com", "HasloMail123");
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
