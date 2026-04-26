using System.ComponentModel.DataAnnotations;

namespace Unilife.Models
{
    public class Lugar
    {
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public string Tipo { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Dirección")]
        public string Direccion { get; set; } = string.Empty;

        [Display(Name = "Distancia")]
        public double Distancia { get; set; }

        [Display(Name = "Precio promedio")]
        public decimal PrecioPromedio { get; set; }

        [Display(Name = "Calificación")]
        public double Calificacion { get; set; }

        public string Descripcion { get; set; } = string.Empty;

        [Display(Name = "Imagen URL")]
        public string ImagenUrl { get; set; } = string.Empty;
    }
}