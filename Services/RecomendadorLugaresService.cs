using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using Microsoft.ML.Trainers;
using Unilife.Data;
using Unilife.Models;
using Unilife.Models.ML;

namespace Unilife.Services
{
    public class RecomendadorLugaresService
    {
        private readonly ApplicationDbContext _context;
        private readonly MLContext _mlContext;

        public RecomendadorLugaresService(ApplicationDbContext context)
        {
            _context = context;
            _mlContext = new MLContext(seed: 0);
        }

        public async Task<List<Lugar>> ObtenerRecomendacionesAsync(string usuarioId, int topN = 5)
        {
            var valoraciones = await _context.ValoracionesLugar.ToListAsync();
            var lugares = await _context.Lugares.ToListAsync();

            
            if (valoraciones.Count < 10)
                return Fallback(usuarioId, valoraciones, lugares, topN);

            
            var datos = valoraciones.Select(v => new LugarValoracionData
            {
                UsuarioId = v.UsuarioId,
                LugarId = v.LugarId.ToString(),
                Label = v.Puntuacion
            });
            var trainingData = _mlContext.Data.LoadFromEnumerable(datos);

            // 2. Pipeline + entrenamiento
            var options = new MatrixFactorizationTrainer.Options
            {
                MatrixColumnIndexColumnName = "UsuarioIdKey",
                MatrixRowIndexColumnName = "LugarIdKey",
                LabelColumnName = "Label",
                NumberOfIterations = 20,
                ApproximationRank = 16
            };

            var pipeline = _mlContext.Transforms.Conversion
                .MapValueToKey("UsuarioIdKey", "UsuarioId")
                .Append(_mlContext.Transforms.Conversion.MapValueToKey("LugarIdKey", "LugarId"))
                .Append(_mlContext.Recommendation().Trainers.MatrixFactorization(options));

            var modelo = pipeline.Fit(trainingData);
            var motor = _mlContext.Model
                .CreatePredictionEngine<LugarValoracionData, LugarPrediccion>(modelo);

            
            var yaValorados = valoraciones
                .Where(v => v.UsuarioId == usuarioId)
                .Select(v => v.LugarId)
                .ToHashSet();

            var ranking = new List<(Lugar lugar, float score)>();
            foreach (var lugar in lugares.Where(l => !yaValorados.Contains(l.Id)))
            {
                var pred = motor.Predict(new LugarValoracionData
                {
                    UsuarioId = usuarioId,
                    LugarId = lugar.Id.ToString()
                });
                if (!float.IsNaN(pred.Score))
                    ranking.Add((lugar, pred.Score));
            }

            if (ranking.Count == 0)
                return Fallback(usuarioId, valoraciones, lugares, topN);

            return ranking
                .OrderByDescending(r => r.score)
                .Take(topN)
                .Select(r => r.lugar)
                .ToList();
        }

        private List<Lugar> Fallback(string usuarioId, List<ValoracionLugar> valoraciones,
                                     List<Lugar> lugares, int topN)
        {
            var yaValorados = valoraciones
                .Where(v => v.UsuarioId == usuarioId)
                .Select(v => v.LugarId)
                .ToHashSet();

            return lugares
                .Where(l => !yaValorados.Contains(l.Id))
                .OrderByDescending(l => l.Calificacion)
                .Take(topN)
                .ToList();
        }
    }
}