using RabbitMQ.Client;

namespace FromQueueMailSender
{
    internal static class RabbitConfigureHelper
    {
        internal static void DeclareExchange(this IModel channel, string exchangeName)
        {
            channel.ExchangeDeclare(
                exchange: exchangeName,
                type: ExchangeType.Direct,
                durable: true,
                autoDelete: false,
                arguments: null);
        }

        internal static void DeclareQueue(this IModel channel, string queue)
        {
            channel.QueueDeclare(
                queue: queue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
        }

        internal static void BindQueueToExchange(
            this IModel channel,
            string exchangeName,
            string queue,
            string routingKey)
        {
            channel.QueueBind(
                queue: queue,
                exchange: exchangeName,
                routingKey: routingKey,
                arguments: null);
        }
    }
}