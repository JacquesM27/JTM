using JTM.Enum;
using MediatR;

namespace JTM.CQRS.Command.Account
{
    public sealed record RegisterCommand : IRequest
    {
        public string UserName { get; init; }
        public string Email { get; init; }
        public string Password { get; init; }
        public UserRole UserRole { get; init; }

        public RegisterCommand(string userName, string email, string password, UserRole userRole)
        {
            UserName = userName;
            Email = email;
            Password = password;
            UserRole = userRole;
        }
    }
}
