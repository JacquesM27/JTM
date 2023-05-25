using FromQueueMailSender.DTO;
using FromQueueMailSender.Services.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace FromQueueMailSender.Services.Message
{
    internal abstract class AbstractMessageReaderService : BackgroundService
    {
        private readonly IConnection? _connection;
        private readonly ConnectionFactory? _connectionFactory;
        private readonly IModel? _channel;
        protected readonly IMailService _mailService;
        protected readonly SettingsFile _settings;
        protected string _queueName;

        public AbstractMessageReaderService(IMailService mailService, IConfiguration configuration)
        {
            _mailService = mailService;
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
            SetQueueName();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (ch, ea) =>
            {
                string content = Encoding.UTF8.GetString(ea.Body.Span);
                MessageDto messageDto = JsonSerializer.Deserialize<MessageDto?>(content) ?? throw new ArgumentNullException(content);
                await HandleMessage(messageDto);
            };
            _channel.BasicConsume(_queueName, true, consumer);
            return Task.CompletedTask;
        }

        protected abstract Task HandleMessage(MessageDto messageDto);

        protected abstract void SetQueueName();
    }
}