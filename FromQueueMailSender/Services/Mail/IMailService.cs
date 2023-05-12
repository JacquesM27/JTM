using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FromQueueMailSender.Services.Mail
{
    internal interface IMailService
    {
        Task SendEmailAsync(string receiverName, string receiverEmail, string subject, string htmlBody);
    }
}
