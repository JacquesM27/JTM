using JTM.Data;
using JTM.DTO.WorkingTime;
using JTM.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JTM.CQRS.Query.WorkingTime
{
    public class GetWorkingTimeQueryHandler : IRequestHandler<GetWorkingTimeQuery, DetailsWorkingTimeDto>
    {
        private readonly DataContext _dataContext;

        public GetWorkingTimeQueryHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<DetailsWorkingTimeDto> Handle(GetWorkingTimeQuery request, CancellationToken cancellationToken)
        {
            var workingTime = await _dataContext.WorkingTimes
                .Include(t => t.Employee)
                .Include(t => t.Company)
                .Include(t => t.Author)
                .Include(t => t.LastEditor)
                .Where(t => !t.Deleted)
                .SingleOrDefaultAsync(w => w.Id == request.WorkingTimeId, cancellationToken)
                ?? throw new NotFoundException($"Working time with id:{request.WorkingTimeId} not found.");

            return new DetailsWorkingTimeDto()
            {
                Id = workingTime.Id,
                StartOdWorkingDate = workingTime.StartOdWorkingDate,
                EndOdWorkingDate = workingTime.EndOdWorkingDate,
                Note = workingTime.Note,
                EmployeeName = workingTime.Employee.Username,
                AuthorName = workingTime.Author.Username,
                LastEditorName = workingTime.LastEditor.Username,
                Company = workingTime.Company.Name
            };
        }
    }
}
