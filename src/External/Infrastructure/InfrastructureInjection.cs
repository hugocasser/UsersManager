using System.Reflection;
using Application.Abstraction.Repositories;
using Infrastructure.Configurations;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Services.EmailServices;
using Infrastructure.Services.TokenServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class InfrastructureInjection
{
    public static IServiceCollection AddDbContext(
        this IServiceCollection serviceCollection, 
        IApplicationConfiguration configuration)
    {
        serviceCollection.AddPostgresDatabase(configuration.DatabaseConnectionString);
        return serviceCollection;
    }
    

    private static IServiceCollection AddPostgresDatabase(
        this IServiceCollection serviceCollection, 
        string? connectionString)
    {
        serviceCollection.AddDbContext<UsersDbContext>(options =>
        {
            options.EnableDetailedErrors();
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            options.UseNpgsql(
                connectionString,
                builder =>
                {
                    builder.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
                    builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                });
        });

        return serviceCollection;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IUsersRepository, UsersRepository>();
        serviceCollection.AddScoped<IRolesRepository, RolesRepository>();
        serviceCollection.AddScoped<IRefreshTokensRepository, RefreshTokensRepository>();
        
        return serviceCollection;
    }
    
    public static IServiceCollection AddServices(
        this IServiceCollection serviceCollection,
        IEmailServicesConfiguration emailServicesConfiguration, 
        ITokenGenerationConfiguration tokenGenerationConfiguration)
    {
        serviceCollection.UseEmailServices(emailServicesConfiguration);
        serviceCollection.UseTokenServices(tokenGenerationConfiguration);
        
        return serviceCollection;
    }
    
    public static IServiceCollection AddRedisCaching(
        this IServiceCollection serviceCollection,
        IApplicationConfiguration configuration)
    {
        serviceCollection.Decorate<IRefreshTokensRepository, CachedRefreshTokensRepository>();
        configuration.InjectCacheService(serviceCollection);
        
        return serviceCollection;
    }
}