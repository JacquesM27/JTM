using JTM.Data.UnitOfWork;
using JTM.Exceptions;
using MediatR;

namespace JTM.CQRS.Command.WorkingTime
{
    public sealed class DeleteTimeCommandHandler : IRequestHandler<DeleteTimeCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTimeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteTimeCommand request, CancellationToken cancellationToken)
        {
            await ValidUser(request.DeletorId);

            var workingTime = await _unitOfWork.WorkingTimeRepository.GetByIdAsync(request.WorkingTimeId) 
                ?? throw new WorkingTimeException($"Working time with id:{request.WorkingTimeId} does not exist.");

            workingTime.Deleted = true;
            workingTime.LastEditorId = request.DeletorId;
            workingTime.LastModified = DateTime.UtcNow;

            await _unitOfWork.WorkingTimeRepository.UpdateAsync(request.WorkingTimeId, workingTime);
            await _unitOfWork.SaveChangesAsync();
        }

        private async Task ValidUser(int userId)
        {
            if (!await _unitOfWork.UserRepository.AnyAsync(userId))
                throw new WorkingTimeException($"User with id:{userId} does not exist.");
        }
    }
}
