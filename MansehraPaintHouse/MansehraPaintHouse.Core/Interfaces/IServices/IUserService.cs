using MansehraPaintHouse.Core.Entities;
//using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using MansehraPaintHouse.Core.Models;

namespace MansehraPaintHouse.Core.Interfaces.IServices
{
    public interface IUserService
    {
        Task<ApplicationUser> GetUserByIdAsync(string userId);
        Task<ApplicationUser> GetUserByEmailAsync(string email);
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();
        Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password);
        Task<IdentityResult> UpdateUserAsync(ApplicationUser user);
        Task<IdentityResult> DeleteUserAsync(string userId);
        Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
        Task<IdentityResult> ResetPasswordAsync(string userId, string token, string newPassword);
        Task<string> GeneratePasswordResetTokenAsync(string userId);
        Task<IdentityResult> ConfirmEmailAsync(string userId, string token);
        Task<string> GenerateEmailConfirmationTokenAsync(string userId);
        Task<bool> IsEmailConfirmedAsync(string userId);
        Task<IdentityResult> LockUserAsync(string userId, DateTimeOffset? lockoutEnd);
        Task<IdentityResult> UnlockUserAsync(string userId);
        Task<bool> IsInRoleAsync(ApplicationUser user, string role);
    }
} 