using FromQueueMailSender.DTO;
using FromQueueMailSender.Services.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

namespace FromQueueMailSender.Services.Message
{
    internal abstract class AbstractMessageReaderService : BackgroundService
    {
        private readonly ConnectionFactory? _connectionFactory;
        private readonly IConnection? _connection;
        protected readonly IModel? _channel;
        protected readonly IMailService _mailService;

        public AbstractMessageReaderService(IMailService mailService)
        {
            _mailService = mailService;
            _connectionFactory = new()
            {
                HostName = "localhost",
                Port = 8180,
                UserName = "guest",
                Password = "guest",
            };
            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        protected abstract Task HandleMessage(MessageDto messageDto);
    }
}
