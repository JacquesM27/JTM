using JTM.Data;
using JTM.DTO.Account;
using JTM.Helper.PasswordHelper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JTM.CQRS.Command.Account.ChangePassowrdUser
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, AuthResponseDto>
    {
        private readonly DataContext _dataContext;

        public ChangePasswordCommandHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<AuthResponseDto> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _dataContext.Users
                .SingleOrDefaultAsync(c => c.Id == request.UserId, cancellationToken: cancellationToken);

            if (user is null)
            {
                return new AuthResponseDto { Message = "Invalid user." };
            }
            else if (user.PasswordTokenExpires < DateTime.UtcNow)
            {
                return new AuthResponseDto { Message = "Token expires." };
            }
            else if (!request.Token.Equals(user.PasswordResetToken))
            {
                return new AuthResponseDto { Message = "Invalid token." };
            }

            PasswordHelper.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.PasswordResetToken = null;

            await _dataContext.SaveChangesAsync(cancellationToken);
            return new AuthResponseDto { Success = true, };
        }
    }
}
