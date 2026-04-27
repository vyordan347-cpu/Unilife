using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unilife.Data;
using Unilife.Models;
using Microsoft.AspNetCore.Authorization;

namespace Unilife.Controllers
{
    [Authorize]

    public class EventosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventosController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string tipoEvento)
        {
            var eventos = _context.Eventos.AsQueryable();

            if (!string.IsNullOrEmpty(tipoEvento))
            {
                eventos = eventos.Where(e => e.TipoEvento == tipoEvento);
            }

            ViewBag.TipoEvento = tipoEvento;

            var listaEventos = await eventos.ToListAsync();

            listaEventos = listaEventos
                .OrderBy(e => e.Fecha)
                .ThenBy(e => e.Hora)
                .ToList();

            return View(listaEventos);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var evento = await _context.Eventos.FirstOrDefaultAsync(e => e.Id == id);

            if (evento == null) return NotFound();

            return View(evento);
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