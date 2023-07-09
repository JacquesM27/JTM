using JTM.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;

namespace JTM.IntegrationTests.Helpers
{
    class TestWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureHostConfiguration(config =>
            {
                config.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.Test.json");
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

            return base.CreateHost(builder);
        }
    }
}
