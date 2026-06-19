using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unilife.Data;
using Unilife.Models;
using Microsoft.AspNetCore.Identity;
using Unilife.Services;

namespace Unilife.Controllers
{
    [Authorize]
    public class LugaresController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly RecomendadorLugaresService _recomendador;
        private readonly UserManager<ApplicationUser> _userManager;

        public LugaresController(
            ApplicationDbContext context,
            RecomendadorLugaresService recomendador,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _recomendador = recomendador;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string tipo, string buscar, bool soloRecomendados = false)
        {
            var usuarioId = _userManager.GetUserId(User);
            var recomendados = await _recomendador.ObtenerRecomendacionesAsync(usuarioId!, 5);
            var recomendadosIds = recomendados.Select(l => l.Id).ToHashSet();

            var lugares = _context.Lugares.AsQueryable();

            if (!string.IsNullOrWhiteSpace(tipo))
                lugares = lugares.Where(l => l.Tipo == tipo);

            if (!string.IsNullOrWhiteSpace(buscar))
            {
                lugares = lugares.Where(l =>
                    l.Nombre.Contains(buscar) ||
                    l.Direccion.Contains(buscar) ||
                    l.Descripcion.Contains(buscar));
            }

            var lista = await lugares.OrderByDescending(l => l.Calificacion).ToListAsync();

            if (soloRecomendados)
                lista = lista.Where(l => recomendadosIds.Contains(l.Id)).ToList();

            ViewBag.Tipo = tipo;
            ViewBag.Buscar = buscar;
            ViewBag.RecomendadosIds = recomendadosIds;
            ViewBag.SoloRecomendados = soloRecomendados;

            return View(lista);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var lugar = await _context.Lugares.FirstOrDefaultAsync(l => l.Id == id);
            if (lugar == null) return NotFound();

            return View(lugar);
        }

        // POST: /Lugares/Valorar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Valorar(int lugarId, int puntuacion)
        {
            if (puntuacion < 1 || puntuacion > 5)
                return RedirectToAction(nameof(Details), new { id = lugarId });

            var usuarioId = _userManager.GetUserId(User);

            var existente = await _context.ValoracionesLugar
                .FirstOrDefaultAsync(v =>
                    v.UsuarioId == usuarioId &&
                    v.LugarId == lugarId);

            if (existente == null)
            {
                _context.ValoracionesLugar.Add(new ValoracionLugar
                {
                    UsuarioId = usuarioId!,
                    LugarId = lugarId,
                    Puntuacion = puntuacion
                });
            }
            else
            {
                existente.Puntuacion = puntuacion;
                existente.Fecha = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = lugarId });
        }

        [Authorize(Roles = "Coordinador")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Coordinador")]
        public async Task<IActionResult> Create(Lugar lugar)
        {
            ModelState.Remove("Id");

            if (!ModelState.IsValid)
                return View(lugar);

            _context.Lugares.Add(lugar);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Coordinador")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var lugar = await _context.Lugares.FindAsync(id);
            if (lugar == null) return NotFound();

            return View(lugar);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Coordinador")]
        public async Task<IActionResult> Edit(int id, Lugar lugar)
        {
            if (id != lugar.Id) return NotFound();

            ModelState.Remove("Id");

            if (!ModelState.IsValid)
                return View(lugar);

            _context.Lugares.Update(lugar);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Coordinador")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var lugar = await _context.Lugares.FirstOrDefaultAsync(l => l.Id == id);
            if (lugar == null) return NotFound();

            return View(lugar);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Coordinador")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lugar = await _context.Lugares.FindAsync(id);

            if (lugar != null)
            {
                _context.Lugares.Remove(lugar);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}