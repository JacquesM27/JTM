using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Text;

namespace JTM.Services.BrokerService.BrokerSenderStrategy.RabbitSender
{
    public class ConfirmAccountRabbitSender : RabbitSenderBase, IBrokerSender
    {
        private readonly string _activateAccountRoutingKey;

        public ConfirmAccountRabbitSender(IConfiguration configuration) : base(configuration)
        {
            _activateAccountRoutingKey = Configuration.GetSection("RabbitConfiguration:ActivateAccountRoutingKey").Value!;
        }

        public void PublishMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            Channel.BasicPublish(
                exchange: ExchangeName,
                routingKey: _activateAccountRoutingKey,
                basicProperties: null,
                body: body);
        }
    }
}
