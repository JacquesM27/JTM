using JTM.Data.UnitOfWork;
using JTM.Exceptions;
using MediatR;

namespace JTM.CQRS.Command.WorkingTime
{
    public sealed class AddWorkingTimeCommandHandler : IRequestHandler<AddWorkingTimeCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddWorkingTimeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(AddWorkingTimeCommand request, CancellationToken cancellationToken)
        {
            await ValidUser(request.EmployeeId);
            await ValidUser(request.AuthorId);
            await ValidCompany(request.CompanyId);

            Data.Model.WorkingTime workingTime = new()
            {
                WorkingDate = request.WorkingDate,
                SecondsOfWork = request.SecondsOfWork,
                Note = request.Note is null ? "" : request.Note,
                CreatedAt = DateTime.UtcNow,
                LastModified = DateTime.UtcNow,
                Deleted = false,
                CompanyId = request.CompanyId,
                EmployeeId = request.EmployeeId,
                AuthorId = request.AuthorId,
                LastEditorId = request.AuthorId
            };
            await _unitOfWork.WorkingTimeRepository.AddAsync(workingTime);
            await _unitOfWork.SaveChangesAsync();
        }

        private async Task ValidUser(int userId)
        {
            if (!await _unitOfWork.UserRepository.AnyAsync(userId))
                throw new AuthException($"User with id:{userId} does not exist.");
        }

        private async Task ValidCompany(int? companyId) 
        {
            if (companyId is not null)
                if (!await _unitOfWork.CompanyRepository.AnyAsync((int)companyId))
                    throw new AuthException($"Company with id:{companyId} does not exits.");
        }
    }
}
