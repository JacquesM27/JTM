using JTM.DTO.WorkingTime;
using MediatR;

namespace JTM.CQRS.Query.WorkingTime
{
    public sealed record GetWorkingTimeQuery : IRequest<DetailsWorkingTimeDto>
    {
        public int WorkingTimeId { get; init; }

        public GetWorkingTimeQuery(int workingTimeId)
        {
            WorkingTimeId = workingTimeId;
        }
    }
}
