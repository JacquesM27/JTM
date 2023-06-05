using FluentValidation;
using JTM.CQRS.Command.Account;
using JTM.Data;
using JTM.DTO.Account.RegisterUser;
using JTM.Enum;
using JTM.Model;
using JTM.Services.RabbitService;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace JTM.IntegrationTests.CQRS.Command.Account
{
    public class RegisterCommandTests : IClassFixture<WebApplicationFactory<Program>>
    {
        //private readonly string _testConnectionString;
        private readonly WebApplicationFactory<Program> _applicationFactory;
        private readonly DbContextOptions<DataContext> _contextOptions;
        private readonly Mock<IRabbitService> _mockRabbitService = new();
        private readonly DataContext _dataContext;
        //private readonly ITokenService _authService;
        //private readonly IHttpContextAccessor _contextAccessor;

        public RegisterCommandTests(WebApplicationFactory<Program> applicationFactory)
        {
            _applicationFactory = applicationFactory;
            _contextOptions = new DbContextOptionsBuilder<DataContext>()
               .UseInMemoryDatabase(databaseName: "InMemory_JTM")
               .Options;
            _dataContext = new DataContext(_contextOptions);
            //IConfiguration configuration = new ConfigurationBuilder()
            //   .AddJsonFile("appsettings.json")
            //   .Build();
            //_testConnectionString = configuration.GetConnectionString("TestConnectionString");
            //_applicationFactory = applicationFactory
            //    .WithWebHostBuilder(builder =>
            //    {
            //        builder.ConfigureServices(services =>
            //        {
            //            //ef
            //            var dbContextOptions = services.SingleOrDefault(service =>
            //                service.ServiceType == typeof(DbContextOptions<DataContext>));
            //            services.Remove(dbContextOptions);
            //            services.AddDbContext<DataContext>(options =>
            //                options.UseSqlServer(_testConnectionString));

            //            //dapper
            //            //var IDbConnection = services.Where(s => s.ServiceType == typeof(IDapperConnectionFactory)).ToList();
            //            //foreach (var item in IDbConnection)
            //            //{
            //            //    services.Remove(item);
            //            //}
            //            //services.AddScoped<IDapperConnectionFactory, TestDapperConnectionFactory>();
            //        });
            //    });
            //var servicescope = _applicationFactory.Services.CreateScope();
            //_authService = servicescope.ServiceProvider.GetService<ITokenService>();
            //_dataContext = servicescope.ServiceProvider.GetService<DataContext>();
            //_contextAccessor = servicescope.ServiceProvider.GetService<IHttpContextAccessor>();
            //CleanTestDatabaseToTest.CleanDb(_testConnectionString);
        }

        [Fact]
        public async Task Register_ForValidData_ShouldCreateUserAsync()
        {
            // Arrange
            string tmpEmail = "test@test.com";
            _mockRabbitService.Setup(c => c.SendMessage(It.IsAny<MessageQueueType>(), It.IsAny<MessageDto>()));
            var command = new RegisterUserCommand(
                    userName: "test",
                    email: tmpEmail,
                    password: "123");
            var commandHandler = new RegisterUserCommandHandler(_dataContext, _mockRabbitService.Object);

            // Act
            await commandHandler.Handle(command, default);

            // Assert
            Assert.True(_dataContext.Users.Any(u => u.Email == tmpEmail));
        }

        [Fact]
        public async Task Register_ForBusyEmail_ShouldThrowsAuthExceptionWithEmailBusyMessageAsync()
        {
            // Arrange
            string tmpEmail = "test@test.com";
            _mockRabbitService.Setup(c => c.SendMessage(It.IsAny<MessageQueueType>(), It.IsAny<MessageDto>()));
            var command = new RegisterUserCommand(
                    userName: "test",
                    email: tmpEmail,
                    password: "123");
            var commandHandler = new RegisterUserCommandHandler(_dataContext, _mockRabbitService.Object);

            // Act
            await _dataContext.Users.AddAsync(new User() { Email = tmpEmail });
            await _dataContext.SaveChangesAsync();
            async Task HandleCommand() => await commandHandler.Handle(command, default);

            // Assert
            var exception = await Assert.ThrowsAnyAsync<ValidationException>(HandleCommand);
            Assert.Equal("Email address is busy.", exception.Message);
            //await Assert.ThrowsAnyAsync<ValidationException>(async () => await commandHandler.Handle(command, default));
        }

        //[Fact]
        //public async Task Login_ForNotFoundUser_ShouldReturnFalse()
        //{
        //    // Arrange
        //    var userDto = new UserDto
        //    {
        //        Email = "123",
        //        Password = "123"
        //    };

        //    //Act 
        //    var result = await _authService.Login(userDto);

        //    //Assert
        //    Assert.False(result.Success);
        //    Assert.True(result.Message.Equals("User not found."));
        //}

        //[Fact]
        //public async Task Login_ForWrongPassword_ShouldReturnFalse()
        //{
        //    //Arrange
        //    string password = "123";
        //    var user = await AddFakeConfirmedUser(password);
        //    var userDto = new UserDto
        //    {
        //        Email = user.Email,
        //        Password = "999"
        //    };

        //    //Act
        //    var result = await _authService.Login(userDto);

        //    //Assert
        //    Assert.False(result.Success);
        //    Assert.True(result.Message.Equals("Wrong password."));
        //}

        //[Fact]
        //public async Task Login_ForNotActivatedUser_ShouldReturnFalse()
        //{
        //    //Arrange
        //    string password = "123";
        //    var user = await AddFakeUnconfirmedUser(password);
        //    var userDto = new UserDto
        //    {
        //        Email = user.Email,
        //        Password = password
        //    };

        //    //Act
        //    var result = await _authService.Login(userDto);

        //    //Assert
        //    Assert.False(result.Success);
        //    Assert.True(result.Message.Equals("Account not activated."));
        //}

        //[Fact]
        //public async Task Login_ForCorrectCredentials_ShouldReturnTrue()
        //{
        //    //Arrange
        //    string password = "123";
        //    var user = await AddFakeConfirmedUser(password);
        //    var userDto = new UserDto
        //    {
        //        Email = user.Email,
        //        Password = password
        //    };

        //    //Act
        //    var result = await _authService.Login(userDto);

        //    //Assert
        //    Assert.True(result.Success);
        //    Assert.True(result.Message.Equals(string.Empty));
        //}

        //[Fact]
        //public async Task ChangePassword_ForNotFoundUser_ShouldReturnFalse()
        //{
        //    //Arrange
        //    var changePassDto = new ChangePasswordDto
        //    {
        //        Password= "123",
        //        Token = "123",
        //        UserId = -1
        //    };

        //    //Act 
        //    var result = await _authService.ChangePassword(changePassDto);

        //    //Assert
        //    Assert.False(result.Success);
        //    Assert.True(result.Message.Equals("Invalid user."));
        //}

        //[Fact]
        //public async Task ChangePassword_ForExpiredToken_ShouldReturnFalse()
        //{
        //    //Arrange
        //    string password = "123";
        //    var user = await AddFakeConfirmedUser(password);
        //    await ExpireUserResetPasswordToken(user);
        //    var changePassDto = new ChangePasswordDto
        //    {
        //        Password = password,
        //        Token = user.PasswordResetToken,
        //        UserId = user.Id
        //    };

        //    //Act 
        //    var result = await _authService.ChangePassword(changePassDto);

        //    //Assert
        //    Assert.False(result.Success);
        //    Assert.True(result.Message.Equals("Token expires."));
        //}

        //[Fact]
        //public async Task ChangePassword_ForInvalidToken_ShouldReturnFalse()
        //{
        //    //Arrange
        //    string password = "123";
        //    var user = await AddFakeConfirmedUser(password);
        //    var changePassDto = new ChangePasswordDto
        //    {
        //        Password = password,
        //        Token = "this is wrong token",
        //        UserId = user.Id
        //    };

        //    //Act 
        //    var result = await _authService.ChangePassword(changePassDto);

        //    //Assert
        //    Assert.False(result.Success);
        //    Assert.True(result.Message.Equals("Invalid token."));
        //}

        //[Fact]
        //public async Task ChangePassword_ForCorrectData_ShouldReturnTrue()
        //{
        //    //Arrange
        //    string password = "123";
        //    var user = await AddFakeConfirmedUser(password);
        //    var changePassDto = new ChangePasswordDto
        //    {
        //        Password = password,
        //        Token = user.PasswordResetToken,
        //        UserId = user.Id
        //    };

        //    //Act 
        //    var result = await _authService.ChangePassword(changePassDto);

        //    //Assert
        //    Assert.True(result.Success);
        //    Assert.True(result.Message.Equals(string.Empty));
        //}

        //[Fact]
        //public async Task ConfirmAccount_ForInvalidUser_ShouldReturnFalse()
        //{
        //    //Arrange
        //    int userId = 0;
        //    string token = string.Empty;

        //    //Act 
        //    var result = await _authService.ConfirmAccount(userId, token);

        //    //Assert
        //    Assert.False(result.Success);
        //    Assert.True(result.Message.Equals("Invalid user."));
        //}

        //[Fact]
        //public async Task ConfirmAccount_ForConfirmedUser_ShouldReturnFalse()
        //{
        //    //Arrange
        //    var user = await AddFakeConfirmedUser("123");

        //    //Act
        //    var result = await _authService.ConfirmAccount(user.Id, user.ActivationToken);

        //    //Assert
        //    Assert.False(result.Success);
        //    Assert.True(result.Message.Equals("User already confirmed."));
        //}

        //[Fact]
        //public async Task ConfirmAccount_ForExpiredToken_ShouldReturnFalse()
        //{
        //    //Arrange
        //    var user = await AddFakeUnconfirmedUser("123");
        //    await ExpireUserActivationToken(user);

        //    //Act
        //    var result = await _authService.ConfirmAccount(user.Id, user.ActivationToken);

        //    //Assert
        //    Assert.False(result.Success);
        //    Assert.True(result.Message.Equals("Token expired."));
        //}

        //[Fact]
        //public async Task ConfirmAccount_ForInvalidToken_ShouldReturnFalse()
        //{
        //    //Arrange
        //    var user = await AddFakeUnconfirmedUser("123");

        //    //Act
        //    var result = await _authService.ConfirmAccount(user.Id, string.Empty);

        //    //Assert
        //    Assert.False(result.Success);
        //    Assert.True(result.Message.Equals("Invalid token."));
        //}

        //[Fact]
        //public async Task ConfirmAccount_ForValidData_ShouldReturnTrue()
        //{
        //    //Arrange
        //    var user = await AddFakeUnconfirmedUser("123");

        //    //Act
        //    var result = await _authService.ConfirmAccount(user.Id, user.ActivationToken);

        //    //Assert
        //    Assert.True(result.Success);
        //    Assert.True(result.Message.Equals(string.Empty));
        //}

        //[Fact]
        //public async Task RefreshToken_ForInvalidUser_ShouldReturnFalse()
        //{
        //    //Arrange
        //    ForwardRefreshTokenAsCookie("666");

        //    //Act
        //    var result = await _authService.RefreshToken();

        //    //Assert
        //    Assert.False(result.Success);
        //    Assert.True(result.Message.Equals("Invalid user."));
        //}

        //[Fact]
        //public async Task RefreshToken_ForExpiredToken_ShouldReturnFalse()
        //{
        //    //Arrange
        //    var user = await AddFakeConfirmedUser("123");
        //    await SetExpiredDummyRefreshToken(user);
        //    ForwardRefreshTokenAsCookie(user.RefreshToken);

        //    //Act
        //    var result = await _authService.RefreshToken();

        //    //Assert
        //    Assert.False(result.Success);
        //    Assert.True(result.Message.Equals("Token expired."));
        //}

        //[Fact]
        //public async Task RefreshToken_ForValidToken_ShouldReturnTrue()
        //{
        //    //Arrange
        //    var user = await AddFakeConfirmedUser("123");
        //    await SetActiveDummyRefreshToken(user);
        //    ForwardRefreshTokenAsCookie(user.RefreshToken);

        //    //Act
        //    var result = await _authService.RefreshToken();

        //    //Assert
        //    Assert.True(result.Success);
        //    Assert.True(result.Message.Equals(string.Empty));
        //}


        //[Fact]
        //public async Task RefreshActivationToken_ForInvalidUser_ShouldReturnFalse()
        //{
        //    //Arrange
        //    //Act 
        //    var result = await _authService.RefreshActivationToken(string.Empty);

        //    //Assert
        //    Assert.False(result.Success);
        //    Assert.True(result.Message.Equals("Invalid user."));
        //}

        //[Fact]
        //public async Task RefreshActivationToken_ForConfirmedUser_ShouldReturnFalse()
        //{
        //    //Arrange
        //    var user = await AddFakeConfirmedUser("123");

        //    //Act 
        //    var result = await _authService.RefreshActivationToken(user.Email);

        //    //Assert
        //    Assert.False(result.Success);
        //    Assert.True(result.Message.Equals("User already confirmed."));
        //}

        //[Fact]
        //public async Task RefreshActivationToken_ForUnonfirmedUser_ShouldReturnTrue()
        //{
        //    //Arrange
        //    var user = await AddFakeUnconfirmedUser("123");

        //    //Act 
        //    var result = await _authService.RefreshActivationToken(user.Email);

        //    //Assert
        //    Assert.True(result.Success);
        //    Assert.True(result.Message.Equals(string.Empty));
        //}

        //[Fact]
        //public async Task ForgetPassword_ForInvalidUser_ShouldReturnFalse()
        //{
        //    //Arrange
        //    //Act
        //    var result = await _authService.ForgetPassword(string.Empty);

        //    //Assert
        //    Assert.False(result.Success);
        //    Assert.True(result.Message.Equals("Invalid user."));
        //}

        //[Fact]
        //public async Task ForgetPassword_ForValidUser_ShouldReturnTrue()
        //{
        //    //Arrange
        //    var user = await AddFakeConfirmedUser("123");

        //    //Act
        //    var result = await _authService.ForgetPassword(user.Email);

        //    //Assert
        //    Assert.True(result.Success);
        //    Assert.True(result.Message.Equals(string.Empty));
        //}


        //private async Task<User> AddFakeConfirmedUser(string password)
        //{
        //    PasswordHelper.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
        //    User user = new()
        //    {
        //        Username = "test",
        //        Email = "test@test.com",
        //        PasswordHash = passwordHash,
        //        PasswordSalt = passwordSalt,
        //        EmailConfirmed = true,
        //        PasswordTokenExpires = DateTime.UtcNow.AddDays(1),
        //        PasswordResetToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64))
        //    };
        //    _dataContext.Add(user);
        //    await _dataContext.SaveChangesAsync();
        //    return user;
        //}

        //private async Task<User> AddFakeUnconfirmedUser(string password)
        //{
        //    PasswordHelper.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
        //    User user = new()
        //    {
        //        Username = "test",
        //        Email = "test@test.com",
        //        PasswordHash = passwordHash,
        //        PasswordSalt = passwordSalt,
        //        EmailConfirmed = false,
        //        PasswordTokenExpires = DateTime.UtcNow.AddDays(1),
        //        PasswordResetToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
        //        ActivationToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
        //        ActivationTokenExpires = DateTime.UtcNow.AddDays(1)
        //    };
        //    _dataContext.Add(user);
        //    await _dataContext.SaveChangesAsync();
        //    return user;
        //}

        //private async Task ExpireUserResetPasswordToken(User user)
        //{
        //    user.PasswordTokenExpires = DateTime.UtcNow.AddDays(-1);
        //    await _dataContext.SaveChangesAsync();
        //}

        //private async Task ExpireUserActivationToken(User user)
        //{
        //    user.ActivationTokenExpires = DateTime.UtcNow.AddDays(-1);
        //    await _dataContext.SaveChangesAsync();
        //}

        //private async Task SetActiveDummyRefreshToken(User user)
        //{
        //    user.RefreshToken = "123";
        //    user.TokenExpires= DateTime.UtcNow.AddDays(1);
        //    await _dataContext.SaveChangesAsync();
        //}

        //private async Task SetExpiredDummyRefreshToken(User user)
        //{
        //    user.RefreshToken = "123";
        //    user.TokenExpires = DateTime.UtcNow.AddDays(-1);
        //    await _dataContext.SaveChangesAsync();
        //}

        //private void ForwardRefreshTokenAsCookie(string token)
        //{
        //    var context = new DefaultHttpContext();
        //    context.Request.Cookies = HttpCookieHelper.MockRequestCookieCollection("refreshToken", token);
        //    var accessor = new HttpContextAccessor { HttpContext = context };
        //    _contextAccessor.HttpContext = accessor.HttpContext;
        //}
    }
}
