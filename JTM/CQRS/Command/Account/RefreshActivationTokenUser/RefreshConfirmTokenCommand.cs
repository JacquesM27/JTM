using JTM.DTO.Account;
using MediatR;

namespace JTM.CQRS.Command.Account.RefreshActivationTokenUser
{
    public class RefreshConfirmTokenCommand : IRequest<AuthResponseDto>
    {
        public string? Email { get; set; }
    }
}
