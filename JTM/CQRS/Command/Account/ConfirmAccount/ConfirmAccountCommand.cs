using MediatR;

namespace JTM.CQRS.Command.Account
{
    public sealed record ConfirmAccountCommand : IRequest
    {
        public int UserId { get; init; }
        public string Token { get; init; }

        public ConfirmAccountCommand(int userId, string token)
        {
            UserId = userId;
            Token = token;
        }
    }
}
