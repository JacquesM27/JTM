namespace JTM.Services.TokenService
{
    public interface ITokenService
    {
        string CreateToken(User user);
        RefreshToken CreateRefreshToken();
        Task SetRefreshToken(RefreshToken refreshToken, User user);
    }
}
