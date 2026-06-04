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

            // Método auxiliar para no repetir código
            async Task CrearUsuarioAsync(
                string email, string password, string rol,
                string nombre, string apellido, string? carrera = null)
            {
                if (await userManager.FindByEmailAsync(email) == null)
                {
                    var usuario = new ApplicationUser
                    {
                        UserName = email,
                        Email = email,
                        EmailConfirmed = true,
                        Nombre = nombre,
                        Apellido = apellido,
                        Carrera = carrera
                    };

                    var resultado = await userManager.CreateAsync(usuario, password);

                    if (resultado.Succeeded)
                    {
                        await userManager.AddToRoleAsync(usuario, rol);
                    }
                }
            }

            await CrearUsuarioAsync("coordinador@unilife.com", "Admin123*", "Coordinador", "Admin", "UniLife");
            await CrearUsuarioAsync("docente@unilife.com", "Docente123*", "Docente", "Juan", "Pérez");
            await CrearUsuarioAsync("alumno@unilife.com", "Alumno123*", "Alumno", "María", "García", "Ingeniería de Software");
        }
    }
}