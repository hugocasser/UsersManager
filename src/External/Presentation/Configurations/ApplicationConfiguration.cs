﻿using Application.Abstraction.Services;
using Infrastructure.Configurations;
using Infrastructure.Services;
using StackExchange.Redis;

namespace Presentation.Configurations;

public class ApplicationConfiguration : IApplicationConfiguration
{
    public string DatabaseConnectionString { get; private set; } = string.Empty;
    public bool IsDevelopmentEnvironment { get; private set; }
    public string CacheServiceConnectionString { get; set; } = string.Empty;
    public IApplicationConfiguration.Ports OpenPorts { get; }
    
    public ApplicationConfiguration(IConfiguration configuration)
    {
        IsDevelopmentEnvironment = string.Equals(
            Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"), 
            "development", 
            StringComparison.InvariantCultureIgnoreCase);

        _ = SetupDbConnectionString(configuration) || FallBackToDevelopmentEnvironment();

        OpenPorts = new IApplicationConfiguration.Ports()
        {
            HttpsPort = 3001,
            HttpPort = 3000,
        };
    }

    public void InjectCacheService(IServiceCollection serviceCollection)
    {
        if (IsDevelopmentEnvironment)
        {
            serviceCollection.AddMemoryCache();
            serviceCollection.AddSingleton<ICacheService, InMemoryCacheService>();
            return;
        }

        serviceCollection.AddSingleton<IConnectionMultiplexer, ConnectionMultiplexer>(provider => 
            provider.GetService<ConnectionMultiplexer>() 
            ?? ConnectionMultiplexer.Connect(CacheServiceConnectionString));
        
        serviceCollection.AddSingleton<ICacheService, RedisCacheService>();
    }
    
    private bool SetupDbConnectionString(IConfiguration configuration)
    {
        var dbConnectionString = configuration.GetConnectionString("IdentityDbContext");
        var cacheConnectionString = configuration.GetConnectionString("CacheDbContext");

        if (string.IsNullOrEmpty(dbConnectionString) || string.IsNullOrEmpty(cacheConnectionString))
        {
            return false;
        }

        DatabaseConnectionString =  dbConnectionString;
        CacheServiceConnectionString = cacheConnectionString;
        
        return true;
    }

    /// <summary>
    /// Force setup the Development environment if unable to setup proper environment.
    /// This means what the application will be using self deploy sqlite database.
    /// </summary>
    private bool FallBackToDevelopmentEnvironment()
    {
        Console.WriteLine("ERROR: UNABLE TO INITIALIZE ANY OF ENVIRONMENTS!");
        Console.WriteLine("FALLBACK TO FORCE DEVELOPMENT!");
        
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
        IsDevelopmentEnvironment = true;
        DatabaseConnectionString = "Data Source=IdentityContext.db";
        
        return true;
    }
}