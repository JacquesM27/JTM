using JTM.DTO.Account;
using MediatR;

namespace JTM.CQRS.Command.Account.ChangePassowrdUser
{
    public class ChangePasswordCommand : IRequest<AuthResponseDto>
    {
        public int UserId { get; set; }
        public string Password { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
