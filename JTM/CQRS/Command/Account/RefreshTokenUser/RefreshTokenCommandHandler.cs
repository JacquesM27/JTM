using JTM.Data.Model;
using JTM.Data.UnitOfWork;
using JTM.DTO.Account;
using JTM.Exceptions;
using JTM.Services.TokenService;
using MediatR;
using System.Linq.Expressions;

namespace JTM.CQRS.Command.Account
{
    public sealed class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;

        public RefreshTokenCommandHandler(
            IUnitOfWork unitOfWork,
            ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
        }

        public async Task<AuthResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.RefreshToken))
                throw new AuthException("Missing token.");

            Expression<Func<User, bool>> filter = user => user.RefreshToken == request.RefreshToken;
            var user = await _unitOfWork.UserRepository.QuerySingleAsync(filter);

            if (user is null)
                throw new AuthException("Invalid token.");
            else if (user.Banned)
                throw new AuthException("Account banned.");
            else if (user.TokenExpires < DateTime.UtcNow)
                throw new AuthException("Token expired.");

            string token = _tokenService.CreateToken(user);
            var newRefreshToken = _tokenService.CreateRefreshToken();
            await _tokenService.SetRefreshToken(newRefreshToken, user);

            await _unitOfWork.SaveChangesAsync();

            return new AuthResponseDto(
                token: token,
                refreshToken: newRefreshToken.Token,
                tokenExpires: newRefreshToken.Expires);
        }
    }
}
