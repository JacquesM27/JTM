using JTM.Data;
using JTM.Exceptions;
using JTM.Helper.PasswordHelper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JTM.CQRS.Command.Account
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand>
    {
        private readonly DataContext _dataContext;

        public ChangePasswordCommandHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _dataContext.Users
                .SingleOrDefaultAsync(c => c.Id == request.UserId, cancellationToken: cancellationToken);

            if (user is null)
                throw new AuthException("Invalid user.");
            else if (user.PasswordTokenExpires < DateTime.UtcNow)
                throw new AuthException("Token expires.");
            else if (!request.Token.Equals(user.PasswordResetToken))
                throw new AuthException("Invalid token.");

            PasswordHelper.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.PasswordResetToken = null;

            await _dataContext.SaveChangesAsync(cancellationToken);
        }
    }
}
