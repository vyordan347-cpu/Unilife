namespace Unilife.Models
{
    public class PerfilViewModel
    {
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Carrera { get; set; }
        public string Rol { get; set; } = string.Empty;
    }
}