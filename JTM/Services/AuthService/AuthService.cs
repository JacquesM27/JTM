using JTM.Data;

namespace JTM.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _dataContext;

        public AuthService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public Task<AuthResponseDto> Login(UserDto request)
        {
            throw new NotImplementedException();
        }

        public Task<AuthResponseDto> RefreshToken()
        {
            throw new NotImplementedException();
        }

        public Task<User> RegisterUser(UserDto request)
        {
            throw new NotImplementedException();
        }
    }
}
