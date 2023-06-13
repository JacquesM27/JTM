using JTM.Data.UnitOfWork;
using JTM.Exceptions;
using MediatR;

namespace JTM.CQRS.Command.WorkingTime
{
    public sealed class UpdateWorkingTimeCommandHandler : IRequestHandler<UpdateWorkingTimeCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateWorkingTimeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateWorkingTimeCommand request, CancellationToken cancellationToken)
        {
            if (request.HeaderId != request.RouteId)
                throw new WorkingTimeException($"Id from header({request.HeaderId}) does not equal id from route({request.RouteId}).");

            var timeToUpdate = await _unitOfWork.WorkingTimeRepository.GetByIdAsync(request.HeaderId)
                ?? throw new NotFoundException($"Working time with id: {request.HeaderId} does not exist.");

            await ValidUser(request.EmployeeId);
            await ValidUser(request.EditorId);
            await ValidCompany(request.CompanyId);

            timeToUpdate.SecondsOfWork = request.SecondsOfWork;
            timeToUpdate.CompanyId = request.CompanyId;
            timeToUpdate.EmployeeId = request.EmployeeId;
            timeToUpdate.LastEditorId = request.EditorId;
            timeToUpdate.LastModified = DateTime.Now;
            timeToUpdate.WorkingDate = request.WorkingDate;

            await _unitOfWork.WorkingTimeRepository.UpdateAsync(request.HeaderId, timeToUpdate);
            await _unitOfWork.SaveChangesAsync();
        }

        private async Task ValidUser(int userId)
        {
            if (!await _unitOfWork.UserRepository.AnyAsync(userId))
                throw new NotFoundException($"User with id:{userId} does not exist.");
        }

        private async Task ValidCompany(int? companyId)
        {
            if (companyId is not null)
                if (!await _unitOfWork.CompanyRepository.AnyAsync((int)companyId))
                    throw new NotFoundException($"Company with id:{companyId} does not exist.");
        }
    }
}
