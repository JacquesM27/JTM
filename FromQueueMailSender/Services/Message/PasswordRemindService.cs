using FromQueueMailSender.DTO;
using FromQueueMailSender.Services.Mail;
using Microsoft.Extensions.Configuration;

namespace FromQueueMailSender.Services.Message
{
    internal class PasswordRemindService : AbstractMessageReaderService
    {
        public PasswordRemindService(IMailService mailService, IConfiguration configuration) 
            : base(mailService, configuration) { }

        protected override async Task HandleMessage(MessageDto messageDto)
            =>  await _mailService.SendPasswordReminderEmailAsync(messageDto.ReceiverName, messageDto.ReceiverEmail, messageDto.Url);

        protected override void SetQueueName()
            =>   _queueName = _settings.RabbitConfiguration!.ReminderPasswordQueue!;
    }
}