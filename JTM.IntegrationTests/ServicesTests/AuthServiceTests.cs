using FluentAssertions;
using JTM.Data;
using JTM.Data.DapperConnection;
using JTM.DTO;
using JTM.Helper.PasswordHelper;
using JTM.IntegrationTests.Helpers;
using JTM.Model;
using JTM.Services.AuthService;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Security.Cryptography;
using Xunit;

namespace JTM.IntegrationTests.ServicesTests
{
    public class AuthServiceTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly string _testConnectionString;
        private readonly WebApplicationFactory<Program> _applicationFactory;
        private readonly DataContext _dataContext;
        private readonly IAuthService _authService;

        public AuthServiceTests(WebApplicationFactory<Program> applicationFactory)
        {
            IConfiguration configuration = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
               .Build();
            _testConnectionString = configuration.GetConnectionString("TestConnectionString");
            _applicationFactory = applicationFactory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        //ef
                        var dbContextOptions = services.SingleOrDefault(service =>
                            service.ServiceType == typeof(DbContextOptions<DataContext>));
                        services.Remove(dbContextOptions);
                        services.AddDbContext<DataContext>(options =>
                            options.UseSqlServer(_testConnectionString));

                        //dapper
                        var IDbConnection = services.Where(s => s.ServiceType == typeof(IDapperConnectionFactory)).ToList();
                        foreach (var item in IDbConnection)
                        {
                            services.Remove(item);
                        }
                        services.AddScoped<IDapperConnectionFactory, TestDapperConnectionFactory>();
                    });
                });
            var servicescope = _applicationFactory.Services.CreateScope();
            _authService = servicescope.ServiceProvider.GetService<IAuthService>();
            _dataContext = servicescope.ServiceProvider.GetService<DataContext>();
        }

        [Fact]
        public async Task Login_ForNotFoundUser_ShouldReturnFalse()
        {
            // Arrange
            var userDto = new UserDto
            {
                Email = "123",
                Password = "123"
            };

            //Act 
            var result = await _authService.Login(userDto);

            //Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async Task Login_ForWrongPassword_ShouldReturnFalse()
        {
            //Arrange
            string password = "123";
            var user = await AddFakeConfirmedUser(password);
            var userDto = new UserDto
            {
                Email = user.Email,
                Password = "999"
            };

            //Act
            var result = await _authService.Login(userDto);

            //Assert
            Assert.False(result.Success);
            await CleanTestDatabaseToTest.CleanDb(_testConnectionString);
        }

        [Fact]
        public async Task Login_ForNotActivatedUser_ShouldReturnFalse()
        {
            //Arrange
            string password = "123";
            var user = await AddFakeUnconfirmedUser(password);
            var userDto = new UserDto
            {
                Email = user.Email,
                Password = password
            };

            //Act
            var result = await _authService.Login(userDto);

            //Assert
            Assert.False(result.Success);
            await CleanTestDatabaseToTest.CleanDb(_testConnectionString);
        }

        [Fact]
        public async Task Login_ForCorrectCredentials_ShouldReturnTrue()
        {
            //Arrange
            string password = "123";
            var user = await AddFakeConfirmedUser(password);
            var userDto = new UserDto
            {
                Email = user.Email,
                Password = password
            };

            //Act
            var result = await _authService.Login(userDto);

            //Assert
            Assert.True(result.Success);
            await CleanTestDatabaseToTest.CleanDb(_testConnectionString);
        }

        [Fact]
        public async Task ChangePassword_ForNotFoundUser_ShouldReturnFalse()
        {
            //Arrange
            var changePassDto = new ChangePasswordDto
            {
                Password= "123",
                Token = "123",
                UserId = -1
            };

            //Act 
            var result = await _authService.ChangePassword(changePassDto);

            //Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async Task ChangePassword_ForExpiredToken_ShouldReturnFalse()
        {
            //Arrange
            string password = "123";
            var user = await AddFakeConfirmedUser(password);
            await ExpireUserToken(user);
            var changePassDto = new ChangePasswordDto
            {
                Password = password,
                Token = user.PasswordResetToken,
                UserId = user.Id
            };

            //Act 
            var result = await _authService.ChangePassword(changePassDto);

            //Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async Task ChangePassword_ForInvalidToken_ShouldReturnFalse()
        {
            //Arrange
            string password = "123";
            var user = await AddFakeConfirmedUser(password);
            var changePassDto = new ChangePasswordDto
            {
                Password = password,
                Token = "this is wrong token",
                UserId = user.Id
            };

            //Act 
            var result = await _authService.ChangePassword(changePassDto);

            //Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async Task ChangePassword_ForCorrectData_ShouldReturnTrue()
        {
            //Arrange
            string password = "123";
            var user = await AddFakeConfirmedUser(password);
            var changePassDto = new ChangePasswordDto
            {
                Password = password,
                Token = user.PasswordResetToken,
                UserId = user.Id
            };

            //Act 
            var result = await _authService.ChangePassword(changePassDto);

            //Assert
            Assert.True(result.Success);
        }


        private async Task<User> AddFakeConfirmedUser(string password)
        {
            PasswordHelper.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            User user = new()
            {
                Username = "test",
                Email = "test@test.com",
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                EmailConfirmed = true,
                PasswordTokenExpires = DateTime.UtcNow.AddDays(1),
                PasswordResetToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64))
            };
            _dataContext.Add(user);
            await _dataContext.SaveChangesAsync();
            return user;
        }

        private async Task<User> AddFakeUnconfirmedUser(string password)
        {
            PasswordHelper.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            User user = new()
            {
                Username = "test",
                Email = "test@test.com",
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                EmailConfirmed = false,
                PasswordTokenExpires = DateTime.UtcNow.AddDays(1),
                PasswordResetToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64))
            };
            _dataContext.Add(user);
            await _dataContext.SaveChangesAsync();
            return user;
        }

        private async Task ExpireUserToken(User user)
        {
            user.PasswordTokenExpires = DateTime.UtcNow.AddDays(-1);
            await _dataContext.SaveChangesAsync();
        }

    }
}
