using JTM.DTO.Account;
using MediatR;

namespace JTM.CQRS.Command.Account.ChangePassowrdUser
{
    public record ChangePasswordCommand : IRequest<AuthResponseDto>
    {
        public int UserId { get; init; }
        public string Password { get; init; }
        public string Token { get; init; }

        public ChangePasswordCommand(int userId, string password, string token)
        {
            UserId = userId;
            Password = password;
            Token = token;
        }
    }
}
