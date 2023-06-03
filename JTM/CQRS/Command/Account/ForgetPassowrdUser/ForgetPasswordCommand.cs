using JTM.DTO.Account;
using MediatR;

namespace JTM.CQRS.Command.Account.ForgetPassowrdUser
{
    public record ForgetPasswordCommand : IRequest<AuthResponseDto>
    {
        public string? Email { get; init; }

        public ForgetPasswordCommand(string? email)
        {
            Email = email;
        }
    }
}
