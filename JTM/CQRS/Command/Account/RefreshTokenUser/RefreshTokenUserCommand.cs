using JTM.DTO.Account;
using MediatR;

namespace JTM.CQRS.Command.RefreshTokenUser
{
    public class RefreshTokenUserCommand : IRequest<AuthResponseDto>
    {
    }
}
