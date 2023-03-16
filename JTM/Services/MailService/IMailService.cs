namespace JTM.Services.MailService
{
    public interface IMailService
    {
        Task SendConfirmationEmail(string email);
        Task SendPasswordResetEmail(string email);
    }
}
