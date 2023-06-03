using JTM.DTO.Account;
using MediatR;

namespace JTM.CQRS.Command.Account.BanUser
{
    public record BanUserCommand : IRequest<AuthResponseDto>
    {
        public int UserId { get; init; }

        public BanUserCommand(int userId)
        {
            UserId = userId;
        }
    }
}
