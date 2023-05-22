using FromQueueMailSender.DTO;
using FromQueueMailSender.Services.Mail;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace FromQueueMailSender.Services.Message
{
    internal class ActiveAccountService : AbstractMessageReaderService
    {
        private readonly string _activateAccountQueue;

        public ActiveAccountService(IMailService mailService)
            : base(mailService)
        {
            _activateAccountQueue = "jtmAccountQueue";
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
            _channel.BasicConsume(_activateAccountQueue, true, consumer);
            return Task.CompletedTask;
        }

        protected override async Task HandleMessage(MessageDto messageDto)
        {
             await _mailService.SendActivationEmailAsync(messageDto.ReceiverName, messageDto.ReceiverEmail, messageDto.Url);
        }
    }
}
