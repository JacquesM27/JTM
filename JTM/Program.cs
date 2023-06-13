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

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
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

builder.Services.AddTransient<ExceptionHandlingMessage>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

builder.Services.AddSingleton<IRabbitService, RabbitService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = true,
            ValidIssuer = "secret key",
            ValidateAudience = true,
            ValidAudience = "secret key2"
        };
    });

builder.Services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
builder.Services.AddScoped<IValidator<AddCompanyDto>, AddCompanyDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateCompanyDto>, UpdateCompanyDtoValidator>();
builder.Services.AddScoped<IValidator<AddWorkingTimeDto>, AddWorkingTimeDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateWorkingTimeDto>, UpdateWorkingTimeDtoValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<ExceptionHandlingMessage>();

app.Run();

public partial class Program { }