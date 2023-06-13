using MediatR;

namespace JTM.CQRS.Command.WorkingTime
{
    public sealed record AddWorkingTimeCommand : IRequest
    {
        public DateTime WorkingDate { get; init; }
        public int SecondsOfWork { get; init; }
        public string? Note { get; init; }
        public int? CompanyId { get; init; }
        public int EmployeeId { get; init; }
        public int AuthorId { get; init; }

        public AddWorkingTimeCommand(DateTime workingDate, int secondsOfWork, string? note, int? companyId, int employeeId, int authorId)
        {
            WorkingDate = workingDate;
            SecondsOfWork = secondsOfWork;
            Note = note;
            CompanyId = companyId;
            EmployeeId = employeeId;
            AuthorId = authorId;
        }
    }
}
