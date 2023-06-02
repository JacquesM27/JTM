using JTM.DTO.Account;
using MediatR;

namespace JTM.CQRS.Command.LoginUser
{
    public class LoginUserCommand : IRequest<AuthResponseDto>
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
