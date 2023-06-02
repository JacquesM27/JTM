using JTM.DTO.Account;
using MediatR;

namespace JTM.CQRS.Command.Account.ForgetPassowrdUser
{
    public class ForgetPasswordCommand : IRequest<AuthResponseDto>
    {
        public string? Email { get; set; }
    }
}
