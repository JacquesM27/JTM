using JTM.Data.Model;
using JTM.Data.UnitOfWork;
using JTM.DTO.Account.RegisterUser;
using JTM.Exceptions;
using JTM.Services.RabbitService;
using MediatR;
using System.Linq.Expressions;
using System.Security.Cryptography;

namespace JTM.CQRS.Command.Account
{
    public sealed class ForgetPasswordUserCommandHandler : IRequestHandler<ForgetPasswordCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRabbitService _rabbitService;

        public ForgetPasswordUserCommandHandler(IUnitOfWork unitOfWork, IRabbitService rabbitService)
        {
            _unitOfWork = unitOfWork;
            _rabbitService = rabbitService;
        }

        public async Task Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
        {
            Expression<Func<User, bool>> filter = user => user.Email == request.Email;
            var user = await _unitOfWork.UserRepository.QuerySingleAsync(filter)
                ?? throw new AuthException("Invalid user.");

            user.PasswordTokenExpires = DateTime.UtcNow.AddDays(1);
            user.PasswordResetToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            await _unitOfWork.SaveChangesAsync();

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
