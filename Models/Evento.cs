using System.ComponentModel.DataAnnotations;

namespace Unilife.Models
{
    public class Evento
    {
        public int Id { get; set; }

        [Required]
        public string Titulo { get; set; } = string.Empty;

        [Required]
        public string Tipo { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan Hora { get; set; }

        [Required]
        public string Lugar { get; set; } = string.Empty;

        public int Interesados { get; set; }
    }
}