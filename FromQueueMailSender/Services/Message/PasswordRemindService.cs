using FromQueueMailSender.DTO;
using FromQueueMailSender.Services.Mail;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace FromQueueMailSender.Services.Message
{
    internal class PasswordRemindService : AbstractMessageReaderService
    {
        private readonly string _reminderPasswordQueue;

        public PasswordRemindService(IMailService mailService) 
            : base(mailService) 
        {
            _reminderPasswordQueue = "jtmPasswordQueue";
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
            _channel.BasicConsume(_reminderPasswordQueue, true, consumer);
            return Task.CompletedTask;
        }

        protected override async Task HandleMessage(MessageDto messageDto)
        {
            await _mailService.SendPasswordReminderEmailAsync(messageDto.ReceiverName, messageDto.ReceiverEmail, messageDto.Url);
        }
    }
}
