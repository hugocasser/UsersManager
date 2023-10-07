using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class RolesSeederConfiguration : IEntityTypeConfiguration<IdentityRole<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityRole<Guid>> builder)
    {
        builder.HasData(
            new IdentityRole<Guid>("SuperAdmin")
            {
                Id =Guid.Parse("914B42D8-BFB6-4BA7-BC88-03B3B774711B"),
                NormalizedName = "SUPERADMIN"
            },
            new IdentityRole<Guid>("Support")
            {
                Id =Guid.Parse("E0321B9F-28D3-4DBD-97D8-D6F9B556A14A"),
                NormalizedName = "SUPPORT"
            },
            new IdentityRole<Guid>("Admin")
            {
                Id = Guid.Parse("7D0EA8DC-434C-48E1-B12D-9BB7AF02FEAA"), 
                NormalizedName = "ADMIN"
            },
            new IdentityRole<Guid>("User")
            {
                Id = Guid.Parse("FC57DFBD-27A9-4198-94A3-3F84F13C1F65"),
                NormalizedName = "USER"
            }
        );
    }
}