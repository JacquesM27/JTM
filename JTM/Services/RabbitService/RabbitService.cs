using JTM.DTO.Account.RegisterUser;
using JTM.Enum;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace JTM.Services.RabbitService
{
    public class RabbitService : IRabbitService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        private readonly string _exchangeName = "jtmMailExchange";
        private readonly string _reminderPasswordRoutingKey = "jtm.email.password.remind";
        private readonly string _activateAccountRoutingKey = "jtm.email.account.active";

        public RabbitService()
        {
            ConnectionFactory connectionFactory = new()
            {
                HostName = "localhost",
                Port = 8180,
                UserName = "guest",
                Password = "guest",
            };
            _connection = connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void SendMessage(MessageQueueType messageQueueType, MessageDto message)
        {
            switch (messageQueueType)
            {
                case MessageQueueType.PasswordRemind:
                    PublishMessage(_exchangeName, _reminderPasswordRoutingKey, JsonSerializer.Serialize(message));
                    break;
                case MessageQueueType.AccountActivate:
                    PublishMessage(_exchangeName, _activateAccountRoutingKey, JsonSerializer.Serialize(message));
                    break;
                default:
                    break;
            }
        }

        private void PublishMessage(string exchangeName, string routingKey, string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(
                exchange: exchangeName,
                routingKey: routingKey,
                basicProperties: null,
                body: body);
        }
    }
}
