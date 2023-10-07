using Infrastructure.Exeptions;
using Infrastructure.Services.TokenServices;

namespace Presentation.Configurations;

public class TokenGenerationConfiguration : ITokenGenerationConfiguration
{
    public string Issuer { get; }
    public string Audience { get; }
    public string Key { get; }

    public TokenGenerationConfiguration(IConfiguration configuration)
    {
        if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("TEST_ConnectionString")))
        {
            Issuer = "identity";
            Audience = "identity";
            Key = "865D92FD-B1C8-41A4-850F-409792C9B740";
            
            return;
        }
        
        Issuer = configuration["Jwt:Issuer"] ?? throw new UserSecretsInvalidException("setup-jwt-issuer-secret");
        Audience = configuration["Jwt:Audience"] ?? throw new UserSecretsInvalidException("setup-jwt-audience-secret");
        Key = configuration["Jwt:Key"] ?? throw new UserSecretsInvalidException("setup-jwt-key-secrets");
    }
}