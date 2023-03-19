using Microsoft.AspNetCore.Mvc.Testing;
using JTM;
using JTM.Data;
using JTM.Services.AuthService;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Connections;
using JTM.Data.DapperConnection;
using JTM.IntegrationTests.Helpers;
using Moq;
using JTM.DTO;
using FluentAssertions;
using JTM.Model;

namespace JTM.IntegrationTests.ControllersTests
{
    public class AuthControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        //private readonly string _testConnectionString;
        private readonly HttpClient _httpClient;
        private readonly Mock<IAuthService> _authServiceMock = new();

        public AuthControllerTests(WebApplicationFactory<Program> factory)
        {
            //var configuration = new ConfigurationBuilder()
            //    .AddJsonFile("appsettings.json")
            //    .Build();
            //_testConnectionString = configuration.GetConnectionString("TestConnectionString");
            _httpClient = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        //var dbContextOptions = services.SingleOrDefault(service =>
                        //    service.ServiceType == typeof(DbContextOptions<DataContext>));
                        //services.Remove(dbContextOptions);
                        //services.AddDbContext<DataContext>(options =>
                        //    options.UseSqlServer(_testConnectionString));
                        //var IDbConnection = services.Where(s => s.ServiceType == typeof(IDapperConnectionFactory)).ToList();
                        //foreach (var item in IDbConnection)
                        //{
                        //    services.Remove(item);
                        //}
                        //services.AddScoped<IDapperConnectionFactory, TestDapperConnectionFactory>();
                        services.AddSingleton(_authServiceMock.Object);
                    });
                })
                .CreateClient();
        }

        [Fact]
        public async Task Login_ForValidUser_ReturnsOk()
        {
            //arrange
            var authResponseDto = new AuthResponseDto()
            {
                Success = true,
            };
            _authServiceMock
                .Setup(e => e.Login(It.IsAny<UserDto>()))
                .ReturnsAsync(authResponseDto);

            var userDto = new UserDto()
            {
                Email = "test",
                Password = "password",
            };
            var httpContent = userDto.ToJsonHttpContent();

            //act
            var response = await _httpClient.PostAsync("/api/account/login", httpContent);

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task Login_ForInvalidUser_ReturnsBadRequest()
        {
            //arrange
            var authResponseDto = new AuthResponseDto() { Success = false, };
            _authServiceMock
               .Setup(e => e.Login(It.IsAny<UserDto>()))
               .ReturnsAsync(authResponseDto);

            var userDto = new UserDto()
            {
                Email = "test",
                Password = "password",
            };
            var httpContent = userDto.ToJsonHttpContent();

            //act
            var response = await _httpClient.PostAsync("/api/account/login", httpContent);

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Register_ForInvalidModel_ReturnsBadRequest()
        {
            //arrange
            var registerUser = new UserRegisterDto()
            {
                Email = "test",
                Password = "password",
                UserName = "test"
            };
            _authServiceMock
               .Setup(e => e.RegisterUser(It.IsAny<UserRegisterDto>()))
               .ReturnsAsync(new AuthResponseDto() { Success = false, });
            var httpContent = registerUser.ToJsonHttpContent();

            //act
            var response = await _httpClient.PostAsync("/api/account/register", httpContent);

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Register_ForValidModel_ReturnsOk()
        {
            //arrange
            var registerUser = new UserRegisterDto()
            {
                Email = "test",
                Password = "password",
                UserName = "test"
            };
            _authServiceMock
               .Setup(e => e.RegisterUser(It.IsAny<UserRegisterDto>()))
               .ReturnsAsync(new AuthResponseDto() { Success = true, });
            var httpContent = registerUser.ToJsonHttpContent();

            //act
            var response = await _httpClient.PostAsync("/api/account/register", httpContent);

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task ForgetPassword_ForInvalidEmail_ReturnsBadRequest()
        {
            //arrange
            _authServiceMock
                .Setup(e => e.ForgetPassword(It.IsAny<string>()))
                .ReturnsAsync(new AuthResponseDto() { Success = false });

            //act
            var response = await _httpClient.PostAsync("/api/Account/forget-password?email=test%40test.com", null);

            //assert 
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task ForgetPassword_ForNoneEmail_ReturnsBadRequest()
        {
            //arrange

            //act
            var response = await _httpClient.PostAsync("/api/Account/forget-password", null);

            //assert 
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task ForgetPassword_ForValidData_ReturnsOk()
        {
            //arrange
            _authServiceMock
                .Setup(e => e.ForgetPassword(It.IsAny<string>()))
                .ReturnsAsync(new AuthResponseDto() { Success = true });

            //act
            var response = await _httpClient.PostAsync("/api/Account/forget-password?email=test%40test.com", null);

            //assert 
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task RefreshToken_ForNoneCache_ReturnsBadRequest()
        {
            //arrange
            _authServiceMock
                .Setup(e => e.RefreshToken())
                .ReturnsAsync(new AuthResponseDto() { Success = false });

            //act
            var response = await _httpClient.PostAsync("/api/Account/refresh-token", null);

            //assert 
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task RefreshToken_ForOkCache_ReturnsOk()
        {
            //arrange
            _authServiceMock
                .Setup(e => e.RefreshToken())
                .ReturnsAsync(new AuthResponseDto() { Success = true });

            //act
            var response = await _httpClient.PostAsync("/api/Account/refresh-token", null);

            //assert 
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task ConfirmAccount_ForMissingToken_ReturnsBadRequest()
        {
            //arrange

            //act
            var response = await _httpClient.PostAsync("/api/Account/confirm?userId=1", null);

            //assert 
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task ConfirmAccount_ForMissingUserId_ReturnsBadRequest()
        {
            //arrange

            //act
            var response = await _httpClient.PostAsync("/api/Account/confirm?token=1234", null);

            //assert 
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task ConfirmAccount_ForInvalidInput_ReturnsOk()
        {
            //arrange
            _authServiceMock
                .Setup(e => e.ConfirmAccount(It.IsAny<int>(),It.IsAny<string>()))
                .ReturnsAsync(new AuthResponseDto() { Success = false });

            //act
            var response = await _httpClient.PostAsync("/api/Account/confirm?token=1234&userId=1", null);

            //assert 
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task ConfirmAccount_ForValidInput_ReturnsOk()
        {
            //arrange
            _authServiceMock
                .Setup(e => e.ConfirmAccount(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(new AuthResponseDto() { Success = true });

            //act
            var response = await _httpClient.PostAsync("/api/Account/confirm?token=1234&userId=1", null);

            //assert 
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task RefreshConfirmToken_ForMissingEmail_ReturnsBadRequest()
        {
            //arrange

            //act
            var response = await _httpClient.PostAsync("/api/Account/confirm-refresh", null);

            //assert 
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task RefreshConfirmToken_ForValidEmail_ReturnsOk()
        {
            //arrange
            _authServiceMock
                .Setup(e => e.RefreshActivationToken(It.IsAny<string>()))
                .ReturnsAsync(new AuthResponseDto() { Success = true });

            //act
            var response = await _httpClient.PostAsync("/api/Account/confirm-refresh?email=test%40test.com", null);

            //assert 
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task RefreshConfirmToken_ForInvalidInput_ReturnsBadRequest()
        {
            //arrange
            _authServiceMock
                .Setup(e => e.RefreshActivationToken(It.IsAny<string>()))
                .ReturnsAsync(new AuthResponseDto() { Success = false });

            //act
            var response = await _httpClient.PostAsync("/api/Account/confirm-refresh?email=test%40test.com", null);

            //assert 
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task ChangePassword_ForInvalidInput_ReturnsBadRequest()
        {
            //arrange
            _authServiceMock
               .Setup(e => e.ChangePassword(It.IsAny<ChangePasswordDto>()))
               .ReturnsAsync(new AuthResponseDto() { Success = false, });
            var changepassword = new ChangePasswordDto()
            {
                UserId = 1,
                Password = "password",
            };
            var httpContent = changepassword.ToJsonHttpContent();
            //act
            var response = await _httpClient.PostAsync("/api/account/change-password", httpContent);

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task ChangePassword_ForValidInput_ReturnsOk()
        {
            //arrange
            _authServiceMock
               .Setup(e => e.ChangePassword(It.IsAny<ChangePasswordDto>()))
               .ReturnsAsync(new AuthResponseDto() { Success = true, });
            var changepassword = new ChangePasswordDto()
            {
                UserId = 1,
                Password = "password",
            };
            var httpContent = changepassword.ToJsonHttpContent();
            //act
            var response = await _httpClient.PostAsync("/api/account/change-password", httpContent);

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}
