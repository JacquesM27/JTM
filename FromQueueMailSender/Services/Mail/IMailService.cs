namespace FromQueueMailSender.Services.Mail
{
    internal interface IMailService
    {
        Task SendActivationEmailAsync(string receiverName, string receiverEmail, string url);
        Task SendPasswordReminderEmailAsync(string receiverName, string receiverEmail, string url);
    }
}
