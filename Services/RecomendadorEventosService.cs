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
            // Solo eventos futuros
            var hoy = DateTime.Today;
            var eventos = await _context.Eventos
                .Where(e => e.Fecha >= hoy)
                .ToListAsync();

            // Sin carrera (docente/coordinador) -> no se puede personalizar
            if (string.IsNullOrWhiteSpace(carrera))
                return eventos.OrderBy(e => e.Fecha).Take(topN).ToList();

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

            // 2. Predecir para cada evento futuro
            var ranking = new List<(Evento evento, float prob)>();
            foreach (var ev in eventos)
            {
                var pred = motor.Predict(new EventoInteresData
                {
                    Carrera = carrera,
                    TipoEvento = ev.TipoEvento
                });
                if (pred.Interesa)
                    ranking.Add((ev, pred.Probability));
            }

            // Si el modelo no marcó ninguno como interesante, mostramos los próximos
            if (ranking.Count == 0)
                return eventos.OrderBy(e => e.Fecha).Take(topN).ToList();

            return ranking
                .OrderByDescending(r => r.prob)
                .ThenBy(r => r.evento.Fecha)
                .Take(topN)
                .Select(r => r.evento)
                .ToList();
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