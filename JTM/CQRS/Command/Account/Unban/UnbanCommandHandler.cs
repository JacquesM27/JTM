using JTM.Data.UnitOfWork;
using JTM.Exceptions;
using MediatR;

namespace JTM.CQRS.Command.Account
{
    public class UnbanCommandHandler : IRequestHandler<UnbanCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UnbanCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(UnbanCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId)
                ?? throw new AuthException("Invalid user.");

            user.Banned = false;
            await _unitOfWork.UserRepository.UpdateAsync(user.Id, user);
            await _unitOfWork.SaveChangesAsync();

            return user.Id;
        }
    }
}
