using Domain.Model;
using Microsoft.AspNetCore.Identity;

namespace Application.Abstraction.Repositories;

public interface IUsersRepository
{
    public IQueryable<User?> GetAllUsers();
    public Task<User?> GetUserByIdAsync(string id);
    public Task<User?> GetUserByEmailAsync(string email);
    public Task<IList<string>> GetUserRolesAsync(User user);
    public Task<IdentityResult> CreateUserAsync(User user, string password);
    public Task<IdentityResult> UpdateUserAsync(User user);
    public Task<IdentityResult> DeleteUserAsync(User user);
    public Task<IdentityResult> SetUserRoleAsync(User user, UserRoles role);
    public Task<bool> CheckPasswordAsync(User user, string requestPassword);
    public Task<IdentityResult> ResetPasswordAsync(User user, string token, string newPassword);
    public Task<string> GeneratePasswordResetTokenAsync(User user);
    public Task<List<User>> ToListAsync(IQueryable<User> users);
    public Task<IdentityResult> ConfirmUserEmail(User user, string code);
}