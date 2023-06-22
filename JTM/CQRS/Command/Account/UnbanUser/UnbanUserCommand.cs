using MediatR;

namespace JTM.CQRS.Command.Account
{
    public class UnbanUserCommand : IRequest<int>
    {
        public int UserId { get; init; }

        public UnbanUserCommand(int userId) 
        { 
            UserId = userId;
        }
    }
}
