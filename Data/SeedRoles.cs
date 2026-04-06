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

            string[] roles = { "Administrador", "Cliente" };

            foreach (var rol in roles)
            {
                if (!await roleManager.RoleExistsAsync(rol))
                    await roleManager.CreateAsync(new IdentityRole(rol));
            }

            // Crear usuario admin por defecto
            string email = "admin@hotel.com";
            string password = "Zxcvbn1@";   /// la contraseña no olvidar 

            if (await userManager.FindByEmailAsync(email) == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    FullName = "Administrador del Sistema"
                };

                var result = await userManager.CreateAsync(admin, password);

                if (result.Succeeded)
                    await userManager.AddToRoleAsync(admin, "Administrador");
            }
        }
    }
}