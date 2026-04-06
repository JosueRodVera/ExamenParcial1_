using Hoteleria.Models;
using Microsoft.AspNetCore.Identity;

namespace Hoteleria.Data
{
    public static class SeedRoles
    {
        public static async Task Inicializar(IServiceProvider service)
        {
            var roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = service.GetRequiredService<UserManager<ApplicationUser>>();

            // Roles
            string[] roles = { "Administrador", "Cliente" };
            foreach (var rol in roles)
            {
                if (!await roleManager.RoleExistsAsync(rol))
                    await roleManager.CreateAsync(new IdentityRole(rol));
            }

            // Usuario Admin
            string email = "admin@hotel.com";
            string password = "Zxcvbn1@";

            if (await userManager.FindByEmailAsync(email) == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    FullName = "Administrador del Sistema",
                    NombreCompleto = "Administrador del Sistema", // << importante
                    EmailConfirmed = true, // recomendable para evitar bloqueos
                };

                var result = await userManager.CreateAsync(admin, password);

                if (result.Succeeded)
                    await userManager.AddToRoleAsync(admin, "Administrador");
                else
                {
                    foreach (var error in result.Errors)
                        Console.WriteLine(error.Description);
                }
            }
        }
    }
}