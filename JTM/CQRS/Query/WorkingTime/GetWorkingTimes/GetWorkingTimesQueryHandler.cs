using JTM.DTO.WorkingTime;
using MediatR;

namespace JTM.CQRS.Query.WorkingTime.GetWorkingTimes
{
    public class GetWorkingTimesQueryHandler : IRequestHandler<GetWorkingTimesQuery, IEnumerable<BasicWorkingTimeDto>>
    {
        public Task<IEnumerable<BasicWorkingTimeDto>> Handle(GetWorkingTimesQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
