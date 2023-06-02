using JTM.DTO.Account;
using MediatR;

namespace JTM.CQRS.Command.Account.LoginUser
{
    public class LoginCommand : IRequest<AuthResponseDto>
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
