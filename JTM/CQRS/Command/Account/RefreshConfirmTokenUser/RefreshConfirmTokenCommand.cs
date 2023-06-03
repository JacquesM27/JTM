using MediatR;

namespace JTM.CQRS.Command.Account
{
    public record RefreshConfirmTokenCommand : IRequest
    {
        public string Email { get; init; }

        public RefreshConfirmTokenCommand(string email)
        {
            Email = email;
        }
    }
}
