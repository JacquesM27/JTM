using JTM.Data;
using JTM.DTO.Account;
using JTM.Services.TokenService;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JTM.CQRS.Command.Account.RefreshTokenUser
{
    public class RefreshTokenUserCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResponseDto>
    {
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITokenService _tokenService;

        public RefreshTokenUserCommandHandler(
            DataContext dataContext,
            IHttpContextAccessor httpContextAccessor,
            ITokenService tokenService)
        {
            _dataContext = dataContext;
            _httpContextAccessor = httpContextAccessor;
            _tokenService = tokenService;
        }

        public async Task<AuthResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var refreshToken = _httpContextAccessor?.HttpContext?.Request.Cookies["refreshToken"];
            var user = await _dataContext.Users
                .SingleOrDefaultAsync(c => c.RefreshToken == refreshToken, cancellationToken);

            if (user is null)
            {
                return new AuthResponseDto { Message = "Invalid user." };
            }
            else if (user.TokenExpires < DateTime.UtcNow)
            {
                return new AuthResponseDto { Message = "Token expired." };
            }
            else if (user.Banned)
            {
                return new AuthResponseDto { Message = "Account banned." };
            }

            string token = _tokenService.CreateToken(user);
            var newRefreshToken = _tokenService.CreateRefreshToken();
            await _tokenService.SetRefreshToken(newRefreshToken, user);

            return new AuthResponseDto
            {
                Success = true,
                Token = token,
                RefreshToken = newRefreshToken.Token,
                TokenExpires = newRefreshToken.Expires
            };
        }
    }
}
