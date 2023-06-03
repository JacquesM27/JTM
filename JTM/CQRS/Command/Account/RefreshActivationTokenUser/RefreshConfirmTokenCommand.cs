using JTM.DTO.Account;
using MediatR;

namespace JTM.CQRS.Command.Account.RefreshActivationTokenUser
{
    public record RefreshConfirmTokenCommand : IRequest<AuthResponseDto>
    {
        public string Email { get; init; }

        public RefreshConfirmTokenCommand(string email)
        {
            Email = email;
        }
    }
}
