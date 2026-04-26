using Microsoft.EntityFrameworkCore;
using Unilife.Models;

namespace Unilife.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Tarea> Tareas { get; set; }
        public DbSet<Lugar> Lugares { get; set; }
        public DbSet<Evento> Eventos { get; set; }
    }
}