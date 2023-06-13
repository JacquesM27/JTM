using JTM.Data.UnitOfWork;
using JTM.DTO.WorkingTime;
using MediatR;
using System.Linq.Expressions;

namespace JTM.CQRS.Query.WorkingTime.GetWorkingTimes
{
    public class GetWorkingTimesQueryHandler : IRequestHandler<GetWorkingTimesQuery, IEnumerable<BasicWorkingTimeDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetWorkingTimesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<BasicWorkingTimeDto>> Handle(GetWorkingTimesQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Data.Model.WorkingTime, bool>> filter = wt => !wt.Deleted;
            if(request.EmployeesId is not null && request.EmployeesId.Any())
                filter = filter = wt => request.EmployeesId.Contains(wt.EmployeeId);
            if (request.CompaniesId is not null && request.CompaniesId.Any())
                filter = filter = wt => wt.CompanyId != null && request.CompaniesId.Contains((int)wt.CompanyId);
            if (request.EndDate is not null)
                filter = filter = wt => wt.WorkingDate <= request.EndDate;
            if(request.StartDate is not null)
                filter = filter = wt => wt.WorkingDate >= request.StartDate;
            if(!string.IsNullOrEmpty(request.NoteSearchPhrase))
                filter = filter = wt => wt.Note.Contains(request.NoteSearchPhrase);

            var includeProperties = new Expression<Func<Data.Model.WorkingTime, object>>[]
            {
                wt => wt.Employee,
                wt => wt.Company,
                wt => wt.Author,
                wt => wt.LastEditor
            };
            var workingTimes = await _unitOfWork.WorkingTimeRepository.QueryAsync(filter, includeProperties);

            var basicDtos = workingTimes.Select(c => new BasicWorkingTimeDto()
            {
                Company = c.Company is null ? "" : c.Company.Name,
                WorkingDate = c.WorkingDate,
                Id = c.Id,
                SecondsOfWork = c.SecondsOfWork,
                UserName = c.Employee.Username
            }).ToList();

            return basicDtos;
        }
    }
}
