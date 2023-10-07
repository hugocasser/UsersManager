using System.Text;
using Domain.Model;
using Infrastructure.Persistence;
using Infrastructure.Services.TokenServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Presentation.Extensions;

public static class IdentityAuthenticationProgramExtension
{
    public static IServiceCollection AddIdentity(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+ абвгдеёжзийклмнопрстуфхцчшщъыьэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
                options.SignIn.RequireConfirmedEmail = true;
            })
            .AddEntityFrameworkStores<UsersDbContext>()
            .AddDefaultTokenProviders()
            .AddRoles<IdentityRole>();

        return serviceCollection;
    }

    public static IServiceCollection AddJwtAuthentication(
        this IServiceCollection serviceCollection,
        ITokenGenerationConfiguration tokenGenerationConfiguration)
    {
        serviceCollection.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options => { options.SetValidationTokenOptions(tokenGenerationConfiguration); });

        return serviceCollection;
    }

    private static JwtBearerOptions SetValidationTokenOptions(this JwtBearerOptions options,
        ITokenGenerationConfiguration tokenGenerationConfiguration)
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateActor = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            RequireExpirationTime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = tokenGenerationConfiguration.Issuer,
            ValidAudience = tokenGenerationConfiguration.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenGenerationConfiguration.Key))
        };

        return options;
    }
}