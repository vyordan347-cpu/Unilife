using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unilife.Data;
using Unilife.Models;

namespace Unilife.Controllers
{
    [Authorize]
    public class TareasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TareasController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Solo las tareas del usuario logueado
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            var tareas = await _context.Tareas
                .Where(t => t.UsuarioId == userId)
                .ToListAsync();

            tareas = tareas
                .OrderBy(t => t.Completada)
                .ThenBy(t => t.FechaEntrega)
                .ToList();

            return View(tareas);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Tarea tarea)
        {
            if (ModelState.IsValid)
            {
                tarea.UsuarioId = _userManager.GetUserId(User);
                _context.Add(tarea);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(tarea);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var tarea = await ObtenerTareaPropiaAsync(id.Value);
            if (tarea == null) return NotFound();

            return View(tarea);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Tarea tarea)
        {
            if (id != tarea.Id) return NotFound();

            var existente = await ObtenerTareaPropiaAsync(id);
            if (existente == null) return NotFound();

            if (ModelState.IsValid)
            {
                existente.Titulo = tarea.Titulo;
                existente.Curso = tarea.Curso;
                existente.FechaEntrega = tarea.FechaEntrega;
                existente.Prioridad = tarea.Prioridad;
                existente.Completada = tarea.Completada;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(tarea);
        }

        // Marcar / desmarcar como completada
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Completar(int id)
        {
            var tarea = await ObtenerTareaPropiaAsync(id);
            if (tarea == null) return NotFound();

            tarea.Completada = !tarea.Completada;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var tarea = await ObtenerTareaPropiaAsync(id.Value);
            if (tarea == null) return NotFound();

            return View(tarea);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tarea = await ObtenerTareaPropiaAsync(id);

            if (tarea != null)
            {
                _context.Tareas.Remove(tarea);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Garantiza que el alumno solo acceda a SUS tareas
        private async Task<Tarea?> ObtenerTareaPropiaAsync(int id)
        {
            var userId = _userManager.GetUserId(User);
            return await _context.Tareas
                .FirstOrDefaultAsync(t => t.Id == id && t.UsuarioId == userId);
        }
    }
}
