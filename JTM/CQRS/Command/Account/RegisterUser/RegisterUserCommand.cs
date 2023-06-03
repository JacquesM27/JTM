using MediatR;

namespace JTM.CQRS.Command.Account
{
    public record RegisterUserCommand : IRequest
    {
        public string UserName { get; init; }
        public string Email { get; init; }
        public string Password { get; init; }

        public RegisterUserCommand(string userName, string email, string password)
        {
            UserName = userName;
            Email = email;
            Password = password;
        }
    }
}
