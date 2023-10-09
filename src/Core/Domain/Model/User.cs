using Microsoft.AspNetCore.Identity;

namespace Domain.Model;

public class User : IdentityUser<Guid>
{
    public override required Guid Id { get; set; }
    public int? Age { get; set; }
    public string? FirstName { get; set; }
    
}