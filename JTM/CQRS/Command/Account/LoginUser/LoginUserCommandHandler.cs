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
        private readonly ITokenService _authService;

        public LoginUserCommandHandler(DataContext dataContext, ITokenService authService)
        {
            _dataContext = dataContext;
            _authService = authService;
        }

        public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _dataContext.Users
                .SingleOrDefaultAsync(u => u.Email == request.Email, cancellationToken: cancellationToken);

            if (user is null)
            {
                return new AuthResponseDto { Message = "User not found." };
            }
            else if (user.EmailConfirmed is false)
            {
                return new AuthResponseDto { Message = "Account not activated." };
            }
            else if (!PasswordHelper.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return new AuthResponseDto { Message = "Wrong password." };
            }

            string token = _authService.CreateToken(user);
            var refreshToken = _authService.CreateRefreshToken();
            await _authService.SetRefreshToken(refreshToken, user);
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
