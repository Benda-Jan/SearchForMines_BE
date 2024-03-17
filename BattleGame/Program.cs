using Npgsql;
using BattleGame.Data;
using Microsoft.EntityFrameworkCore;
using BattleGame.Services;
using BattleGame.Authorization;
using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using NodaTime;
using NodaTime.Testing;
using Microsoft.AspNetCore.Authentication;

namespace BattleGame;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var connectionString = builder.Configuration["PostgreSql:ConnectionString"];
        var dbPassword = builder.Configuration["PostgreSql:DbPassword"];
        var connectionStringBuilder = new NpgsqlConnectionStringBuilder(connectionString) { Password = dbPassword };


        // Add services to the container.

        builder.Services.AddDbContext<ApplicationContext>(options
            => options
                .UseNpgsql(connectionStringBuilder.ConnectionString));

        builder.Services.AddScoped<GameService>();
        builder.Services.AddScoped<GameFieldService>();
        builder.Services.AddSingleton<IClock>(NodaTime.SystemClock.Instance);

        builder.Services.AddAuthentication()
            .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("Basic", null);

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "APIs", Version = "v1" });
            c.AddSecurityDefinition("Basic", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Basic",
                In = ParameterLocation.Header,
                Description = "Basic Authorization header"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Basic"
                        },
                        Name = "Basic",
                        In = ParameterLocation.Header,

                    },
                    new List<string>()
                }
            });
        });
        //builder.Services.AddSwaggerExamplesFromAssemblyOF<Program>();

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
            //db.Database.Migrate();
        }

        // Configure the HTTP request pipeline.
        app.UseDeveloperExceptionPage();
        app.UseSwagger(c => { c.RouteTemplate = "/swagger/{documentName}/swagger.json"; });
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1"));

        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}

