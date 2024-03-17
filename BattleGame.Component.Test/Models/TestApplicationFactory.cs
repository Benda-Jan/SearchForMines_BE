
using BattleGame.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace BattleGame.Component.Test.Models;

public class TestApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");

        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationContext>));
            if (descriptor is not null)
            {

                services.Remove(descriptor);
            }

            services.AddDbContext<ApplicationContext>(options => options.UseInMemoryDatabase("TestingDatabaseInMemory"));
            services.AddSingleton<ILoggerFactory, NullLoggerFactory>();

            var sp = services.BuildServiceProvider();
            TestDataSeeder.SeedData(sp);
        });
    }
}

