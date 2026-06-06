using System.ComponentModel.DataAnnotations;

namespace Unilife.Models
{
    public class ValoracionLugar
    {
        public int Id { get; set; }

        // Quién valoró (id del usuario de Identity)
        [Required]
        public string UsuarioId { get; set; } = string.Empty;

        // Qué lugar valoró
        [Required]
        public int LugarId { get; set; }

        // Nota de 1 a 5 estrellas
        [Range(1, 5)]
        public int Puntuacion { get; set; }

        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        // Relación opcional hacia el lugar
        public Lugar? Lugar { get; set; }
    }
}