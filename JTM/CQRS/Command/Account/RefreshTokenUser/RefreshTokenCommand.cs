using JTM.DTO.Account;
using MediatR;

namespace JTM.CQRS.Command.Account.RefreshTokenUser
{
    public record RefreshTokenCommand : IRequest<AuthResponseDto>
    {
    }
}
