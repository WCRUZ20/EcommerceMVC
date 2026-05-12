using EcommerceMvc.Models;
using Microsoft.AspNetCore.Identity;

namespace EcommerceMvc.Seed
{
    public static class DbInitializer
    {
        public static async Task SeedAdminUserAsync(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            // Roles

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
                    await roleManager.CreateAsync(
                        new IdentityRole(role));
                }
            }

            // Usuario admin

            const string adminUserName = "admin";
            const string adminEmail = "admin@ecommerce.com";
            const string adminPassword = "Admin123*";

            var existingUser =
                await userManager.FindByEmailAsync(adminEmail);

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

            var result = await userManager.CreateAsync(
                adminUser,
                adminPassword);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(
                    adminUser,
                    "Admin");
            }
        }
    }
}
