using JTM.Data.UnitOfWork;
using JTM.Exceptions;
using MediatR;
using System.Data.SqlTypes;

namespace JTM.CQRS.Command.Account
{
    public sealed class BanUserCommandHandler : IRequestHandler<BanUserCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public BanUserCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(BanUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId)
                 ?? throw new AuthException("Invalid user.");

            user.Banned = true;
            user.TokenExpires = (DateTime)SqlDateTime.MinValue;
            await _unitOfWork.UserRepository.UpdateAsync(user.Id, user);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
