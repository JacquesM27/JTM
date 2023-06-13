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
    public sealed class RefreshConfirmTokenCommandHandler : IRequestHandler<RefreshConfirmTokenCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRabbitService _rabbitService;

        public RefreshConfirmTokenCommandHandler(IUnitOfWork unitOfWork, IRabbitService rabbitService)
        {
            _unitOfWork = unitOfWork;
            _rabbitService = rabbitService;
        }

        public async Task Handle(RefreshConfirmTokenCommand request, CancellationToken cancellationToken)
        {
            Expression<Func<User, bool>> filter = user => user.Email == request.Email;
            var user = await _unitOfWork.UserRepository.QuerySingleAsync(filter);

            if (user is null)
                throw new AuthException("Invalid user.");
            else if (user.EmailConfirmed)
                throw new AuthException("User already confirmed.");

            user.ActivationToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            user.ActivationTokenExpires = DateTime.UtcNow.AddDays(1);
            await _unitOfWork.SaveChangesAsync();

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
