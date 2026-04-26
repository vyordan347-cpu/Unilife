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

        public async Task<IActionResult> Index(string tipo)
        {
            var lugares = _context.Lugares.AsQueryable();

            if (!string.IsNullOrEmpty(tipo))
            {
                lugares = lugares.Where(l => l.Tipo == tipo);
            }

            ViewBag.Tipo = tipo;

            return View(await lugares.OrderBy(l => l.Nombre).ToListAsync());
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Lugar lugar)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lugar);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(lugar);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var lugar = await _context.Lugares.FindAsync(id);

            if (lugar == null) return NotFound();

            return View(lugar);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Lugar lugar)
        {
            if (id != lugar.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(lugar);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(lugar);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var lugar = await _context.Lugares.FirstOrDefaultAsync(l => l.Id == id);

            if (lugar == null) return NotFound();

            return View(lugar);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
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