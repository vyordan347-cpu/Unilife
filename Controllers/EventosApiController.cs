using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Unilife.Data;

namespace Unilife.Controllers
{
    [ApiController]
    [Route("api/eventos")]
    [AllowAnonymous]
    public class EventosApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IDistributedCache _cache;

        public EventosApiController(ApplicationDbContext context, IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
        }

        // GET: api/eventos  (con caché)
        [HttpGet]
        public async Task<IActionResult> GetEventos()
        {
            const string cacheKey = "eventos_lista";

            // 1) ¿Ya está en caché? (Redis en producción, memoria en local)
            var cacheado = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cacheado))
            {
                Response.Headers["X-Cache"] = "HIT";
                return Content(cacheado, "application/json");
            }

            // 2) No estaba: se consulta la base de datos
            var eventos = await _context.Eventos
                .OrderBy(e => e.Fecha)
                .Select(e => new
                {
                    e.Id,
                    e.Titulo,
                    e.Descripcion,
                    Fecha = e.Fecha.ToString("yyyy-MM-dd"),
                    Hora = e.Hora.ToString(@"hh\:mm"),
                    e.Lugar,
                    e.TipoEvento
                })
                .ToListAsync();

            var json = JsonSerializer.Serialize(eventos);

            // 3) Se guarda en caché por 5 minutos
            await _cache.SetStringAsync(cacheKey, json, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });

            Response.Headers["X-Cache"] = "MISS";
            return Content(json, "application/json");
        }

        // GET: api/eventos/proximos
        [HttpGet("proximos")]
        public async Task<IActionResult> GetProximos()
        {
            var hoy = DateTime.Today;
            var eventos = await _context.Eventos
                .Where(e => e.Fecha >= hoy)
                .OrderBy(e => e.Fecha)
                .Select(e => new
                {
                    e.Id,
                    e.Titulo,
                    Fecha = e.Fecha.ToString("yyyy-MM-dd"),
                    e.TipoEvento,
                    e.Lugar
                })
                .ToListAsync();

            return Ok(eventos);
        }
    }
}
