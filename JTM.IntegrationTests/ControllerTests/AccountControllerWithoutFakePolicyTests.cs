using JTM.Data;
using JTM.Data.Model;
using JTM.DTO.Account;
using JTM.DTO.Account.RegisterUser;
using JTM.DTO.ExceptionResponse;
using JTM.Helper.PasswordHelper;
using JTM.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Security.Cryptography;

namespace JTM.IntegrationTests.ControllersTests
{

    public class AccountControllerWithoutFakePolicyTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;
        private readonly WebApplicationFactory<Program> _factory;

        public AccountControllerWithoutFakePolicyTests()
        {
            _factory = new TestWebApplicationFactory();
            _httpClient =  _factory.CreateClient();
        }

        [Fact]
        public async Task Register_ForTotallyWrongUser_ReturnForbiddenStatusCode()
        {
            // Arrage
            var dto = new RegisterUserDto(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            var content = dto.ToJsonHttpContent();

            // Act
            var response = await _httpClient.PostAsync("/api/Account/register", content);
            var errorMessage = await response.FromJsonHttpResponseMessage<ExceptionResponse>();

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.Forbidden, response.StatusCode);
            Assert.Equal(403, errorMessage!.StatusCode);
            Assert.Equal("ValidationErrors", errorMessage!.Title);
            Assert.NotEmpty(errorMessage!.Errors);
        }

        [Fact]
        public async Task Register_ForBusyEmail_ReturnForbiddenStatusCode()
        {
            // Arrange
            string tmpEmail = "test@test.test";
            var dto = new RegisterUserDto("123", tmpEmail, tmpEmail, "12345Abc!@", "12345Abc!@");
            var content = dto.ToJsonHttpContent();
            await SeedUser(new User { Email = tmpEmail, EmailConfirmed = true });

            // Act
            var response = await _httpClient.PostAsync("/api/Account/register", content);
            var errorMessage = await response.FromJsonHttpResponseMessage<ExceptionResponse>();

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.Forbidden, response.StatusCode);
            Assert.Equal(403, errorMessage!.StatusCode);
            Assert.Equal("ValidationErrors", errorMessage!.Title);
            Assert.NotEmpty(errorMessage!.Errors);
        }

        [Fact]
        public async Task Register_ForValidData_ShouldReturnSuccess()
        {
            // Arrange
            string tmpEmail = "valid@valid.com";
            var dto = new RegisterUserDto("valid user", tmpEmail, tmpEmail, "12345Abc!@", "12345Abc!@");
            var content = dto.ToJsonHttpContent();

            // Act
            var response = await _httpClient.PostAsync("/api/Account/register", content);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task RegisterAdmin_ForUnathorizedTry_ShouldReturnUnauthorized()
        {
            // Arrange
            var dto = new RegisterUserDto(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            var content = dto.ToJsonHttpContent();

            // Act
            var response = await _httpClient.PostAsync("/api/Account/registerAdmin", content);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task LoginUser_ForValidUser_ShouldReturnSuccessWithTokens()
        {
            // Arrange
            string email = "valid@login.com";
            string password = "12345Abc!@";
            await SeedConfirmedUser(email, password);
            var dto = new UserDto(email, password);
            var content = dto.ToJsonHttpContent();

            // Act
            var response = await _httpClient.PostAsync("/api/Account/login", content);
            var responseMessage = await response.FromJsonHttpResponseMessage<AuthResponseDto>();

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.NotEmpty(responseMessage!.Token);
            Assert.NotEmpty(responseMessage!.RefreshToken);
        }

        [Fact]
        public async Task RefreshToken_ForUnauthorizedTry_ShouldReturnUnauthorized()
        {
            // Arrange
            // Act
            var response = await _httpClient.PostAsync("/api/Account/refresh-token", null);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task ForgetPassword_ForInvalidEmail_ShouldReturnForbidden()
        {
            // Arrange
            var dto = new EmailDto("");
            var content = dto.ToJsonHttpContent();

            // Act
            var response = await _httpClient.PostAsync("/api/Account/forget-password", content);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ForgetPassword_ForValidEmail_ShouldReturnOk()
        {
            // Arrange
            string email = "valid@reset.com";
            var dto = new EmailDto(email);
            var content = dto.ToJsonHttpContent();
            await SeedConfirmedUser(email, "12345Abc!");

            // Act
            var response = await _httpClient.PostAsync("/api/Account/forget-password", content);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task ConfirmAccount_ForInvalidUser_ShouldReturnBadRequest()
        {
            // Arrange
            ConfirmDto request = new(1, "");

            // Act
            var response = await _httpClient.PostAsJsonAsync("/api/Account/confirm", request);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ConfirmAccount_ForValidUser_ShouldReturnOk()
        {
            // Arrange
            var tmpUser = await SeedUnconfirmedUser("test@unconfirmed.com", "12345Abc!@");
            ConfirmDto request = new(tmpUser.Id, tmpUser.ActivationToken!);

            // Act
            var response = await _httpClient.PostAsJsonAsync("/api/Account/confirm", request);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task RefreshConfirmToken_ForInvalidUser_ShouldReturnBadRequest()
        {
            // Arrange
            var request = new EmailDto(string.Empty);

            // Act
            var response = await _httpClient.PostAsJsonAsync("/api/Account/confirm-refresh", request);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task RefreshConfirmToken_ForValidUser_ShouldReturnOk()
        {
            // Arrange
            string email = "refresh@unconfirmed.com";
            await SeedUnconfirmedUser(email, "12345Abc!@");
            var request = new EmailDto(email);

            // Act
            var response = await _httpClient.PostAsJsonAsync("/api/Account/confirm-refresh", request);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task ChangePassword_ForInvalidUser_ShouldReturnBadRequest()
        {
            // Arrange
            var request = new ChangePasswordDto(1, string.Empty, string.Empty);

            // Act
            var response = await _httpClient.PostAsJsonAsync("/api/Account/change-password", request);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ChangePassword_ForValidUser_ShouldReturnOk()
        {
            // Arrange
            string email = "forgot@password.com";
            var user = await SeedConfirmedUser(email, "12345Abc!@");
            var request = new ChangePasswordDto(user.Id, "@!cbA54321", user.PasswordResetToken!);

            // Act
            var response = await _httpClient.PostAsJsonAsync("/api/Account/change-password", request);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Banhammer_ForUnauthorizedTry_ShouldReturnUnauthorized()
        {
            // Arrange
            // Act
            var response = await _httpClient.PostAsync("/api/Account/banhammer?userId=0", null);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Unban_ForUnauthorizedTry_ShouldReturnUnauthorized()
        {
            // Arrange
            // Act
            var response = await _httpClient.PostAsync("/api/Account/unban?userId=0", null);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }

        private async Task SeedUser(User user)
        {
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var dataContext = scope.ServiceProvider.GetService<DataContext>();
            await dataContext.Users.AddAsync(user);
            await dataContext.SaveChangesAsync();
        }

        private async Task<User> SeedConfirmedUser(string email, string password)
        {
            PasswordHelper.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            var someUser = new User()
            {
                Email = email,
                Banned = false,
                EmailConfirmed = true,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                PasswordTokenExpires = DateTime.UtcNow.AddDays(1),
                PasswordResetToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64))
        };
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var dataContext = scope.ServiceProvider.GetService<DataContext>();
            await dataContext.Users.AddAsync(someUser);
            await dataContext.SaveChangesAsync();
            return someUser;
        }

        private async Task<User> SeedUnconfirmedUser(string email, string password)
        {
            PasswordHelper.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            var someUser = new User()
            {
                Email = email,
                Banned = false,
                EmailConfirmed = false,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                ActivationToken = Guid.NewGuid().ToString(),
                ActivationTokenExpires = DateTime.UtcNow.AddMinutes(10)
            };
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var dataContext = scope.ServiceProvider.GetService<DataContext>();
            await dataContext.Users.AddAsync(someUser);
            await dataContext.SaveChangesAsync();
            return someUser;
        }
    }
}
