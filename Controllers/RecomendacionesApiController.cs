using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unilife.Services;

namespace Unilife.Controllers
{
    [ApiController]
    [Route("api/recomendaciones")]
    [AllowAnonymous]
    public class RecomendacionesApiController : ControllerBase
    {
        private readonly RecomendadorEventosService _eventos;
        private readonly RecomendadorLugaresService _lugares;

        public RecomendacionesApiController(RecomendadorEventosService eventos,
                                            RecomendadorLugaresService lugares)
        {
            _eventos = eventos;
            _lugares = lugares;
        }

        // GET: api/recomendaciones/eventos?carrera=Ingeniería de Software
        [HttpGet("eventos")]
        public async Task<IActionResult> RecomendarEventos([FromQuery] string carrera)
        {
            if (string.IsNullOrWhiteSpace(carrera))
                return BadRequest(new { mensaje = "Debes indicar una carrera." });

            var resultado = await _eventos.ObtenerEventosRecomendadosAsync(carrera, 5);

            var data = resultado.Select(e => new
            {
                e.Id,
                e.Titulo,
                e.TipoEvento,
                Fecha = e.Fecha.ToString("yyyy-MM-dd"),
                e.Lugar
            });

            return Ok(data);
        }

        // GET: api/recomendaciones/lugares?usuarioId=...
        [HttpGet("lugares")]
        public async Task<IActionResult> RecomendarLugares([FromQuery] string usuarioId)
        {
            if (string.IsNullOrWhiteSpace(usuarioId))
                return BadRequest(new { mensaje = "Debes indicar el usuarioId." });

            var resultado = await _lugares.ObtenerRecomendacionesAsync(usuarioId, 5);

            var data = resultado.Select(l => new
            {
                l.Id,
                l.Nombre,
                l.Tipo,
                l.Calificacion
            });

            return Ok(data);
        }
    }
}