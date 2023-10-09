using Microsoft.AspNetCore.Identity;

namespace Domain.Model;

public enum Roles
{
    User,
    Admin,
    Support,
    SuperAdmin
}
public class UserRoles : IdentityRole<Guid>
{
    public string Role { get; set; }
    
    public UserRoles(string role) : base(role){}
    public UserRoles() : base() {}
}