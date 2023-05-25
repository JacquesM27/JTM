using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace FromQueueMailSender.Services.RabbitConfiguration
{
    internal class RabbitConfigurationService : IRabbitConfigurationService
    {
        private readonly SettingsFile _settings;
        private readonly ConnectionFactory _connectionFactory;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitConfigurationService(IConfiguration configuration)
        {
            _settings = new();
            configuration.Bind(_settings);

            _connectionFactory = new()
            {
                HostName = _settings.RabbitConnection!.HostName,
                Port = _settings.RabbitConnection!.Port,
                UserName = _settings.RabbitConnection!.UserName,
                Password = _settings.RabbitConnection!.Password,
            };
            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void ConfigureRabbit()
        {
            _channel.DeclareExchange(_settings.RabbitConfiguration!.ExchangeName!);
            _channel.DeclareQueue(_settings.RabbitConfiguration!.ReminderPasswordQueue!);
            _channel.DeclareQueue(_settings.RabbitConfiguration!.ActivateAccountQueue!);
            _channel.BindQueueToExchange(
                _settings.RabbitConfiguration!.ExchangeName!,
                _settings.RabbitConfiguration!.ReminderPasswordQueue!,
                _settings.RabbitConfiguration!.ReminderPasswordRoutingKey!);
            _channel.BindQueueToExchange(
                _settings.RabbitConfiguration!.ExchangeName!,
                _settings.RabbitConfiguration!.ActivateAccountQueue!,
                _settings.RabbitConfiguration!.ActivateAccountRoutingKey!);
        }
    }
}