namespace FromQueueMailSender
{
    public class SettingsFile
    {
        public RabbitConnection? RabbitConnection { get; init; }
        public RabbitConfiguration? RabbitConfiguration { get; init; }
        public MailConnection? MailConnection { get; set; }
    }

    public class RabbitConnection
    {
        public string? HostName { get; init; }
        public int Port { get; init; }
        public string? UserName { get; init; }
        public string? Password { get; init; }
    }

    public class RabbitConfiguration
    {
        public string? ExchangeName { get; init; }
        public string? ReminderPasswordQueue { get; init; }
        public string? ReminderPasswordRoutingKey { get; init; }
        public string? ActivateAccountQueue { get; init; }
        public string? ActivateAccountRoutingKey { get; init; }
    }

    public class MailConnection
    {
        public string? Host { get; set; }
        public int Port { get; set; }
        public bool SSL { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}