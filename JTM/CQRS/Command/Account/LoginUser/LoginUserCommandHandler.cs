using JTM.Data;
using JTM.DTO.Account;
using JTM.Exceptions;
using JTM.Helper.PasswordHelper;
using JTM.Services.TokenService;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JTM.CQRS.Command.Account
{
    public class LoginUserCommandHandler : IRequestHandler<LoginCommand, AuthResponseDto>
    {
        private readonly DataContext _dataContext;
        private readonly ITokenService _tokenService;

        public LoginUserCommandHandler(DataContext dataContext, ITokenService authService)
        {
            _dataContext = dataContext;
            _tokenService = authService;
        }

        public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _dataContext.Users
                .SingleOrDefaultAsync(u => u.Email == request.Email, cancellationToken: cancellationToken);

            if (user is null)
                throw new AuthException("User not found.");
            else if (user.Banned)
                throw new AuthException("Account blocked.");
            else if (user.EmailConfirmed is false)
                throw new AuthException("Account not activated.");
            else if (!PasswordHelper.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
                throw new AuthException("Wrong password.");

            string token = _tokenService.CreateToken(user);
            var refreshToken = _tokenService.CreateRefreshToken();
            await _tokenService.SetRefreshToken(refreshToken, user);
            return new AuthResponseDto(
                token: token,
                refreshToken: refreshToken.Token,
                tokenExpires: refreshToken.Expires);
        }
    }
}
