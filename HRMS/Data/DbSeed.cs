using Microsoft.AspNetCore.Identity;
using HRMS.Data;

public static class DbSeed
{
    public static async Task SeedRolesAndAdminAsync(IServiceProvider services)
    {
        var roleManager =
            services.GetRequiredService<RoleManager<IdentityRole>>();

        var userManager =
            services.GetRequiredService<UserManager<ApplicationUser>>();

        // Create Roles
        string[] roles =
        {
            "Admin",
            "HR",
            "Manager",
            "Employee"
        };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(
                    new IdentityRole(role));
            }
        }

        // =========================
        // ADMIN USER
        // =========================

        string adminEmail = "admin@hrms.com";
        string adminPassword = "Admin@123";

        var adminUser =
            await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var result =
                await userManager.CreateAsync(
                    adminUser,
                    adminPassword);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(
                    adminUser,
                    "Admin");
            }
        }

        // =========================
        // HR USER
        // =========================

        string hrEmail = "hr@hrms.com";
        string hrPassword = "Hr@12345";

        var hrUser =
            await userManager.FindByEmailAsync(hrEmail);

        if (hrUser == null)
        {
            hrUser = new ApplicationUser
            {
                UserName = hrEmail,
                Email = hrEmail,
                EmailConfirmed = true
            };

            var result =
                await userManager.CreateAsync(
                    hrUser,
                    hrPassword);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(
                    hrUser,
                    "HR");
            }
        }
    }
}