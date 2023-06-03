using MediatR;

namespace JTM.CQRS.Command.Account
{
    public sealed record RefreshConfirmTokenCommand : IRequest
    {
        public string Email { get; init; }

        public RefreshConfirmTokenCommand(string email)
        {
            Email = email;
        }
    }
}
