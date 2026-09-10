using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace Api.Extensions;

public static class DatabaseSeederExtensions
{
    public static async Task SeedDatabaseAsync(this WebApplication app,IConfiguration configuration)
    {
        using var scope = app.Services.CreateScope();

        var services = scope.ServiceProvider;

        try
        {
            var userManager =services.GetRequiredService<UserManager<ApplicationUser>>();

            var roleManager =services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            await DbInitializer.SeedAsync( userManager, roleManager,configuration);
        }
        catch (Exception ex)
        {
            var logger =services.GetRequiredService<ILogger<Program>>();

            logger.LogError(ex,"An error occurred while seeding the database.");
        }
    }
}