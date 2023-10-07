using System.Reflection;
using Domain.Model;
using Infrastructure.Persistence.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Persistence;

public class UsersDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    private readonly IConfiguration _configuration;

    public UsersDbContext(
        DbContextOptions options,
        IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // im getting credentials from UserSecrets, i need manually pass IConfiguration
        modelBuilder.ApplyConfiguration(new AdminSeederConfiguration(_configuration));
    }
}