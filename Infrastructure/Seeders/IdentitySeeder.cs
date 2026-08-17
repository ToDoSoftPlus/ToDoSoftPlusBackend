using Domain.Constant;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Seeders
{
    public static class IdentitySeeder
    {
        public static async Task SeedRolesAsync(RoleManager<ApplicationRole> roleManager)
        {
            string[] roles = { UserRoles.Admin, UserRoles.User };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new ApplicationRole() { Name = role, NormalizedName = role.ToUpper() });
            }
        }
    }
}
