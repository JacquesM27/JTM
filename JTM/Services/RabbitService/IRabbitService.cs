using JTM.DTO.Account.RegisterUser;
using JTM.Enum;

namespace JTM.Services.RabbitService
{
    public interface IRabbitService
    {
        void SendMessage(MessageQueueType messageQueueType, MessageDto message);
    }
}
