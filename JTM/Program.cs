using JTM;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.ConfigureSwagger();
builder.ConfigureMiddlewareServices();
builder.ConfigureMediatr();
builder.ConfigureMediatr();
builder.Services.AddHttpContextAccessor();
builder.ConfigureDbContext();
builder.ConfigureAuthentication();
builder.ConfigureValidators();

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
app.UseRegisteredMiddlewares();
app.Run();

public partial class Program { }