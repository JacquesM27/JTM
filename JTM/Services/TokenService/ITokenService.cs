using JTM.Data.Model;
using JTM.DTO.Account;

namespace JTM.Services.TokenService
{
    public interface ITokenService
    {
        string CreateToken(User user);
        RefreshTokenDto CreateRefreshToken();
        Task SetRefreshToken(RefreshTokenDto refreshToken, User user);
    }
}
