using JTM.Data.UnitOfWork;
using JTM.DTO.WorkingTime;
using JTM.Exceptions;
using MediatR;
using System.Linq.Expressions;

namespace JTM.CQRS.Query.WorkingTime
{
    public class GetWorkingTimeQueryHandler : IRequestHandler<GetWorkingTimeQuery, DetailsWorkingTimeDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetWorkingTimeQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DetailsWorkingTimeDto> Handle(GetWorkingTimeQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Data.Model.WorkingTime, bool>> filter = wt 
                => wt.Id == request.WorkingTimeId 
                    && !wt.Deleted;
            var includeProperties = new Expression<Func<Data.Model.WorkingTime, object>>[]
            {
                wt => wt.Employee,
                wt => wt.Company,
                wt => wt.Author,
                wt => wt.LastEditor
            };
            var workingTime = await _unitOfWork.WorkingTimeRepository.QuerySingleAsync(filter, includeProperties)
                ?? throw new NotFoundException($"Working time with id:{request.WorkingTimeId} not found.");

            return new DetailsWorkingTimeDto()
            {
                Id = workingTime.Id,
                WorkingDate = workingTime.WorkingDate,
                SecondsOfWork = workingTime.SecondsOfWork,
                Note = workingTime.Note,
                EmployeeName = workingTime.Employee.Username,
                AuthorName = workingTime.Author.Username,
                LastEditorName = workingTime.LastEditor.Username,
                Company = workingTime.Company.Name
            };
        }
    }
}
