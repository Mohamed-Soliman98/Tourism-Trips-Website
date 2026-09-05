using Domain.Enum;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    //public static class DbInitializer
    //{
    //    public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<Guid>> roleManager, IConfiguration configuration)
    //    {
    //        string[] roles = { UserRole.SuperAdmin.ToString(), UserRole.Admin.ToString() };

    //        foreach (var role in roles)
    //        {
    //            if (!await roleManager.RoleExistsAsync(role))
    //            {
    //                await roleManager.CreateAsync(new IdentityRole<Guid>
    //                {
    //                    Name = role,
    //                    NormalizedName = role.ToUpper()
    //                });
    //            }
    //        }

    //        var email = configuration["SeedAdmin:Email"];
    //        var password = configuration["SeedAdmin:Password"];
    //        var fullName = configuration["SeedAdmin:FullName"];

    //        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
    //        {
    //            return;
    //        }

    //        if (await userManager.FindByEmailAsync(email) is not null)
    //        {
    //            return;
    //        }

    //        var admin = new ApplicationUser
    //        {
    //            Id = Guid.NewGuid(),
    //            UserName = email,
    //            FullName = fullName ?? "System Administrator",
    //            Email = email,
    //            EmailConfirmed = true
    //        };

    //        var result = await userManager.CreateAsync(admin, password);

    //        if (!result.Succeeded)
    //        {
    //            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
    //            throw new InvalidOperationException($"Failed to create SuperAdmin: {errors}");
    //        }

    //        await userManager.AddToRoleAsync(admin, UserRole.SuperAdmin.ToString());
    //    }
    //}


    public static class DbInitializer
    {
        public static async Task SeedAsync(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole<Guid>> roleManager,
            IConfiguration configuration)
        {
            string[] roles =
            {
                UserRole.SuperAdmin.ToString(),
                UserRole.Admin.ToString()
            };

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

            var email = configuration["SeedAdmin:Email"];
            var password = configuration["SeedAdmin:Password"];
            var fullName = configuration["SeedAdmin:FullName"];

            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password))
            {
                return;
            }

            // TEMPORARY: Reset password for existing SuperAdmin
            var existingAdmin = await userManager.FindByEmailAsync(email);

            if (existingAdmin is not null)
            {
                var token =
                    await userManager.GeneratePasswordResetTokenAsync(existingAdmin);

                var resetResult =
                    await userManager.ResetPasswordAsync(
                        existingAdmin,
                        token,
                        password);

                if (!resetResult.Succeeded)
                {
                    var errors = string.Join(
                        ", ",
                        resetResult.Errors.Select(e => e.Description));

                    throw new InvalidOperationException(
                        $"Failed to reset SuperAdmin password: {errors}");
                }

                return;
            }

            var admin = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = email,
                FullName = fullName ?? "System Administrator",
                Email = email,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(admin, password);

            if (!result.Succeeded)
            {
                var errors = string.Join(
                    ", ",
                    result.Errors.Select(e => e.Description));

                throw new InvalidOperationException(
                    $"Failed to create SuperAdmin: {errors}");
            }

            await userManager.AddToRoleAsync(
                admin,
                UserRole.SuperAdmin.ToString());
        }
    }
}



