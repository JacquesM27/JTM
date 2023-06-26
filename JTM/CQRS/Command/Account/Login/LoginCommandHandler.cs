using JTM.Data.Model;
using JTM.Data.UnitOfWork;
using JTM.DTO.Account;
using JTM.Exceptions;
using JTM.Helper.PasswordHelper;
using JTM.Services.TokenService;
using MediatR;
using System.Linq.Expressions;

namespace JTM.CQRS.Command.Account
{
    public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;

        public LoginCommandHandler(IUnitOfWork unitOfWork, ITokenService authService)
        {
            _unitOfWork = unitOfWork;
            _tokenService = authService;
        }

        public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            Expression<Func<User, bool>> filter = user => user.Email == request.Email;
            var user = await _unitOfWork.UserRepository.QuerySingleAsync(filter);

            if (user is null)
                throw new AuthException("Invalid user.");
            else if (user.Banned)
                throw new AuthException("Account blocked.");
            else if (user.EmailConfirmed is false)
                throw new AuthException("Account not activated.");
            else if (!PasswordHelper.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
                throw new AuthException("Wrong password.");

            string token = _tokenService.CreateToken(user);
            var refreshToken = _tokenService.CreateRefreshToken();
            await _tokenService.SetRefreshToken(refreshToken, user);
            await _unitOfWork.SaveChangesAsync();
            return new AuthResponseDto(
                token: token,
                refreshToken: refreshToken.Token,
                tokenExpires: refreshToken.Expires);
        }
    }
}
