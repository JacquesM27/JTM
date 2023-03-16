using MimeKit;
using MailKit.Net.Smtp;
using JTM.Data;
using Microsoft.EntityFrameworkCore;

namespace JTM.Services.MailService
{
    public class MailService : IMailService
    {
        private readonly DataContext _dataContext;

        public MailService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task SendConfirmationEmail(string email)
        {
            var user = await _dataContext.Users.AsNoTracking()
                .SingleOrDefaultAsync(c => c.Email.Equals(email));
            string subject = "JTM - Confirm Email";
            string body = $"<h1>Confirm token</h1><p>{user.ActivationToken}</p>";
            await SendEmail(user, subject, body);
        }

        public async Task SendPasswordResetEmail(string email)
        {
            var user = await _dataContext.Users.AsNoTracking()
                .SingleOrDefaultAsync(c => c.Email.Equals(email));
            string subject = "JTM - Reset password";
            string body = $"<h1>Password Token</h1><p>{user.PasswordResetToken}</p>";
            await SendEmail(user, subject, body);
        }

        private async Task SendEmail(User user, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("JTM NoReply", "jm.jtm@outlook.com"));
            message.To.Add(new MailboxAddress(user.Username, user.Email));
            message.Subject = subject;
            message.Body = new TextPart("html")
            {
                Text = body
            };

            using var client = new SmtpClient();
            await client.ConnectAsync("smtp-mail.outlook.com", 587, false);
            await client.AuthenticateAsync("jm.jtm@outlook.com", "HasloMail123");
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
