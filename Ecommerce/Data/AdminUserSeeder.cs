using Ecommerce.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Ecommerce.Data;

public static class AdminUserSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var serviceProvider = scope.ServiceProvider;
        var options = serviceProvider.GetRequiredService<IOptions<AdminUserOptions>>().Value;
        var logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(AdminUserSeeder));
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        if (string.IsNullOrWhiteSpace(options.UserName) ||
            string.IsNullOrWhiteSpace(options.Email) ||
            string.IsNullOrWhiteSpace(options.Password) ||
            string.IsNullOrWhiteSpace(options.Role))
        {
            logger.LogWarning("Admin user seed skipped because AdminUser configuration is incomplete.");
            return;
        }

        if (!await roleManager.RoleExistsAsync(options.Role))
        {
            var roleResult = await roleManager.CreateAsync(new IdentityRole(options.Role));
            if (!roleResult.Succeeded)
            {
                LogIdentityErrors(logger, "admin role", roleResult.Errors);
                return;
            }
        }

        var adminUser = await userManager.FindByNameAsync(options.UserName)
            ?? await userManager.FindByEmailAsync(options.Email);

        if (adminUser is null)
        {
            adminUser = new ApplicationUser
            {
                UserName = options.UserName,
                Email = options.Email,
                FirstName = "Admin",
                LastName = "Sistema",
                TipoDoc = "N/A",
                NumDocumento = "ADMIN",
                EmailConfirmed = true,
                CreatedAtUtc = DateTime.UtcNow
            };

            var userResult = await userManager.CreateAsync(adminUser, options.Password);
            if (!userResult.Succeeded)
            {
                LogIdentityErrors(logger, "admin user", userResult.Errors);
                return;
            }

            logger.LogInformation("Admin user {AdminUserName} was created for local testing.", options.UserName);
        }

        if (!await userManager.IsInRoleAsync(adminUser, options.Role))
        {
            var addToRoleResult = await userManager.AddToRoleAsync(adminUser, options.Role);
            if (!addToRoleResult.Succeeded)
            {
                LogIdentityErrors(logger, "admin user role assignment", addToRoleResult.Errors);
            }
        }
    }

    private static void LogIdentityErrors(ILogger logger, string operation, IEnumerable<IdentityError> errors)
    {
        logger.LogError(
            "Failed to seed {Operation}: {Errors}",
            operation,
            string.Join("; ", errors.Select(error => error.Description)));
    }
}
