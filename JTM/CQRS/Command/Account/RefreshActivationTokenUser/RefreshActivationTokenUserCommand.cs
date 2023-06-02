using JTM.DTO.Account;
using MediatR;

namespace JTM.CQRS.Command.RefreshActivationTokenUser
{
    public class RefreshActivationTokenUserCommand : IRequest<AuthResponseDto>
    {
        public string? Email { get; set; }
    }
}
