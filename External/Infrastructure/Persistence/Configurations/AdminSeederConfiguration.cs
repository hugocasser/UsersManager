using Domain.Model;
using Infrastructure.Exeptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Persistence.Configurations;

public class AdminSeederConfiguration : IEntityTypeConfiguration<User>
{
    private readonly IConfiguration _configuration;

    public AdminSeederConfiguration(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(EntityTypeBuilder<User> builder)
    {
        Guid id;
        string email, password;

        if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("TEST_ConnectionString")))
        {
            id = Guid.Parse("C0EC4C8E-4941-45F5-B06B-3657735560E5");
            email = "identity.dev@gmail.com";
            password = "Adm1n.dev-31_13%";
        }
        else
        {
            id = Guid.Empty;
            email = _configuration["Admin:Email"] ?? throw new UserSecretsInvalidException("setup-admin-email-secret");
            password = _configuration["Admin:Password"] ??
                       throw new UserSecretsInvalidException("setup-admin-password-secret");
        }

        var user = new User
        {
            Id = id,
            UserName = "FirstAdmin",
            Email = email,
            NormalizedEmail = email.ToUpper(),
            NormalizedUserName = "FIRST ADMIN",
            EmailConfirmed = true,
            FirstName = "First Admin",
            SecurityStamp = Guid.NewGuid().ToString("D"),
            PhoneNumber = "+000000000000"
        };

        var passwordHasher = new PasswordHasher<User>();
        user.PasswordHash = passwordHasher.HashPassword(user, password);
        builder.HasData(user);
    }
}