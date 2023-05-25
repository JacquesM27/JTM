using FromQueueMailSender.Services.Mail;
using FromQueueMailSender.Services.Message;
using FromQueueMailSender.Services.ProgramConfiguration;
using FromQueueMailSender.Services.RabbitConfiguration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureAppConfiguration(app =>
    {
        app.AddJsonFile("appsettings.json");
    })
    .ConfigureServices(services =>
    {
        services.AddSingleton<IProgramConfiguration, ProgramConfiguration>();
        services.AddSingleton<IRabbitConfigurationService, RabbitConfigurationService>();
        services.AddSingleton<IMailService, MailService>();
        services.AddHostedService<ActiveAccountService>();
        services.AddHostedService<PasswordRemindService>();
    })
    .UseConsoleLifetime()
    .Build();
host.Services.GetService<IRabbitConfigurationService>()!.ConfigureRabbit();

host.Run();