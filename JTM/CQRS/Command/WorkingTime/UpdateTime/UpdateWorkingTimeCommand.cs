using MediatR;

namespace JTM.CQRS.Command.WorkingTime
{
    public sealed record UpdateWorkingTimeCommand : IRequest
    {
        public int HeaderId { get; init; }
        public int RouteId { get; init; }
        public DateTime WorkingDate { get; init; }
        public int SecondsOfWork { get; init; }
        public string? Note { get; init; }
        public int? CompanyId { get; init; }
        public int EmployeeId { get; init; }
        public int EditorId { get; init; }

        public UpdateWorkingTimeCommand(int headerId, int routeId, DateTime workingDate, int secondsOfWork, string? note, int? companyId, int employeeId, int editorId)
        {
            HeaderId = headerId;
            RouteId = routeId;
            WorkingDate = workingDate;
            SecondsOfWork = secondsOfWork;
            Note = note;
            CompanyId = companyId;
            EmployeeId = employeeId;
            EditorId = editorId;
        }
    }
}
