using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Unilife.Models;

namespace Unilife.Data
{
public static class SeedData
{
public static async Task InicializarAsync(IServiceProvider serviceProvider)
{
var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
var context = serviceProvider.GetRequiredService<ApplicationDbContext>();


        // ----- Roles -----
        string[] roles = { "Coordinador", "Docente", "Alumno" };
        foreach (var rol in roles)
        {
            if (!await roleManager.RoleExistsAsync(rol))
                await roleManager.CreateAsync(new IdentityRole(rol));
        }

        // ----- Usuarios -----
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
                    await userManager.AddToRoleAsync(usuario, rol);
            }
        }

        await CrearUsuarioAsync("coordinador@unilife.com", "Admin123*", "Coordinador", "Admin", "UniLife");
        await CrearUsuarioAsync("docente@unilife.com", "Docente123*", "Docente", "Juan", "Pérez");
        await CrearUsuarioAsync("alumno@unilife.com", "Alumno123*", "Alumno", "María", "García", "Ingeniería de Software");

        // ----- Cursos (datos reales en la BD) -----
        if (!await context.Cursos.AnyAsync())
        {
            context.Cursos.AddRange(
                new Curso { Nombre = "Matemáticas Discretas", Descripcion = "Lógica y conjuntos", Carrera = "Ingeniería de Software", Semestre = "III", Docente = "Ing. Ramírez", HoraInicio = new TimeSpan(8, 0, 0), HoraFin = new TimeSpan(10, 0, 0), Pabellon = "A", Aula = "201", CodigoAula = "A201" },
                new Curso { Nombre = "Programación Web", Descripcion = "ASP.NET Core MVC", Carrera = "Ingeniería de Software", Semestre = "V", Docente = "Ing. Torres", HoraInicio = new TimeSpan(10, 0, 0), HoraFin = new TimeSpan(12, 0, 0), Pabellon = "A", Aula = "3", CodigoAula = "A3" },
                new Curso { Nombre = "Base de Datos", Descripcion = "Modelado y SQL", Carrera = "Ingeniería de Software", Semestre = "IV", Docente = "Ing. Salazar", HoraInicio = new TimeSpan(14, 0, 0), HoraFin = new TimeSpan(16, 0, 0), Pabellon = "A", Aula = "105", CodigoAula = "A105" }
            );
            await context.SaveChangesAsync();
        }

        // ----- Eventos -----
        if (!await context.Eventos.AnyAsync())
        {
            context.Eventos.AddRange(
                new Evento { Titulo = "Charla de IA", Descripcion = "Introducción a la inteligencia artificial", Fecha = DateTime.Today.AddDays(3), Hora = new TimeSpan(16, 0, 0), Lugar = "Aula Magna", TipoEvento = "Charla" },
                new Evento { Titulo = "Hackathon UNI", Descripcion = "48 horas de programación", Fecha = DateTime.Today.AddDays(7), Hora = new TimeSpan(9, 0, 0), Lugar = "Auditorio Central", TipoEvento = "Hackathon" },
                new Evento { Titulo = "Taller de Git", Descripcion = "Control de versiones con Git", Fecha = DateTime.Today.AddDays(12), Hora = new TimeSpan(11, 0, 0), Lugar = "Laboratorio 2", TipoEvento = "Taller" }
            );
            await context.SaveChangesAsync();
        }

        // ----- Lugares -----
        if (!await context.Lugares.AnyAsync())
        {
            context.Lugares.AddRange(
                new Lugar { Nombre = "Cafetería Central", Tipo = "Cafetería", Direccion = "Av. Universitaria 123", Distancia = 0.2, PrecioPromedio = 8m, Calificacion = 4.6, Descripcion = "Café y snacks a pasos del campus.", ImagenUrl = "" },
                new Lugar { Nombre = "Biblioteca Municipal", Tipo = "Biblioteca", Direccion = "Jr. Lima 456", Distancia = 0.8, PrecioPromedio = 0m, Calificacion = 4.8, Descripcion = "Espacio silencioso para estudiar.", ImagenUrl = "" },
                new Lugar { Nombre = "Menú El Estudiante", Tipo = "Restaurante", Direccion = "Calle Real 789", Distancia = 0.5, PrecioPromedio = 12m, Calificacion = 4.3, Descripcion = "Almuerzos económicos cerca de la facultad.", ImagenUrl = "" },
                new Lugar { Nombre = "CoWork UNI", Tipo = "Coworking", Direccion = "Av. Grau 321", Distancia = 1.2, PrecioPromedio = 15m, Calificacion = 4.5, Descripcion = "Ambiente para trabajar en equipo con wifi rápido.", ImagenUrl = "" }
            );
            await context.SaveChangesAsync();
        }

        // ----- Tareas del alumno -----
        var alumno = await userManager.FindByEmailAsync("alumno@unilife.com");
        if (alumno != null && !await context.Tareas.AnyAsync(t => t.UsuarioId == alumno.Id))
        {
            context.Tareas.AddRange(
                new Tarea { Titulo = "Entrega proyecto final", Curso = "Programación Web", FechaEntrega = DateTime.Today, Prioridad = "Alta", Completada = false, UsuarioId = alumno.Id },
                new Tarea { Titulo = "Leer capítulo 5", Curso = "Base de Datos", FechaEntrega = DateTime.Today.AddDays(1), Prioridad = "Media", Completada = false, UsuarioId = alumno.Id },
                new Tarea { Titulo = "Resolver ejercicios", Curso = "Matemáticas Discretas", FechaEntrega = DateTime.Today.AddDays(3), Prioridad = "Baja", Completada = false, UsuarioId = alumno.Id },
                new Tarea { Titulo = "Quiz de lógica", Curso = "Matemáticas Discretas", FechaEntrega = DateTime.Today.AddDays(-2), Prioridad = "Media", Completada = true, UsuarioId = alumno.Id }
            );
            await context.SaveChangesAsync();
        }
    }

    public static async Task SeedValoracionesAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        if (await context.ValoracionesLugar.AnyAsync()) return;

        var lugares = await context.Lugares.OrderBy(l => l.Id).ToListAsync();
        if (lugares.Count < 3) return;

        var alumno = await userManager.FindByEmailAsync("alumno@unilife.com");
        var docente = await userManager.FindByEmailAsync("docente@unilife.com");
        var coord = await userManager.FindByEmailAsync("coordinador@unilife.com");

        var lista = new List<ValoracionLugar>();

        void Rate(ApplicationUser? u, int i, int nota)
        {
            if (u == null || i >= lugares.Count) return;

            lista.Add(new ValoracionLugar
            {
                UsuarioId = u.Id,
                LugarId = lugares[i].Id,
                Puntuacion = nota
            });
        }

        // alumno y coordinador con gustos parecidos; docente distinto
        Rate(alumno, 0, 5); Rate(alumno, 1, 4); Rate(alumno, 2, 2); Rate(alumno, 3, 1);
        Rate(coord, 0, 5); Rate(coord, 1, 5); Rate(coord, 2, 3); Rate(coord, 3, 2);
        Rate(docente, 2, 5); Rate(docente, 3, 5); Rate(docente, 0, 2); Rate(docente, 1, 2);

        context.ValoracionesLugar.AddRange(lista);
        await context.SaveChangesAsync();
    }
}


}
