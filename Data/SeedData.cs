using Microsoft.AspNetCore.Identity;
using Unilife.Models;

namespace Unilife.Data
{
    public static class SeedData
    {
        public static async Task InicializarAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roles = { "Coordinador", "Docente", "Alumno" };

            foreach (var rol in roles)
            {
                if (!await roleManager.RoleExistsAsync(rol))
                {
                    await roleManager.CreateAsync(new IdentityRole(rol));
                }
            }

            // Crear usuario coordinador
            var email = "coordinador@unilife.com";

            var usuario = await userManager.FindByEmailAsync(email);

            if (usuario == null)
            {
                usuario = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    Nombre = "Admin",
                    Apellido = "UniLife"
                };

                await userManager.CreateAsync(usuario, "Admin123*");
                await userManager.AddToRoleAsync(usuario, "Coordinador");
            }
        }
    }
}