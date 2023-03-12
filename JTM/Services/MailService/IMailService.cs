namespace JTM.Services.MailService
{
    public interface IMailService
    {
        Task SendConfirmationEmail(User user);
        Task SendPasswordResetEmail(User user);
    }
}
