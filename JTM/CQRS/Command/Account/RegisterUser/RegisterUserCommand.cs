using MediatR;

namespace JTM.CQRS.Command.Account.RegisterUser
{
    public class RegisterUserCommand : IRequest
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
