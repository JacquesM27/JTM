using JTM.DTO.Account.RegisterUser;
using JTM.Enum;
using JTM.Services.BrokerService.BrokerSenderStrategy;
using JTM.Services.BrokerService.BrokerSenderStrategy.RabbitSender;
using System.Text.Json;

namespace JTM.Services.RabbitService
{
    public class RabbitService : IBrokerService
    {
        private readonly IConfiguration _configuration;

        public RabbitService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendMessage(MessageQueueType messageQueueType, MessageDto message)
        {
            IBrokerSender sender = messageQueueType switch
            {
                MessageQueueType.PasswordRemind => new RemindPasswordRabbitSender(_configuration),
                MessageQueueType.AccountActivate => new ConfirmAccountRabbitSender(_configuration),
                _ => throw new ArgumentException($"MessageQueueType: {messageQueueType} is not handled!"),
            };
            sender.PublishMessage(JsonSerializer.Serialize(message));
        }
    }
}
