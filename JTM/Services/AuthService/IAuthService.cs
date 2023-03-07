namespace JTM.Services.AuthService
{
    public interface IAuthService
    {
        Task<User> RegisterUser(UserDto request);
        Task<AuthResponseDto> Login(UserDto request);
        Task<AuthResponseDto> RefreshToken();
        Task<User> Confirm(int userId, string token);
    }
}
