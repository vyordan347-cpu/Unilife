using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unilife.Data;
using Unilife.Models;

namespace Unilife.Controllers
{
    public class LugaresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LugaresController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string tipo, string buscar)
        {
            var lugares = _context.Lugares.AsQueryable();

            if (!string.IsNullOrWhiteSpace(tipo))
            {
                lugares = lugares.Where(l => l.Tipo == tipo);
            }

            if (!string.IsNullOrWhiteSpace(buscar))
            {
                lugares = lugares.Where(l =>
                    l.Nombre.Contains(buscar) ||
                    l.Direccion.Contains(buscar) ||
                    l.Descripcion.Contains(buscar));
            }

            ViewBag.Tipo = tipo;
            ViewBag.Buscar = buscar;

            return View(await lugares.OrderByDescending(l => l.Calificacion).ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var lugar = await _context.Lugares.FirstOrDefaultAsync(l => l.Id == id);

            if (lugar == null) return NotFound();

            return View(lugar);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Lugar lugar)
        {
            ModelState.Remove("Id");

            if (!ModelState.IsValid)
            {
                return View(lugar);
            }

            _context.Lugares.Add(lugar);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var lugar = await _context.Lugares.FindAsync(id);

            if (lugar == null) return NotFound();

            return View(lugar);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Lugar lugar)
        {
            if (id != lugar.Id) return NotFound();

            ModelState.Remove("Id");

            if (!ModelState.IsValid)
            {
                return View(lugar);
            }

            _context.Lugares.Update(lugar);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var lugar = await _context.Lugares.FirstOrDefaultAsync(l => l.Id == id);

            if (lugar == null) return NotFound();

            return View(lugar);
        }

        [HttpPost, ActionName("Delete")]
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