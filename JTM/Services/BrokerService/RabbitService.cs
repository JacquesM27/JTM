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
            IBrokerSender sender;
            switch (messageQueueType)
            {
                case MessageQueueType.PasswordRemind:
                    sender = new RemindPasswordRabbitSender(_configuration);
                    break;
                case MessageQueueType.AccountActivate:
                    sender = new ConfirmAccountRabbitSender(_configuration);
                    break;
                default:
                    throw new ArgumentException($"MessageQueueType: {messageQueueType} is not implemented!");
            }
            sender.PublishMessage(JsonSerializer.Serialize(message));
        }
    }
}
