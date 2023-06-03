using JTM.Data;
using JTM.DTO.Account.RegisterUser;
using JTM.Services.RabbitService;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using JTM.Exceptions;

namespace JTM.CQRS.Command.Account
{
    public sealed class RefreshConfirmTokenCommandHandler : IRequestHandler<RefreshConfirmTokenCommand>
    {
        private readonly DataContext _dataContext;
        private readonly IRabbitService _rabbitService;

        public RefreshConfirmTokenCommandHandler(DataContext dataContext, IRabbitService rabbitService)
        {
            _dataContext = dataContext;
            _rabbitService = rabbitService;
        }

        public async Task Handle(RefreshConfirmTokenCommand request, CancellationToken cancellationToken)
        {
            var user = await _dataContext.Users
                .SingleOrDefaultAsync(c => c.Email.Equals(request.Email), cancellationToken: cancellationToken);

            if (user is null)
                throw new AuthException("Invalid user.");
            else if (user.EmailConfirmed)
                throw new AuthException("User already confirmed.");

            user.ActivationToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            user.ActivationTokenExpires = DateTime.UtcNow.AddDays(1);
            await _dataContext.SaveChangesAsync(cancellationToken);

            SendActivationMessage(user);
        }

        private void SendActivationMessage(User user)
        {
            MessageDto message = new(
                receiverEmail: user.Email,
                receiverName: user.Username,
                url: $"https://localhost:7131/api/account/confirm/{user.Id}/{user.ActivationToken}");

            _rabbitService.SendMessage(Enum.MessageQueueType.AccountActivate, message);
        }
    }
}
