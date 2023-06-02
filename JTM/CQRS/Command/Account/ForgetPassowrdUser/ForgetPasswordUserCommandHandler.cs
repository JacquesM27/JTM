using JTM.Data;
using JTM.DTO.Account;
using JTM.DTO.Account.RegisterUser;
using JTM.Services.RabbitService;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace JTM.CQRS.Command.Account.ForgetPassowrdUser
{
    public class ForgetPasswordUserCommandHandler : IRequestHandler<ForgetPasswordCommand, AuthResponseDto>
    {
        private readonly DataContext _dataContext;
        private readonly IRabbitService _rabbitService;
        public ForgetPasswordUserCommandHandler(DataContext dataContext, IRabbitService rabbitService)
        {
            _dataContext = dataContext;
            _rabbitService = rabbitService;
        }

        public async Task<AuthResponseDto> Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _dataContext.Users
                .SingleOrDefaultAsync(c => c.Email.Equals(request.Email), cancellationToken);

            if (user is null)
            {
                return new AuthResponseDto { Message = "Invalid user." };
            }

            user.PasswordTokenExpires = DateTime.UtcNow.AddDays(1);
            user.PasswordResetToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            await _dataContext.SaveChangesAsync(cancellationToken);

            SendResetPasswordMessage(user);

            return new AuthResponseDto() { Success = true, };
        }

        private void SendResetPasswordMessage(User user)
        {
            MessageDto message = new()
            {
                ReceiverEmail = user.Email,
                ReceiverName = user.Username,
                Url = $"https://localhost:7131/api/account/confirm/{user.Id}/{user.PasswordResetToken}"
            };

            _rabbitService.SendMessage(Enum.MessageQueueType.PasswordRemind, message);
        }
    }
}
