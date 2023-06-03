using FluentValidation;
using FluentValidation.Results;
using JTM.Data;
using JTM.DTO.Account.RegisterUser;
using JTM.Helper.PasswordHelper;
using JTM.Services.RabbitService;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace JTM.CQRS.Command.Account
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand>
    {
        private readonly DataContext _dataContext;
        private readonly IRabbitService _rabbitService;

        public RegisterUserCommandHandler(DataContext dataContext, IRabbitService rabbitService)
        {
            _dataContext = dataContext;
            _rabbitService = rabbitService;
        }

        public async Task Handle(RegisterUserCommand request, CancellationToken cancellationToken)
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
                PasswordResetToken = null
            };

            await _dataContext.AddAsync(newUser, cancellationToken);
            await _dataContext.SaveChangesAsync(cancellationToken);
            SendActivationMessage(newUser);
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

        private async Task CheckEmailUniqueness(string email)
        {
            if (await _dataContext.Users.AnyAsync(c => c.Email == email))
            {
                throw new ValidationException("Email address is busy", 
                    new List<ValidationFailure> 
                    {
                        new ValidationFailure() 
                        {
                            PropertyName = "Email", ErrorMessage = "Email address is busy" 
                        } 
                    });
            }
        }
    }
}
