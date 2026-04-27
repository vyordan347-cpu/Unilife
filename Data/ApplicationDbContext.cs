using Microsoft.EntityFrameworkCore;
using Unilife.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace Unilife.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Tarea> Tareas { get; set; }
        public DbSet<Lugar> Lugares { get; set; }
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Curso> Cursos { get; set; }
    }
}