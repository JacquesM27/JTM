using FluentValidation;
using FluentValidation.Results;
using JTM.Data.Model;
using JTM.Data.UnitOfWork;
using JTM.DTO.Account.RegisterUser;
using JTM.Helper.PasswordHelper;
using JTM.Services.RabbitService;
using MediatR;
using System.Linq.Expressions;
using System.Security.Cryptography;

namespace JTM.CQRS.Command.Account
{
    public sealed class RegisterCommandHandler : IRequestHandler<RegisterCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBrokerService _rabbitService;

        public RegisterCommandHandler(IUnitOfWork unitOfWork, IBrokerService rabbitService)
        {   
            _unitOfWork = unitOfWork;
            _rabbitService = rabbitService;
        }

        public async Task Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            await CheckEmailUniqueness(request.Email);

            PasswordHelper.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            User newUser = new()
            {
                Username = request.UserName,
                Email = request.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                ActivationToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                ActivationTokenExpires = DateTime.UtcNow.AddDays(1),
                PasswordResetToken = null,
                Role = request.UserRole.ToString()
            };

            await _unitOfWork.UserRepository.AddAsync(newUser);
            await _unitOfWork.SaveChangesAsync();
            SendActivationMessage(newUser);
        }

        private void SendActivationMessage(User user)
        {
            MessageDto message = new(
                receiverEmail: user.Email, 
                receiverName: user.Username, 
                url: $"https://localhost:7131/api/account/confirm/{user.Id}/{user.ActivationToken}");

            _rabbitService.SendMessage(Enum.MessageQueueType.AccountActivate, message);
        }

        private async Task CheckEmailUniqueness(string email)
        {
            Expression<Func<User, bool>> filter = user => user.Email == email;
            var user = await _unitOfWork.UserRepository.QuerySingleAsync(filter);
            if (user is not null)
            {
                throw new ValidationException("Email address is busy.", 
                    new List<ValidationFailure> 
                    {
                        new ValidationFailure() 
                        {
                            PropertyName = "Email", ErrorMessage = "Email address is busy." 
                        } 
                    });
            }
        }
    }
}
