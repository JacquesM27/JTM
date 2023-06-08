using JTM.DTO.Account;
using MediatR;

namespace JTM.CQRS.Command.Account
{
    public sealed record RefreshTokenCommand : IRequest<AuthResponseDto>
    {
        public string? RefreshToken { get; set; }

        public RefreshTokenCommand(string? refreshToken)
        {
            RefreshToken = refreshToken;
        }
    }
}
