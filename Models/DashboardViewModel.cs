namespace Unilife.Models
{
    public class DashboardViewModel
    {
        public string Nombre { get; set; } = "Estudiante";

        // Estadísticas (calculadas desde la base de datos)
        public int Completadas { get; set; }
        public int Pendientes { get; set; }
        public int EventosProximos { get; set; }
        public int Progreso { get; set; } // 0 a 100

        // Listas reales
        public List<Tarea> ProximasTareas { get; set; } = new();
        public List<Curso> Horario { get; set; } = new();
        public List<Evento> Recordatorios { get; set; } = new();
    }
}
