using JTM.DTO.Account;
using MediatR;

namespace JTM.CQRS.Command.ForgetPassowrdUser
{
    public class ForgetPasswordUserCommand : IRequest<AuthResponseDto>
    {
        public string? Email { get; set; }
    }
}
