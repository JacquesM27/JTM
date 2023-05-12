// See https://aka.ms/new-console-template for more information
using FromQueueMailSender;
using FromQueueMailSender.Services.Mail;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.WriteLine("Hello, World!");

var host = new HostBuilder()
    .ConfigureServices(services =>
    {
        services.AddSingleton<IMailService, MailService>();
        services.AddHostedService<MessageReaderService>();
    })
    .UseConsoleLifetime()
    .Build();

host.Run();