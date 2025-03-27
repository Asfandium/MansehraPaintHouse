using Microsoft.AspNetCore.Identity;
using MansehraPaintHouse.Core.Constants;
using MansehraPaintHouse.Core.Entities;

namespace MansehraPaintHouse.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Create Admin role if it doesn't exist
            if (!await roleManager.RoleExistsAsync(RoleNames.Admin))
            {
                await roleManager.CreateAsync(new IdentityRole(RoleNames.Admin));
            }

            // Create admin users
            var adminUsers = new[]
            {
                new { Email = "admin1@mansehrapainthouse.com", Password = "Admin@123", FirstName = "Admin", LastName = "One" },
                new { Email = "admin2@mansehrapainthouse.com", Password = "Admin@123", FirstName = "Admin", LastName = "Two" },
                new { Email = "admin3@mansehrapainthouse.com", Password = "Admin@123", FirstName = "Admin", LastName = "Three" }
            };

            foreach (var admin in adminUsers)
            {
                var user = await userManager.FindByEmailAsync(admin.Email);
                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        UserName = admin.Email,
                        Email = admin.Email,
                        FirstName = admin.FirstName,
                        LastName = admin.LastName,
                        EmailConfirmed = true,
                        CreatedAt = DateTime.UtcNow,
                        IsActive = true
                    };

                    var result = await userManager.CreateAsync(user, admin.Password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, RoleNames.Admin);
                    }
                }
            }
        }
    }
} 