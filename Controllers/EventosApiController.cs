using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unilife.Data;

namespace Unilife.Controllers
{
    [ApiController]
    [Route("api/eventos")]
    [AllowAnonymous]
    public class EventosApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EventosApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/eventos
        [HttpGet]
        public async Task<IActionResult> GetEventos()
        {
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

            return Ok(eventos);
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