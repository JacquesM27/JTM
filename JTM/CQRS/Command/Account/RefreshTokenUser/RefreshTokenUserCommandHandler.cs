using JTM.Data;
using JTM.DTO.Account;
using JTM.Exceptions;
using JTM.Services.TokenService;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JTM.CQRS.Command.Account
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
                throw new AuthException("Invalid user.");
            else if (user.Banned)
                throw new AuthException("Account banned.");
            else if (user.TokenExpires < DateTime.UtcNow)
                throw new AuthException("Token expired.");

            string token = _tokenService.CreateToken(user);
            var newRefreshToken = _tokenService.CreateRefreshToken();
            await _tokenService.SetRefreshToken(newRefreshToken, user);

            return new AuthResponseDto(
                token: token,
                refreshToken: newRefreshToken.Token,
                tokenExpires: newRefreshToken.Expires);
        }
    }
}
