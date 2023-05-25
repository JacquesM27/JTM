using FromQueueMailSender.DTO;
using FromQueueMailSender.Services.Mail;
using Microsoft.Extensions.Configuration;

namespace FromQueueMailSender.Services.Message
{
    internal class ActiveAccountService : AbstractMessageReaderService
    {
        public ActiveAccountService(IMailService mailService, IConfiguration configuration)
            : base(mailService, configuration) { }

        protected override async Task HandleMessage(MessageDto messageDto)
            =>  await _mailService.SendActivationEmailAsync(messageDto.ReceiverName, messageDto.ReceiverEmail, messageDto.Url);

        protected override void SetQueueName()
            =>  _queueName = _settings.RabbitConfiguration!.ActivateAccountQueue!;
    }
}