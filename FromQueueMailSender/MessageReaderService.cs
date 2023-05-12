using FromQueueMailSender.DTO;
using FromQueueMailSender.Services.Mail;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FromQueueMailSender
{
    internal class MessageReaderService : BackgroundService
    {
        private readonly ConnectionFactory _connectionFactory;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IMailService _mailService;
        private string _queueName = "JTM.Mail";

        public MessageReaderService(IMailService mailService)
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
            DeclareQueue();
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                string content = Encoding.UTF8.GetString(ea.Body.Span);
                MailDetailsDTO? deserializedContent = JsonSerializer.Deserialize<MailDetailsDTO>(content) ?? throw new ArgumentNullException(content);
                _mailService.SendEmailAsync(deserializedContent.ReceiverName,
                                            deserializedContent.ReceiverEmail,
                                            deserializedContent.Subject,
                                            deserializedContent.HtmlBody);
            };
            _channel.BasicConsume(_queueName, true, consumer);
            return Task.CompletedTask;
        }

        private void DeclareQueue()
        {
            _channel.QueueDeclare(
                queue: _queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
        }
    }
}
