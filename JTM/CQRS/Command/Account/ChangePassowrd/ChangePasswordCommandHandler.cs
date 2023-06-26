using JTM.Data.UnitOfWork;
using JTM.Exceptions;
using JTM.Helper.PasswordHelper;
using MediatR;

namespace JTM.CQRS.Command.Account
{
    public sealed class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ChangePasswordCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId);

            if (user is null)
                throw new AuthException("Invalid user.");
            else if (user.PasswordTokenExpires < DateTime.UtcNow)
                throw new AuthException("Token expires.");
            else if (string.IsNullOrEmpty(request.Token))
                throw new AuthException("Invalid token.");
            else if (!request.Token.Equals(user.PasswordResetToken))
                throw new AuthException("Invalid token.");

            PasswordHelper.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.PasswordResetToken = null;

            await _unitOfWork.UserRepository.UpdateAsync(user.Id, user);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
