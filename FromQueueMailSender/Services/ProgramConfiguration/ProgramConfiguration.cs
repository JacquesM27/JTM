using Microsoft.Extensions.Configuration;

namespace FromQueueMailSender.Services.ProgramConfiguration
{
    internal class ProgramConfiguration : IProgramConfiguration
    {
        public SettingsFile Connfiguration { get; }

        public ProgramConfiguration(IConfiguration configuration)
        {
            Connfiguration = new();
            configuration.Bind(Connfiguration);
        }
    }
}
