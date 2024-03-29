﻿using FromQueueMailSender.Services.Mail.Formats;
using FromQueueMailSender.Services.ProgramConfiguration;
using MailKit.Net.Smtp;
using MimeKit;

namespace FromQueueMailSender.Services.Mail
{
    internal class MailService : IMailService
    {
        private readonly MailConnection _configuration;

        public MailService(IProgramConfiguration configuration)
        {
            _configuration = configuration.Connfiguration.MailConnection!;
        }
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
            message.From.Add(new MailboxAddress(_configuration.Username, _configuration.Email));
            message.To.Add(new MailboxAddress(receiverName, receiverEmail));
            message.Subject = subject;
            message.Body = new TextPart("html")
            {
                Text = htmlBody
            };

            using var client = new SmtpClient();
            await client.ConnectAsync(_configuration.Host, _configuration.Port,_configuration.SSL);
            await client.AuthenticateAsync(_configuration.Email, _configuration.Password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}