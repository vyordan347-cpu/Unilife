namespace Unilife.Models
{
    public class PerfilViewModel
    {
        // Datos del usuario
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Carrera { get; set; }
        public string Rol { get; set; } = string.Empty;

        // Estadísticas (calculadas desde la BD)
        public int Completadas { get; set; }
        public int Pendientes { get; set; }
        public int Puntos { get; set; }
        public int Progreso { get; set; } // 0 a 100

        // Cursos reales
        public List<string> Cursos { get; set; } = new();

        // Logros derivados de datos reales
        public List<Insignia> Insignias { get; set; } = new();
    }

    public class Insignia
    {
        public string Emoji { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public bool Desbloqueada { get; set; }
    }
}
