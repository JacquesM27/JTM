using JTM.Data;
using JTM.Helper.PasswordHelper;
using MediatR;
using System.Security.Cryptography;

namespace JTM.CQRS.Command.Auth.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, int>
    {
        private readonly DataContext _dataContext;
        public RegisterUserCommandHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public Task<int> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
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

            _dataContext.Add(newUser);
            return Task.FromResult(newUser.Id);
        }
    }
}
