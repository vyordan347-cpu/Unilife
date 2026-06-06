using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unilife.Data;

namespace Unilife.Controllers
{
    [ApiController]
    [Route("api/lugares")]
    [AllowAnonymous]
    public class LugaresApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LugaresApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/lugares
        [HttpGet]
        public async Task<IActionResult> GetLugares([FromQuery] string? tipo)
        {
            var query = _context.Lugares.AsQueryable();

            if (!string.IsNullOrWhiteSpace(tipo))
                query = query.Where(l => l.Tipo == tipo);

            var lugares = await query
                .OrderByDescending(l => l.Calificacion)
                .Select(l => new
                {
                    l.Id,
                    l.Nombre,
                    l.Tipo,
                    l.Direccion,
                    l.Distancia,
                    l.PrecioPromedio,
                    l.Calificacion
                })
                .ToListAsync();

            return Ok(lugares);
        }
    }
}