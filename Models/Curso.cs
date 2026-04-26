using System.ComponentModel.DataAnnotations;

namespace Unilife.Models
{
    public class Curso
    {
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; } = string.Empty;

        public string Descripcion { get; set; } = string.Empty;

        [Required]
        public string Carrera { get; set; } = string.Empty;

        [Required]
        public string Semestre { get; set; } = string.Empty;

        [Required]
        public string Docente { get; set; } = string.Empty;

        [Display(Name = "Hora de inicio")]
        [DataType(DataType.Time)]
        public TimeSpan HoraInicio { get; set; }

        [Display(Name = "Hora de fin")]
        [DataType(DataType.Time)]
        public TimeSpan HoraFin { get; set; }

        public string Pabellon { get; set; } = string.Empty;

        public string Aula { get; set; } = string.Empty;

        [Display(Name = "Código de aula")]
        public string CodigoAula { get; set; } = string.Empty;
    }
}