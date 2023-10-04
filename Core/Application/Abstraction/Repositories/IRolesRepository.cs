using Microsoft.AspNetCore.Identity;

namespace Application.Abstraction.Repositories;

public interface IRolesRepository
{
    public Task<IdentityResult> CreateRoleAsync(string role);
    public Task<IdentityResult> DeleteRoleAsync(IdentityRole role);
    public Task<bool> RoleExistsAsync(string role);
}