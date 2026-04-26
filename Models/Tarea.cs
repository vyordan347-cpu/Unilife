using System.ComponentModel.DataAnnotations;

namespace Unilife.Models
{
    public class Tarea
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Título")]
        public string Titulo { get; set; } = string.Empty;

        [Required]
        public string Curso { get; set; } = string.Empty;

        [Display(Name = "Fecha de entrega")]
        [DataType(DataType.Date)]
        public DateTime FechaEntrega { get; set; }

        [Required]
        public string Prioridad { get; set; } = string.Empty;

        public bool Completada { get; set; }
    }
}