using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unilife.Data;
using Unilife.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Unilife.Services;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Unilife.Controllers
{
    [Authorize]
    public class EventosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly RecomendadorEventosService _recomendador;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDistributedCache _cache;

        public EventosController(
            ApplicationDbContext context,
            RecomendadorEventosService recomendador,
            UserManager<ApplicationUser> userManager,
            IDistributedCache cache)
        {
            _context = context;
            _recomendador = recomendador;
            _userManager = userManager;
            _cache = cache;
        }

        public async Task<IActionResult> Index(string tipoEvento, bool soloRecomendados = false)
        {
            var usuario = await _userManager.GetUserAsync(User);

            var claveCache = $"eventos_reco_{usuario?.Carrera ?? "sincarrera"}";
            List<Evento> recomendados;

            var enCache = await _cache.GetStringAsync(claveCache);

            if (enCache != null)
            {
                recomendados = JsonSerializer.Deserialize<List<Evento>>(enCache)!;
            }
            else
            {
                recomendados = await _recomendador
                    .ObtenerEventosRecomendadosAsync(usuario?.Carrera, 5);

                await _cache.SetStringAsync(
                    claveCache,
                    JsonSerializer.Serialize(recomendados),
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                    });
            }

            var recomendadosIds = recomendados.Select(e => e.Id).ToHashSet();

            var eventos = _context.Eventos.AsQueryable();

            if (!string.IsNullOrEmpty(tipoEvento))
            {
                eventos = eventos.Where(e => e.TipoEvento == tipoEvento);
            }

            var listaEventos = await eventos.ToListAsync();

            listaEventos = listaEventos
                .OrderBy(e => e.Fecha)
                .ThenBy(e => e.Hora)
                .ToList();

            if (soloRecomendados)
            {
                listaEventos = listaEventos
                    .Where(e => recomendadosIds.Contains(e.Id))
                    .ToList();
            }

            ViewBag.TipoEvento = tipoEvento;
            ViewBag.RecomendadosIds = recomendadosIds;
            ViewBag.SoloRecomendados = soloRecomendados;
            ViewBag.Carrera = usuario?.Carrera;

            return View(listaEventos);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var evento = await _context.Eventos.FirstOrDefaultAsync(e => e.Id == id);

            if (evento == null) return NotFound();

            return View(evento);
        }

        public async Task<IActionResult> Recomendados()
        {
            var usuario = await _userManager.GetUserAsync(User);

            var recomendados = await _recomendador
                .ObtenerEventosRecomendadosAsync(usuario?.Carrera, 5);

            ViewBag.Carrera = usuario?.Carrera;

            return View(recomendados);
        }

        [Authorize(Roles = "Coordinador")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Coordinador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Evento evento)
        {
            if (ModelState.IsValid)
            {
                _context.Eventos.Add(evento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(evento);
        }

        [Authorize(Roles = "Coordinador")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var evento = await _context.Eventos.FindAsync(id);

            if (evento == null) return NotFound();

            return View(evento);
        }

        [Authorize(Roles = "Coordinador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Evento evento)
        {
            if (id != evento.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Eventos.Update(evento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(evento);
        }

        [Authorize(Roles = "Coordinador")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var evento = await _context.Eventos.FirstOrDefaultAsync(e => e.Id == id);

            if (evento == null) return NotFound();

            return View(evento);
        }

        [Authorize(Roles = "Coordinador")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var evento = await _context.Eventos.FindAsync(id);

            if (evento != null)
            {
                _context.Eventos.Remove(evento);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}