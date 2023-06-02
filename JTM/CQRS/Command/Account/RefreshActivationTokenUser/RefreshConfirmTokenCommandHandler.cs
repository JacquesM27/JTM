using JTM.Data;
using JTM.DTO.Account;
using JTM.DTO.Account.RegisterUser;
using JTM.Services.RabbitService;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace JTM.CQRS.Command.Account.RefreshActivationTokenUser
{
    public class RefreshConfirmTokenCommandHandler : IRequestHandler<RefreshConfirmTokenCommand, AuthResponseDto>
    {
        private readonly DataContext _dataContext;
        private readonly IRabbitService _rabbitService;

        public RefreshConfirmTokenCommandHandler(DataContext dataContext, IRabbitService rabbitService)
        {
            _dataContext = dataContext;
            _rabbitService = rabbitService;
        }

        public async Task<AuthResponseDto> Handle(RefreshConfirmTokenCommand request, CancellationToken cancellationToken)
        {
            var user = await _dataContext.Users
                .SingleOrDefaultAsync(c => c.Email.Equals(request.Email), cancellationToken: cancellationToken);
            if (user is null)
            {
                return new AuthResponseDto { Message = "Invalid user." };
            }
            if (user.EmailConfirmed)
            {
                return new AuthResponseDto { Message = "User already confirmed." };
            }
            user.ActivationToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            user.ActivationTokenExpires = DateTime.UtcNow.AddDays(1);

            await _dataContext.SaveChangesAsync(cancellationToken);

            SendActivationMessage(user);

            return new AuthResponseDto { Success = true, };
        }

        private void SendActivationMessage(User user)
        {
            MessageDto message = new()
            {
                ReceiverEmail = user.Email,
                ReceiverName = user.Username,
                Url = $"https://localhost:7131/api/account/confirm/{user.Id}/{user.ActivationToken}"
            };

            _rabbitService.SendMessage(Enum.MessageQueueType.AccountActivate, message);
        }
    }
}
