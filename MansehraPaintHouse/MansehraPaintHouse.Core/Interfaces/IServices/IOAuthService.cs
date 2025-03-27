using MansehraPaintHouse.Core.Entities;

namespace MansehraPaintHouse.Core.Interfaces.IServices
{
    public interface IOAuthService
    {
        Task<string> GetAuthorizationUrlAsync();
        Task<ApplicationUser> HandleCallbackAsync(string code);
        Task<ApplicationUser> GetUserInfoAsync(string accessToken);
        Task<bool> ValidateTokenAsync(string token);
    }
} 