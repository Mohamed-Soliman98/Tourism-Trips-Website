using Domain.Enum;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Date
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            string[] roles = { UserRole.SuperAdmin.ToString(), UserRole.Admin.ToString() };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(
                        new IdentityRole<Guid>
                        {
                            Name = role,
                            NormalizedName = role.ToUpper()
                        });
                }
            }

            const string superAdminEmail = "SuperAdmin@tourism.com";

            if (await userManager.FindByEmailAsync(superAdminEmail) is null)
            {
                var admin = new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    UserName = superAdminEmail,
                    FullName = "Mohamed Soliman",
                    Email = superAdminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(admin, "NewPassword@123456"); 

                if (!result.Succeeded)
                {
                    var errors = string.Join( ", ",result.Errors.Select(e => e.Description));

                    throw new InvalidOperationException($"Failed to create SuperAdmin: {errors}");
                    
                }
                await userManager.AddToRoleAsync(admin, UserRole.SuperAdmin.ToString());    
            }
        }
    }
}
