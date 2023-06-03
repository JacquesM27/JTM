using JTM.DTO.Account;
using MediatR;

namespace JTM.CQRS.Command.Account
{
    public record RefreshTokenCommand : IRequest<AuthResponseDto>
    {
    }
}
