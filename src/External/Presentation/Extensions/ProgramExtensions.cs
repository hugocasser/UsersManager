using System.Runtime.InteropServices;
using Application;
using Infrastructure;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.OpenApi.Models;
using Presentation.Configurations;

namespace Presentation.Extensions;

public static class ProgramExtensions
{
    public static WebApplicationBuilder ConfigureBuilder(this WebApplicationBuilder builder)
    {
        var applicationConfiguration = new ApplicationConfiguration(builder.Configuration);
        var tokenGenerationConfiguration = new TokenGenerationConfiguration(builder.Configuration);

        builder.Services
            .AddDbContext(applicationConfiguration)
            .AddRepositories()
            .AddRedisCaching(applicationConfiguration)
            .AddApplication()
            .AddUsersIdentity()
            .AddServices(
                new EmailServicesConfiguration(builder.Configuration),
                tokenGenerationConfiguration)
            .AddJwtAuthentication(tokenGenerationConfiguration)
            .AddSwagger()
            .AddCors(options => options.ConfigureAllowAllCors())
            .AddEndpointsApiExplorer()
            .AddControllers();
        
        
        builder.AddLoggingServices(applicationConfiguration);
        
        return builder;
    }
    
    public static WebApplication ConfigureApplication(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/error-development");
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        else
        {
            app.UseExceptionHandler("/error");
        }

        app.UseLoggingDependOnEnvironment();
        
        // if we will be deploying in kubernetes, then https and ssl certificates
        // will be managing by kubernetes it self
        if (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "false")
        {
            app.UseHttpsRedirection();
        }
        
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseCors("AllowAll");
        app.MapControllers();

        return app;
    }

    public static async Task<WebApplication> RunApplicationAsync(this WebApplication webApplication)
    {
        using var scope = webApplication.Services.CreateScope();
        var serviceProvider = scope.ServiceProvider;
        
        try
        {
            var usersDbContext = serviceProvider.GetRequiredService<UsersDbContext>();
            await usersDbContext.Database.EnsureCreatedAsync();
            
            await webApplication.RunAsync();
        }
        catch (Exception ex)
        {
            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "Host terminated unexpectedly");
        }

        return webApplication;
    }
    
    private static CorsOptions ConfigureAllowAllCors(this CorsOptions options)
    {
        options.AddPolicy("AllowAll", policy =>
        {
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
            policy.AllowAnyOrigin();
        });
        
        return options;
    }

    private static IServiceCollection AddSwagger(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme()
            {
                In = ParameterLocation.Header,
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });
        });

        return serviceCollection;
    }
    
}