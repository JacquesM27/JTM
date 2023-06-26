using MediatR;

namespace JTM.CQRS.Command.Account
{
    public sealed record BanCommand : IRequest<int>
    {
        public int UserId { get; init; }

        public BanCommand(int userId)
        {
            UserId = userId;
        }
    }
}
