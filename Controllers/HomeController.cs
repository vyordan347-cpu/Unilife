using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unilife.Data;
using Unilife.Models;

namespace Unilife.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var userId = _userManager.GetUserId(User);
        var usuario = await _userManager.GetUserAsync(User);
        var hoy = DateTime.Today;

        // Tareas reales del alumno
        var tareasUsuario = await _context.Tareas
            .Where(t => t.UsuarioId == userId)
            .ToListAsync();

        // Cursos (horario) y eventos próximos (recordatorios) reales
        var cursos = await _context.Cursos.ToListAsync();
        var eventosProximos = await _context.Eventos
            .Where(e => e.Fecha >= hoy)
            .ToListAsync();

        var completadas = tareasUsuario.Count(t => t.Completada);
        var total = tareasUsuario.Count;

        var modelo = new DashboardViewModel
        {
            Nombre = usuario?.Nombre ?? "Estudiante",
            Completadas = completadas,
            Pendientes = total - completadas,
            EventosProximos = eventosProximos.Count,
            Progreso = total == 0 ? 0 : (int)Math.Round(completadas * 100.0 / total),

            ProximasTareas = tareasUsuario
                .Where(t => !t.Completada)
                .OrderBy(t => t.FechaEntrega)
                .Take(4)
                .ToList(),

            Horario = cursos
                .OrderBy(c => c.HoraInicio)
                .Take(4)
                .ToList(),

            Recordatorios = eventosProximos
                .OrderBy(e => e.Fecha)
                .ThenBy(e => e.Hora)
                .Take(3)
                .ToList()
        };

        return View(modelo);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [AllowAnonymous]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
