using FromQueueMailSender.DTO;
using FromQueueMailSender.Services.Mail;
using FromQueueMailSender.Services.ProgramConfiguration;

namespace FromQueueMailSender.Services.Message
{
    internal class PasswordRemindService : AbstractMessageReaderService
    {
        public PasswordRemindService(IMailService mailService, IProgramConfiguration configuration) 
            : base(mailService, configuration) { }

        protected override async Task HandleMessage(MessageDto messageDto)
            =>  await _mailService.SendPasswordReminderEmailAsync(messageDto.ReceiverName, messageDto.ReceiverEmail, messageDto.Url);

        protected override void SetQueueName()
            =>   _queueName = _settings.RabbitConfiguration!.ReminderPasswordQueue!;
    }
}