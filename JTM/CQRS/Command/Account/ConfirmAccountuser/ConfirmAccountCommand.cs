using JTM.DTO.Account;
using MediatR;

namespace JTM.CQRS.Command.Account.ConfirmAccountUser
{
    public record ConfirmAccountCommand : IRequest<AuthResponseDto>
    {
        public int UserId { get; init; }
        public string? Token { get; init; }

        public ConfirmAccountCommand(int userId, string? token)
        {
            UserId = userId;
            Token = token;
        }
    }
}
