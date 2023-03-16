namespace JTM.Services.AuthService
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterUser(UserRegisterDto request);
        Task<AuthResponseDto> Login(UserDto request);
        Task<AuthResponseDto> RefreshToken();
        Task<AuthResponseDto> ConfirmAccount(int userId, string token);
        Task<AuthResponseDto> ChangePassword(ChangePasswordDto request);
        Task<AuthResponseDto> ForgetPassword(string email);
    }
}
