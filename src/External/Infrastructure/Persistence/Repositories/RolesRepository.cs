using Application.Abstraction.Repositories;
using Domain.Model;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Persistence.Repositories;

public class RolesRepository : IRolesRepository
{
    private readonly RoleManager<UserRoles> _roleManager;

    public RolesRepository(RoleManager<UserRoles> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<IdentityResult> CreateRoleAsync(string role)
    {
        return await _roleManager.CreateAsync(new UserRoles(role));
    }

    public async Task<IdentityResult> DeleteRoleAsync(UserRoles role)
    {
        return await _roleManager.DeleteAsync(role);
    }

    public async Task<bool> RoleExistsAsync(string role)
    {
        return await _roleManager.RoleExistsAsync(role);
    }
}