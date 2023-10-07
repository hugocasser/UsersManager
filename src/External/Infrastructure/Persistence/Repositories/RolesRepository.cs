using Application.Abstraction.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Persistence.Repositories;

public class RolesRepository : IRolesRepository
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public RolesRepository(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<IdentityResult> CreateRoleAsync(string role)
    {
        return await _roleManager.CreateAsync(new IdentityRole(role));
    }

    public async Task<IdentityResult> DeleteRoleAsync(IdentityRole role)
    {
        return await _roleManager.DeleteAsync(role);
    }

    public async Task<bool> RoleExistsAsync(string role)
    {
        return await _roleManager.RoleExistsAsync(role);
    }
}