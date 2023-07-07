using FluentValidation;
using JTM.Data;
using JTM.Data.UnitOfWork;
using JTM.DTO.Account.RegisterUser;
using JTM.DTO.Company.AddCompany;
using JTM.DTO.Company.UpdateCompany;
using JTM.DTO.WorkingTime.AddWorkingTime;
using JTM.DTO.WorkingTime.UpdateWorkingTime;
using JTM.Middleware;
using JTM.Services.RabbitService;
using JTM.Services.TokenService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;

namespace JTM
{
    public static class BuilderConfigurationExtension
    {
        public static void ConfigureSwagger(this WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "Standard Auth header - use Bearer scheme: \"Bearer {token}\"",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                options.OperationFilter<SecurityRequirementsOperationFilter>();
            });
        }

        public static void ConfigureMiddlewareServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<ExceptionHandlingMessage>();
        }

        public static void ConfigureMediatr(this WebApplicationBuilder builder)
        {
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
        }

        public static void ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IRabbitService, RabbitService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static void ConfigureDbContext(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));
        }

        public static void ConfigureAuthentication(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = builder.Configuration.GetSection("JWT:Issuer").Value,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8
                            .GetBytes(builder.Configuration.GetSection("JWT:SigningKey").Value)),
                        ValidateAudience = true,
                        ValidAudience = builder.Configuration.GetSection("JWT:Audience").Value
                    };
                });
        }

        public static void ConfigureValidators(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
            builder.Services.AddScoped<IValidator<AddCompanyDto>, AddCompanyDtoValidator>();
            builder.Services.AddScoped<IValidator<UpdateCompanyDto>, UpdateCompanyDtoValidator>();
            builder.Services.AddScoped<IValidator<AddWorkingTimeDto>, AddWorkingTimeDtoValidator>();
            builder.Services.AddScoped<IValidator<UpdateWorkingTimeDto>, UpdateWorkingTimeDtoValidator>();
        }

        public static void UseRegisteredMiddlewares(this WebApplication app)
        {
            app.UseMiddleware<ExceptionHandlingMessage>();
        }
    }
}
