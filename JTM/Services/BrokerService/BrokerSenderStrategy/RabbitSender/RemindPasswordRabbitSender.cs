using RabbitMQ.Client;
using System.Text;

namespace JTM.Services.BrokerService.BrokerSenderStrategy.RabbitSender
{
    public sealed class RemindPasswordRabbitSender : RabbitSenderBase, IBrokerSender
    {
        protected readonly string _reminderPasswordRoutingKey;

        public RemindPasswordRabbitSender(IConfiguration configuration) : base(configuration)
        {
            _reminderPasswordRoutingKey = Configuration.GetSection("RabbitConfiguration:ReminderPasswordRoutingKey").Value!;
        }

        public void PublishMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            Channel.BasicPublish(
                exchange: ExchangeName,
                routingKey: _reminderPasswordRoutingKey,
                basicProperties: null,
                body: body);
        }
    }
}
