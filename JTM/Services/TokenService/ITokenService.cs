namespace JTM.Services.TokenService
{
    public interface ITokenService
    {
        //Task<AuthResponseDto> RefreshToken();
        //Task<AuthResponseDto> ConfirmAccount(int userId, string token);
        //Task<AuthResponseDto> ChangePassword(ChangePasswordDto request);
        //Task<AuthResponseDto> ForgetPassword(string email);
        //Task<AuthResponseDto> RefreshActivationToken(string email);
        string CreateToken(User user);
        RefreshToken CreateRefreshToken();
        Task SetRefreshToken(RefreshToken refreshToken, User user);
    }
}
