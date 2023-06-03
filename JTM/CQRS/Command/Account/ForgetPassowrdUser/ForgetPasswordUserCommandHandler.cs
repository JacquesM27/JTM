using JTM.Data;
using JTM.DTO.Account.RegisterUser;
using JTM.Exceptions;
using JTM.Services.RabbitService;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace JTM.CQRS.Command.Account
{
    public class ForgetPasswordUserCommandHandler : IRequestHandler<ForgetPasswordCommand>
    {
        private readonly DataContext _dataContext;
        private readonly IRabbitService _rabbitService;

        public ForgetPasswordUserCommandHandler(DataContext dataContext, IRabbitService rabbitService)
        {
            _dataContext = dataContext;
            _rabbitService = rabbitService;
        }

        public async Task Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _dataContext.Users
                .SingleOrDefaultAsync(c => c.Email.Equals(request.Email), cancellationToken)
                ?? throw new AuthException("Invalid user.");

            user.PasswordTokenExpires = DateTime.UtcNow.AddDays(1);
            user.PasswordResetToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            await _dataContext.SaveChangesAsync(cancellationToken);

            SendResetPasswordMessage(user);
        }

        private void SendResetPasswordMessage(User user)
        {
            MessageDto message = new(
                receiverEmail: user.Email,
                receiverName: user.Username,
                url: $"https://localhost:7131/api/account/confirm/{user.Id}/{user.PasswordResetToken}");

            _rabbitService.SendMessage(Enum.MessageQueueType.PasswordRemind, message);
        }
    }
}
