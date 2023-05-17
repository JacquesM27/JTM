using JTM.DTO.Account;
using JTM.DTO.Account.RegisterUser;

namespace JTM.Services.AuthService
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterUser(RegisterUserDto request);
        Task<AuthResponseDto> Login(UserDto request);
        Task<AuthResponseDto> RefreshToken();
        Task<AuthResponseDto> ConfirmAccount(int userId, string token);
        Task<AuthResponseDto> ChangePassword(ChangePasswordDto request);
        Task<AuthResponseDto> ForgetPassword(string email);
        Task<AuthResponseDto> RefreshActivationToken(string email);
    }
}
