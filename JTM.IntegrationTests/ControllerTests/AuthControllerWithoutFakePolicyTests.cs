using JTM.Data;
using JTM.DTO.Account.RegisterUser;
using JTM.DTO.ExceptionResponse;
using JTM.IntegrationTests.Helpers;
using JTM.Services.RabbitService;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace JTM.IntegrationTests.ControllersTests
{
    public class AuthControllerWithoutFakePolicyTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;
        private readonly Mock<IBrokerService> _mockRabbitService;
        private readonly WebApplicationFactory<Program> _factory;

        public AuthControllerWithoutFakePolicyTests(WebApplicationFactory<Program> factory)
        {
            _mockRabbitService = new();
            _factory = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureAppConfiguration((hostingContext, config) =>
                    {
                        config.AddJsonFile("appsettings.Test.json", optional: true, reloadOnChange: true);
                    });
                    builder.ConfigureServices(services =>
                    {
                        var dbContextOptions = services.SingleOrDefault(service =>
                            service.ServiceType == typeof(DbContextOptions<DataContext>));
                        services.Remove(dbContextOptions!);
                        services.AddDbContext<DataContext>(options =>
                            options.UseInMemoryDatabase("Jtm_InMemory"));
                        //services.AddSingleton<IPolicyEvaluator, FakeAdminPolicyEvaluator>();
                        //services.AddMvc(option => option.Filters.Add(new FakeAdminFilter()));
                        //services.AddDbContext<DataContext>(options =>
                        //    options.UseSqlServer(_testConnectionString));
                    });
                });
            _httpClient =  factory.CreateClient();
        }

        [Fact]
        public async Task Register_ForTotallyWrongUser_ReturnForbiddenStatusCode()
        {
            // Arrage
            var dto = new RegisterUserDto("", "", "", "", "");
            var content = dto.ToJsonHttpContent();

            // Act
            var response = await _httpClient.PostAsync("/api/Account/register", content);
            var errorMessage = await response.FromJsonHttpResponseMessage<ExceptionResponse>();

            // Arrange
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
            await SeedUser(new Data.Model.User { Email = tmpEmail, EmailConfirmed = true });

            // Act
            var response = await _httpClient.PostAsync("/api/Account/register", content);
            var errorMessage = await response.FromJsonHttpResponseMessage<ExceptionResponse>();

            // Arrange
            Assert.Equal(System.Net.HttpStatusCode.Forbidden, response.StatusCode);
            Assert.Equal(403, errorMessage!.StatusCode);
            Assert.Equal("ValidationErrors", errorMessage!.Title);
            Assert.NotEmpty(errorMessage!.Errors);
        }

        private async Task SeedUser(Data.Model.User user)
        {
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var dataContext = scope.ServiceProvider.GetService<DataContext>();
            await dataContext.Users.AddAsync(user);
            await dataContext.SaveChangesAsync();
        }

        [Fact]
        public async Task Login_ForValidUser_ReturnOk()
        {
            ////arrange
            //var authResponseDto = new AuthResponseDto()
            //{
            //    Success = true,
            //};
            //_authServiceMock
            //    .Setup(e => e.Login(It.IsAny<UserDto>()))
            //    .ReturnsAsync(authResponseDto);

            //var userDto = new UserDto()
            //{
            //    Email = "test3",
            //    Password = "password",
            //};
            //var httpContent = userDto.ToJsonHttpContent();

            ////act
            //var response = await _httpClient.PostAsync("/api/account/login", httpContent);

            ////assert
            //response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task Login_ForInvalidUser_ReturnBadRequest()
        {
            ////arrange
            //var authResponseDto = new AuthResponseDto() { Success = false, };
            //_authServiceMock
            //   .Setup(e => e.Login(It.IsAny<UserDto>()))
            //   .ReturnsAsync(authResponseDto);

            //var userDto = new UserDto()
            //{
            //    Email = "test",
            //    Password = "password",
            //};
            //var httpContent = userDto.ToJsonHttpContent();

            ////act
            //var response = await _httpClient.PostAsync("/api/account/login", httpContent);

            ////assert
            //response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Register_ForInvalidModel_ReturnBadRequest()
        {
            ////arrange
            //var registerUser = new RegisterUserDto(
            //    userName: "test",
            //    email: "test",
            //    emailConfirmation: "test1",
            //    password: "123",
            //    passwordConfirmation: "123");
            
            ////_authServiceMock
            ////   .Setup(e => e.RegisterUser(It.IsAny<RegisterUserDto>()))
            ////   .ReturnsAsync(new AuthResponseDto() { Success = false, });
            //_mediatorMock
            //    .Setup(e => e.Send(It.IsAny<RegisterUserCommand>(), default))
            //    .ReturnsAsync(It.IsAny<int>());
            //var httpContent = registerUser.ToJsonHttpContent();

            ////act
            //var response = await _httpClient.PostAsync("/api/account/register", httpContent);

            ////assert
            //response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

    

        //[Fact]
        //public async Task ForgetPassword_ForInvalidEmail_ReturnBadRequest()
        //{
        //    //arrange
        //    _authServiceMock
        //        .Setup(e => e.ForgetPassword(It.IsAny<string>()))
        //        .ReturnsAsync(new AuthResponseDto() { Success = false });

        //    //act
        //    var response = await _httpClient.PostAsync("/api/Account/forget-password?email=test%40test.com", null);

        //    //assert 
        //    response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        //}

        //[Fact]
        //public async Task ForgetPassword_ForNoneEmail_ReturnBadRequest()
        //{
        //    //arrange

        //    //act
        //    var response = await _httpClient.PostAsync("/api/Account/forget-password", null);

        //    //assert 
        //    response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        //}

        //[Fact]
        //public async Task ForgetPassword_ForValidData_ReturnOk()
        //{
        //    //arrange
        //    _authServiceMock
        //        .Setup(e => e.ForgetPassword(It.IsAny<string>()))
        //        .ReturnsAsync(new AuthResponseDto() { Success = true });

        //    //act
        //    var response = await _httpClient.PostAsync("/api/Account/forget-password?email=test%40test.com", null);

        //    //assert 
        //    response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        //}

        //[Fact]
        //public async Task RefreshToken_ForNoneCache_ReturnBadRequest()
        //{
        //    //arrange
        //    _authServiceMock
        //        .Setup(e => e.RefreshToken())
        //        .ReturnsAsync(new AuthResponseDto() { Success = false });

        //    //act
        //    var response = await _httpClient.PostAsync("/api/Account/refresh-token", null);

        //    //assert 
        //    response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        //}

        //[Fact]
        //public async Task RefreshToken_ForOkCache_ReturnOk()
        //{
        //    //arrange
        //    _authServiceMock
        //        .Setup(e => e.RefreshToken())
        //        .ReturnsAsync(new AuthResponseDto() { Success = true });

        //    //act
        //    var response = await _httpClient.PostAsync("/api/Account/refresh-token", null);

        //    //assert 
        //    response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        //}

        //[Fact]
        //public async Task ConfirmAccount_ForMissingToken_ReturnBadRequest()
        //{
        //    //arrange

        //    //act
        //    var response = await _httpClient.PostAsync("/api/Account/confirm?userId=1", null);

        //    //assert 
        //    response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        //}

        //[Fact]
        //public async Task ConfirmAccount_ForMissingUserId_ReturnBadRequest()
        //{
        //    //arrange

        //    //act
        //    var response = await _httpClient.PostAsync("/api/Account/confirm?token=1234", null);

        //    //assert 
        //    response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        //}

        //[Fact]
        //public async Task ConfirmAccount_ForInvalidInput_ReturnOk()
        //{
        //    //arrange
        //    _authServiceMock
        //        .Setup(e => e.ConfirmAccount(It.IsAny<int>(),It.IsAny<string>()))
        //        .ReturnsAsync(new AuthResponseDto() { Success = false });

        //    //act
        //    var response = await _httpClient.PostAsync("/api/Account/confirm?token=1234&userId=1", null);

        //    //assert 
        //    response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        //}

        //[Fact]
        //public async Task ConfirmAccount_ForValidInput_ReturnOk()
        //{
        //    //arrange
        //    _authServiceMock
        //        .Setup(e => e.ConfirmAccount(It.IsAny<int>(), It.IsAny<string>()))
        //        .ReturnsAsync(new AuthResponseDto() { Success = true });

        //    //act
        //    var response = await _httpClient.PostAsync("/api/Account/confirm?token=1234&userId=1", null);

        //    //assert 
        //    response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        //}

        //[Fact]
        //public async Task RefreshConfirmToken_ForMissingEmail_ReturnBadRequest()
        //{
        //    //arrange

        //    //act
        //    var response = await _httpClient.PostAsync("/api/Account/confirm-refresh", null);

        //    //assert 
        //    response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        //}

        //[Fact]
        //public async Task RefreshConfirmToken_ForValidEmail_ReturnOk()
        //{
        //    //arrange
        //    _authServiceMock
        //        .Setup(e => e.RefreshActivationToken(It.IsAny<string>()))
        //        .ReturnsAsync(new AuthResponseDto() { Success = true });

        //    //act
        //    var response = await _httpClient.PostAsync("/api/Account/confirm-refresh?email=test%40test.com", null);

        //    //assert 
        //    response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        //}

        //[Fact]
        //public async Task RefreshConfirmToken_ForInvalidInput_ReturnBadRequest()
        //{
        //    //arrange
        //    _authServiceMock
        //        .Setup(e => e.RefreshActivationToken(It.IsAny<string>()))
        //        .ReturnsAsync(new AuthResponseDto() { Success = false });

        //    //act
        //    var response = await _httpClient.PostAsync("/api/Account/confirm-refresh?email=test%40test.com", null);

        //    //assert 
        //    response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        //}

        //[Fact]
        //public async Task ChangePassword_ForInvalidInput_ReturnBadRequest()
        //{
        //    //arrange
        //    _authServiceMock
        //       .Setup(e => e.ChangePassword(It.IsAny<ChangePasswordDto>()))
        //       .ReturnsAsync(new AuthResponseDto() { Success = false, });
        //    var changepassword = new ChangePasswordDto()
        //    {
        //        UserId = 1,
        //        Password = "password",
        //    };
        //    var httpContent = changepassword.ToJsonHttpContent();
        //    //act
        //    var response = await _httpClient.PostAsync("/api/account/change-password", httpContent);

        //    //assert
        //    response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        //}

        //[Fact]
        //public async Task ChangePassword_ForValidInput_ReturnOk()
        //{
        //    //arrange
        //    _authServiceMock
        //       .Setup(e => e.ChangePassword(It.IsAny<ChangePasswordDto>()))
        //       .ReturnsAsync(new AuthResponseDto() { Success = true, });
        //    var changepassword = new ChangePasswordDto()
        //    {
        //        UserId = 1,
        //        Password = "password",
        //    };
        //    var httpContent = changepassword.ToJsonHttpContent();
        //    //act
        //    var response = await _httpClient.PostAsync("/api/account/change-password", httpContent);

        //    //assert
        //    response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        //}
    }
}
