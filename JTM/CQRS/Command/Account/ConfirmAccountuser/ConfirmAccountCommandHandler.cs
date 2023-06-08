using JTM.Data.UnitOfWork;
using JTM.Exceptions;
using MediatR;

namespace JTM.CQRS.Command.Account
{
    public sealed class ConfirmAccountCommandHandler : IRequestHandler<ConfirmAccountCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ConfirmAccountCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(ConfirmAccountCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId);

            if (user is null)
                throw new AuthException("Invalid user.");
            else if (user.EmailConfirmed)
                throw new AuthException("User already confirmed.");
            else if (user.ActivationTokenExpires < DateTime.UtcNow)
                throw new AuthException("Token expired.");
            else if (!request.Token!.Equals(user.ActivationToken))
                throw new AuthException("Invalid token.");

            user.EmailConfirmed = true;
            user.ActivationToken = null;
            await _unitOfWork.UserRepository.UpdateAsync(user.Id, user);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
