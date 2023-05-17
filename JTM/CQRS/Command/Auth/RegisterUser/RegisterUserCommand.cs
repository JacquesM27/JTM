using MediatR;

namespace JTM.CQRS.Command.Auth.RegisterUser
{
    public class RegisterUserCommand : IRequest<int>
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
