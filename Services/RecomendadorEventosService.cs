using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using Unilife.Data;
using Unilife.Models;
using Unilife.Models.ML;

namespace Unilife.Services
{
    public class RecomendadorEventosService
    {
        private readonly ApplicationDbContext _context;
        private readonly MLContext _mlContext;

        public RecomendadorEventosService(ApplicationDbContext context)
        {
            _context = context;
            _mlContext = new MLContext(seed: 0);
        }

        public async Task<List<Evento>> ObtenerEventosRecomendadosAsync(string? carrera, int topN = 5)
        {
            var hoy = DateTime.Today;
            var eventos = await _context.Eventos.ToListAsync();

            // Sin carrera (docente/coordinador): próximos primero, y si no hay, los más recientes
            if (string.IsNullOrWhiteSpace(carrera))
                return OrdenarPorFecha(eventos, hoy).Take(topN).ToList();

            // 1. Entrenar el modelo con datos sintéticos
            var datos = _mlContext.Data.LoadFromEnumerable(DatosEntrenamiento());

            var pipeline = _mlContext.Transforms.Categorical
                .OneHotEncoding("CarreraCod", "Carrera")
                .Append(_mlContext.Transforms.Categorical.OneHotEncoding("TipoCod", "TipoEvento"))
                .Append(_mlContext.Transforms.Concatenate("Features", "CarreraCod", "TipoCod"))
                .Append(_mlContext.BinaryClassification.Trainers
                    .SdcaLogisticRegression(labelColumnName: "Interesa", featureColumnName: "Features"));

            var modelo = pipeline.Fit(datos);
            var motor = _mlContext.Model
                .CreatePredictionEngine<EventoInteresData, EventoPrediccion>(modelo);

            // 2. Eventos que le interesan según carrera + tipo
            var interesantes = new List<Evento>();
            foreach (var ev in eventos)
            {
                var pred = motor.Predict(new EventoInteresData
                {
                    Carrera = carrera,
                    TipoEvento = ev.TipoEvento
                });
                if (pred.Interesa)
                    interesantes.Add(ev);
            }

            // Si el modelo no marcó ninguno, mostramos algunos igual (próximos primero)
            if (interesantes.Count == 0)
                return OrdenarPorFecha(eventos, hoy).Take(topN).ToList();

            // Con fecha: primero los próximos (más cercanos), luego los pasados (más recientes)
            return OrdenarPorFecha(interesantes, hoy).Take(topN).ToList();
        }

        // Próximos primero (del más cercano al más lejano); después los pasados (del más reciente al más antiguo)
        private static IEnumerable<Evento> OrdenarPorFecha(List<Evento> eventos, DateTime hoy)
        {
            return eventos
                .OrderBy(e => e.Fecha >= hoy ? 0 : 1)
                .ThenBy(e => e.Fecha >= hoy ? e.Fecha.Ticks : -e.Fecha.Ticks);
        }

        private static IEnumerable<EventoInteresData> DatosEntrenamiento()
        {
            (string carrera, string tipo, bool interesa)[] filas =
            {
                ("Ingeniería de Software", "Hackathon", true),
                ("Ingeniería de Software", "Taller", true),
                ("Ingeniería de Software", "Charla", true),
                ("Ingeniería de Software", "Conferencia", true),
                ("Ingeniería de Software", "Arte", false),
                ("Ingeniería de Software", "Deporte", false),
                ("Ingeniería de Software", "Música", false),

                ("Medicina", "Conferencia", true),
                ("Medicina", "Charla", true),
                ("Medicina", "Taller", true),
                ("Medicina", "Hackathon", false),
                ("Medicina", "Música", false),
                ("Medicina", "Deporte", true),

                ("Derecho", "Conferencia", true),
                ("Derecho", "Charla", true),
                ("Derecho", "Hackathon", false),
                ("Derecho", "Arte", false),
                ("Derecho", "Deporte", false),

                ("Arquitectura", "Arte", true),
                ("Arquitectura", "Taller", true),
                ("Arquitectura", "Conferencia", true),
                ("Arquitectura", "Hackathon", false),
                ("Arquitectura", "Deporte", false),

                ("Administración", "Conferencia", true),
                ("Administración", "Charla", true),
                ("Administración", "Hackathon", false),
                ("Administración", "Arte", false),
            };

            foreach (var f in filas)
  
                yield return new EventoInteresData { Carrera = f.carrera, TipoEvento = f.tipo, Interesa = f.interesa };
        }




    }
}