namespace JTM.Services.AuthService
{
    public interface IAuthService
    {
        Task<User> RegisterUser(UserRegisterDto request);
        Task<AuthResponseDto> Login(UserDto request);
        Task<AuthResponseDto> RefreshToken();
        Task<AuthResponseDto> ConfirmAccount(int userId, string token);
        Task<AuthResponseDto> ChangePassword(ChangePasswordDto request);
    }
}
