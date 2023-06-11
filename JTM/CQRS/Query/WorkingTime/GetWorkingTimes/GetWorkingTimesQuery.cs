using JTM.DTO.WorkingTime;
using MediatR;

namespace JTM.CQRS.Query.WorkingTime.GetWorkingTimes
{
    public class GetWorkingTimesQuery : IRequest<IEnumerable<BasicWorkingTimeDto>>
    {
        public IEnumerable<int>? EmployeesId { get; init; }
        public DateTime? StartDate { get; init; }
        public DateTime? EndDate { get; init; }
        public IEnumerable<int>? CompaniesId { get; init; }
        public string? NoteSearchPhrase { get; init; }
        public int PageNumber { get; init; }
        public int PageSize { get; init; }

        public GetWorkingTimesQuery(IEnumerable<int>? employeesId, DateTime? startDate, DateTime? endDate, IEnumerable<int>? companiesId, string? noteSearchPhrase, int pageNumber, int pageSize)
        {
            EmployeesId = employeesId;
            StartDate = startDate;
            EndDate = endDate;
            CompaniesId = companiesId;
            NoteSearchPhrase = noteSearchPhrase;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
