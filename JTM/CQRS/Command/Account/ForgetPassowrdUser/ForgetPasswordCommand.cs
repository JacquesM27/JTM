using MediatR;

namespace JTM.CQRS.Command.Account
{
    public sealed record ForgetPasswordCommand : IRequest
    {
        public string Email { get; init; }

        public ForgetPasswordCommand(string email)
        {
            Email = email;
        }
    }
}
