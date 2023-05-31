using FluentAssertions;
using JTM.CQRS.Command.RegisterUser;
using JTM.CQRS.Command.SendRabbitActivationMessage;
using JTM.Data;
using JTM.DTO.Account;
using JTM.DTO.Account.RegisterUser;
using JTM.IntegrationTests.Helpers;
using JTM.Services.AuthService;
using JTM.Services.RabbitService;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using RabbitMQ.Client;

namespace JTM.IntegrationTests.ControllersTests
{
    public class AuthControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        //private readonly string _testConnectionString;
        private readonly HttpClient _httpClient;
        private readonly Mock<ITokenService> _authServiceMock = new();
        private readonly Mock<IMediator> _mediatorMock = new();
        private readonly Mock<DataContext> _dataContextMock = new();

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
        public async Task Login_ForValidUser_ReturnOk()
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
                Email = "test3",
                Password = "password",
            };
            var httpContent = userDto.ToJsonHttpContent();

            //act
            var response = await _httpClient.PostAsync("/api/account/login", httpContent);

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task Login_ForInvalidUser_ReturnBadRequest()
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
        public async Task Register_ForInvalidModel_ReturnBadRequest()
        {
            //arrange
            var registerUser = new RegisterUserDto()
            {
                Email = "test",
                Password = "password",
                UserName = "test"
            };
            //_authServiceMock
            //   .Setup(e => e.RegisterUser(It.IsAny<RegisterUserDto>()))
            //   .ReturnsAsync(new AuthResponseDto() { Success = false, });
            _mediatorMock
                .Setup(e => e.Send(It.IsAny<RegisterUserCommand>(), default))
                .ReturnsAsync(It.IsAny<int>());
            var httpContent = registerUser.ToJsonHttpContent();

            //act
            var response = await _httpClient.PostAsync("/api/account/register", httpContent);

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

    

        [Fact]
        public async Task ForgetPassword_ForInvalidEmail_ReturnBadRequest()
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
        public async Task ForgetPassword_ForNoneEmail_ReturnBadRequest()
        {
            //arrange

            //act
            var response = await _httpClient.PostAsync("/api/Account/forget-password", null);

            //assert 
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task ForgetPassword_ForValidData_ReturnOk()
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
        public async Task RefreshToken_ForNoneCache_ReturnBadRequest()
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
        public async Task RefreshToken_ForOkCache_ReturnOk()
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
        public async Task ConfirmAccount_ForMissingToken_ReturnBadRequest()
        {
            //arrange

            //act
            var response = await _httpClient.PostAsync("/api/Account/confirm?userId=1", null);

            //assert 
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task ConfirmAccount_ForMissingUserId_ReturnBadRequest()
        {
            //arrange

            //act
            var response = await _httpClient.PostAsync("/api/Account/confirm?token=1234", null);

            //assert 
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task ConfirmAccount_ForInvalidInput_ReturnOk()
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
        public async Task ConfirmAccount_ForValidInput_ReturnOk()
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
        public async Task RefreshConfirmToken_ForMissingEmail_ReturnBadRequest()
        {
            //arrange

            //act
            var response = await _httpClient.PostAsync("/api/Account/confirm-refresh", null);

            //assert 
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task RefreshConfirmToken_ForValidEmail_ReturnOk()
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
        public async Task RefreshConfirmToken_ForInvalidInput_ReturnBadRequest()
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
        public async Task ChangePassword_ForInvalidInput_ReturnBadRequest()
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
        public async Task ChangePassword_ForValidInput_ReturnOk()
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
