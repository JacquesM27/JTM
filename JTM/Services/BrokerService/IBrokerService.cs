using JTM.DTO.Account.RegisterUser;
using JTM.Enum;

namespace JTM.Services.RabbitService
{
    public interface IBrokerService
    {
        void SendMessage(MessageQueueType messageQueueType, MessageDto message);
    }
}
