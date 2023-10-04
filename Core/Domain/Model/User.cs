using Microsoft.AspNetCore.Identity;

namespace Domain.Model;

public class User : IdentityUser<Guid>
{
    public int? Age { get; set; }
    public string? FirstName { get; set; }
}