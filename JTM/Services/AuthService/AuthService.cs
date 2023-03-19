using Dapper;
using JTM.Data;
using JTM.Data.DapperConnection;
using JTM.DTO.Validator;
using JTM.Model;
using JTM.Services.MailService;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Asn1.Ocsp;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace JTM.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDapperConnectionFactory _connectionFactory;

        public AuthService(DataContext dataContext, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IDapperConnectionFactory connectionFactory)
        {
            _dataContext = dataContext;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _connectionFactory = connectionFactory;
        }

        public async Task<AuthResponseDto> Login(UserDto request)
        {
            var user = await _dataContext.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(u => u.Email == request.Email);
            if (user is null)
            {
                return new AuthResponseDto { Message = "User not found." };
            }
            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return new AuthResponseDto { Message = "Wrong password." };
            }
            string token = CreateToken(user);
            var refreshToken = CreateRefreshToken();
            SetRefreshToken(refreshToken, user);
            return new AuthResponseDto
            {
                Success = true,
                Token = token,
                RefreshToken = refreshToken.Token,
                TokenExpires = refreshToken.Expires
            };
        }

        public async Task<AuthResponseDto> ChangePassword(ChangePasswordDto request)
        {
            var user = await _dataContext.Users.SingleOrDefaultAsync(c => c.Id == request.UserId);
            if (user is null)
            {
                return new AuthResponseDto { Message = "Invalid user." };
            }
            else if (!request.Token.Equals(user.PasswordResetToken))
            {
                return new AuthResponseDto { Message = "Invalid token." };
            }
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.PasswordResetToken = null;

            await _dataContext.SaveChangesAsync();
            return new AuthResponseDto { Success = true, };
        }

        public async Task<AuthResponseDto> ConfirmAccount(int userId, string token)
        {
            var user = await _dataContext.Users.SingleOrDefaultAsync(c => c.Id == userId);
            if (user is null)
            {
                return new AuthResponseDto { Message = "Invalid user." };
            }
            else if (user.EmailConfirmed)
            {
                return new AuthResponseDto { Message = "User already confirmed." };
            }
            else if (user.ActivationTokenExpires < DateTime.UtcNow)
            {
                return new AuthResponseDto { Message = "Token expired." };
            }
            else if (!token.Equals(user.ActivationToken))
            {
                return new AuthResponseDto { Message = "Invalid token." };
            }

            user.EmailConfirmed = true;
            user.ActivationToken = null;
            await _dataContext.SaveChangesAsync();
            return new AuthResponseDto()
            {
                Success = true,
            };
        }

        public async Task<AuthResponseDto> RefreshToken()
        {
            var refreshToken = _httpContextAccessor?.HttpContext?.Request.Cookies["refreshToken"];
            var user = await _dataContext.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(c => c.RefreshToken == refreshToken);
            if (user is null)
            {
                return new AuthResponseDto { Message = "Invalid Refresh Token!" };
            }
            else if (user.TokenExpires < DateTime.UtcNow)
            {
                return new AuthResponseDto { Message = "Token expired." };
            }

            string token = CreateToken(user);
            var newRefreshToken = CreateRefreshToken();
            SetRefreshToken(newRefreshToken, user);

            return new AuthResponseDto 
            { 
                Success = true, 
                Token = token, 
                RefreshToken= newRefreshToken.Token, 
                TokenExpires= newRefreshToken.Expires 
            };
        }

        public async Task<AuthResponseDto> RegisterUser(UserRegisterDto request)
        {
            var userValidator = new UserRegisterDtoValidator(_dataContext);
            var validatorResult = await userValidator.ValidateAsync(request);
            if (!validatorResult.IsValid)
            {
                var errorMessages = validatorResult.Errors.Select(x => x.ErrorMessage).ToList();
                return new AuthResponseDto { Message = string.Join("\n", errorMessages) };
            }

            var user = await _dataContext.Users.SingleOrDefaultAsync(c => c.Email.Equals(request.Email));
            if (user is not null)
            {
                return new AuthResponseDto { Message = "Email is already taken." };
            }
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            User newUser = new()
            {
                Username = request.UserName,
                Email = request.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                ActivationToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                ActivationTokenExpires = DateTime.UtcNow.AddDays(1),
                PasswordResetToken = null
            };

            _dataContext.Add(newUser);
            await _dataContext.SaveChangesAsync();
            return new AuthResponseDto
            { 
                Success= true,
            };
        }

        public async Task<AuthResponseDto> RefreshActivationToken(string email)
        {
            var user = await _dataContext.Users.SingleOrDefaultAsync(c => c.Email.Equals(email));
            if (user is null)
            {
                return new AuthResponseDto { Message = "Invalid user." };
            }
            user.ActivationToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            user.ActivationTokenExpires = DateTime.UtcNow.AddDays(1);

            await _dataContext.SaveChangesAsync();
            return new AuthResponseDto { Success= true, };
        }

        public async Task<AuthResponseDto> ForgetPassword(string email)
        {
            var user = await _dataContext.Users.SingleOrDefaultAsync(c => c.Email.Equals(email));
            if(user is null)
            {
                return new AuthResponseDto { Message = "Invalid user." };
            }
            user.PasswordTokenExpires= DateTime.UtcNow.AddDays(1);
            user.PasswordResetToken= Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            await _dataContext.SaveChangesAsync();
            return new AuthResponseDto() { Success= true,};
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds,
                issuer: "secret key",
                audience: "secret key2"
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        private RefreshToken CreateRefreshToken()
        {
            var refreshToken = new RefreshToken()
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow
            };
            return refreshToken;
        }

        private async void SetRefreshToken(RefreshToken refreshToken, User user)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = refreshToken.Expires
            };
            _httpContextAccessor?.HttpContext?.Response
                .Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);

            string query = @"UPDATE dbo.Users 
                            SET RefreshToken = @Token, TokenCreated = @Created, TokenExpires = @Expires
                            WHERE Id = @userId";
            var parameters = new { refreshToken.Token, refreshToken.Created, refreshToken.Expires, userId = user.Id };
            using var connection = _connectionFactory.DbConnection;
            await connection.ExecuteAsync(query, parameters);
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash= hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

    }
}
