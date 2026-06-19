using Microsoft.EntityFrameworkCore;
using Unilife.Data;
using Unilife.Models;

namespace Unilife.Services
{
    public class RecomendadorLugaresService
    {
        private readonly ApplicationDbContext _context;

        public RecomendadorLugaresService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Recomienda según la calificación: los mejor valorados que el usuario aún no calificó
        public async Task<List<Lugar>> ObtenerRecomendacionesAsync(string usuarioId, int topN = 5)
        {
            var yaValorados = await _context.ValoracionesLugar
                .Where(v => v.UsuarioId == usuarioId)
                .Select(v => v.LugarId)
                .ToListAsync();

            var lugares = await _context.Lugares
                .Where(l => !yaValorados.Contains(l.Id))
                .OrderByDescending(l => l.Calificacion)
                .Take(topN)
                .ToListAsync();

            return lugares;
        }
    }
}