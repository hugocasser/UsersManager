using Application.Abstraction.Repositories;
using Domain.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly UserManager<User> _userManager;

    public UsersRepository(UserManager<User> userManager)
    {
        _userManager = userManager;
    }
    public IQueryable<User?> GetAllUsers()
    {
        return _userManager.Users;
    }

    public async Task<User?> GetUserByIdAsync(Guid id)
    {
        return await _userManager.FindByIdAsync(id.ToString());
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<IList<string>> GetUserRolesAsync(User user)
    {
        return await _userManager.GetRolesAsync(user);
    }

    public async Task<IdentityResult> CreateUserAsync(User user, string password)
    {
        return await _userManager.CreateAsync(user, password);
    }

    public async Task<IdentityResult> UpdateUserAsync(User user)
    {
        return await _userManager.UpdateAsync(user);
    }

    public async Task<IdentityResult> DeleteUserAsync(User user)
    {
        return await _userManager.DeleteAsync(user);
    }

    public async Task<IdentityResult> SetUserRoleAsync(User user, UserRoles role)
    {
        return await _userManager.AddToRoleAsync(user, role.ToString());
    }

    public async Task<bool> CheckPasswordAsync(User user, string requestPassword)
    {
        return await _userManager.CheckPasswordAsync(user, requestPassword);
    }

    public async Task<IdentityResult> ResetPasswordAsync(User user, string token, string newPassword)
    {
        return await _userManager.ResetPasswordAsync(user, token, newPassword);
    }

    public async Task<string> GeneratePasswordResetTokenAsync(User user)
    {
        return await _userManager.GeneratePasswordResetTokenAsync(user);
    }

    public async Task<List<User>> ToListAsync(IQueryable<User> users)
    {
        return await users.ToListAsync();
    }

    public async Task<IdentityResult> ConfirmUserEmail(User user, string code)
    {
        return await _userManager.ConfirmEmailAsync(user, code);
    }
}