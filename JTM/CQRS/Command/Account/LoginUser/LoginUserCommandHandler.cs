using JTM.Data;
using JTM.DTO.Account;
using JTM.Helper.PasswordHelper;
using JTM.Services.TokenService;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JTM.CQRS.Command.Account.LoginUser
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
            {
                return new AuthResponseDto { Message = "User not found." };
            }
            else if (user.Banned)
            {
                return new AuthResponseDto { Message = "Account banned." };
            }
            else if (user.EmailConfirmed is false)
            {
                return new AuthResponseDto { Message = "Account not activated." };
            }
            else if (!PasswordHelper.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return new AuthResponseDto { Message = "Wrong password." };
            }

            string token = _tokenService.CreateToken(user);
            var refreshToken = _tokenService.CreateRefreshToken();
            await _tokenService.SetRefreshToken(refreshToken, user);
            return new AuthResponseDto
            {
                Success = true,
                Token = token,
                RefreshToken = refreshToken.Token,
                TokenExpires = refreshToken.Expires
            };
        }
    }
}
