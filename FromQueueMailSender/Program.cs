// See https://aka.ms/new-console-template for more information
using FromQueueMailSender;
using FromQueueMailSender.Services.Mail;
using FromQueueMailSender.Services.Message;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json");
IConfiguration config = builder.Build();

ConnectionFactory _connectionFactory;
IConnection _connection;
IModel _channel;

_connectionFactory = new()
{
    HostName = "localhost",
    Port = 8180,
    UserName = "guest",
    Password = "guest",
};
_connection = _connectionFactory.CreateConnection();
_channel = _connection.CreateModel();

string _exchangeName = config.GetSection("RabbitConfiguration:ExchangeName").Value;
string _reminderPasswordQueue = config.GetSection("RabbitConfiguration:ReminderPasswordQueue").Value;
string _reminderPasswordRoutingKey = config.GetSection("RabbitConfiguration:ReminderPasswordRoutingKey").Value;
string _activateAccountQueue = config.GetSection("RabbitConfiguration:ActivateAccountQueue").Value;
string _activateAccountRoutingKey = config.GetSection("RabbitConfiguration:ActivateAccountRoutingKey").Value;

_channel.DeclareExchange(_exchangeName);
_channel.DeclareQueue(_reminderPasswordQueue);
_channel.DeclareQueue(_activateAccountQueue);
_channel.BindQueueToExchange(_exchangeName, _reminderPasswordQueue, _reminderPasswordRoutingKey);
_channel.BindQueueToExchange(_exchangeName, _activateAccountQueue, _activateAccountRoutingKey);

var host = new HostBuilder()
    .ConfigureServices(services =>
    {
        services.AddSingleton<IMailService, MailService>();
        services.AddHostedService<ActiveAccountService>();
        services.AddHostedService<PasswordRemindService>();
    })
    .UseConsoleLifetime()
    .Build();

host.Run();