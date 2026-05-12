using EcommerceMvc.Models;
using Microsoft.AspNetCore.Identity;

namespace EcommerceMvc.Seed
{
    public static class DbInitializer
    {
        public static async Task SeedAdminUserAsync(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            ILogger logger)
        {
            string[] roles =
            {
                "Admin",
                "Customer"
            };

            foreach (var role in roles)
            {
                var exists = await roleManager.RoleExistsAsync(role);

                if (!exists)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var adminSection = configuration.GetSection("SeedAdmin");
            var adminEmail = adminSection["Email"];
            var adminPassword = adminSection["Password"];
            var adminUserName = adminSection["UserName"] ?? "admin";

            if (string.IsNullOrWhiteSpace(adminEmail) || string.IsNullOrWhiteSpace(adminPassword))
            {
                logger.LogWarning(
                    "No se creó usuario administrador inicial. Configure SeedAdmin:Email y SeedAdmin:Password mediante secretos de usuario o variables de entorno.");
                return;
            }

            var existingUser = await userManager.FindByEmailAsync(adminEmail);

            if (existingUser != null)
                return;

            var adminUser = new ApplicationUser
            {
                FullName = "Administrador",
                UserName = adminUserName,
                Email = adminEmail,
                EmailConfirmed = true,
                IsActive = true
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
                return;
            }

            foreach (var error in result.Errors)
            {
                logger.LogError(
                    "No se pudo crear el administrador inicial. Código: {Code}. Descripción: {Description}",
                    error.Code,
                    error.Description);
            }
        }
    }
}
