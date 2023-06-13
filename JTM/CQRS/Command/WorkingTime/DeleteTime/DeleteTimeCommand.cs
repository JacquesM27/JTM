using MediatR;

namespace JTM.CQRS.Command.WorkingTime
{
    public sealed record DeleteTimeCommand : IRequest
    {
        public int WorkingTimeId { get; init; }
        public int DeletorId { get; init; }

        public DeleteTimeCommand(int workingTimeId, int deletorId)
        {
            WorkingTimeId = workingTimeId;
            DeletorId = deletorId;
        }
    }
}
