using RabbitMQ.Client;

namespace JTM.Services.BrokerService.BrokerSenderStrategy.RabbitSender
{
    public abstract class RabbitSenderBase
    {
        protected readonly IConfiguration Configuration;
        protected readonly IModel Channel;
        protected readonly string ExchangeName;

        public RabbitSenderBase(IConfiguration configuration)
        {
            Configuration = configuration;
            ConnectionFactory connectionFactory = new()
            {
                HostName = Configuration.GetSection("RabbitConnection:HostName").Value!,
                Port = Convert.ToInt32(Configuration.GetSection("RabbitConnection:Port").Value!),
                UserName = Configuration.GetSection("RabbitConnection:UserName").Value!,
                Password = Configuration.GetSection("RabbitConnection:Password").Value!,
            };
            var connection = connectionFactory.CreateConnection();
            Channel = connection.CreateModel();
            ExchangeName = Configuration.GetSection("RabbitConfiguration:ExchangeName").Value!;
        }
    }
}
