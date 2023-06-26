using MediatR;

namespace JTM.CQRS.Command.Account
{
    public class UnbanCommand : IRequest<int>
    {
        public int UserId { get; init; }

        public UnbanCommand(int userId) 
        { 
            UserId = userId;
        }
    }
}
