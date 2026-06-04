using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unilife.Data;
using Unilife.Models;

namespace Unilife.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public AccountController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var resultado = await _signInManager.PasswordSignInAsync(
                model.Email,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: false
            );

            if (resultado.Succeeded)
            {
                // Todos los roles entran al inicio (dashboard)
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Correo o contraseña incorrectos.");
            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Perfil()
        {
            var usuario = await _userManager.GetUserAsync(User);
            if (usuario == null)
            {
                return RedirectToAction("Login");
            }

            var roles = await _userManager.GetRolesAsync(usuario);

            // Tareas reales del usuario
            var tareas = await _context.Tareas
                .Where(t => t.UsuarioId == usuario.Id)
                .ToListAsync();

            var completadas = tareas.Count(t => t.Completada);
            var total = tareas.Count;

            // Cursos reales
            var cursos = await _context.Cursos
                .Select(c => c.Nombre)
                .ToListAsync();

            var modelo = new PerfilViewModel
            {
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Email = usuario.Email ?? string.Empty,
                Carrera = usuario.Carrera,
                Rol = roles.FirstOrDefault() ?? "Sin rol",

                Completadas = completadas,
                Pendientes = total - completadas,
                Puntos = completadas * 10,
                Progreso = total == 0 ? 0 : (int)Math.Round(completadas * 100.0 / total),

                Cursos = cursos,

                Insignias = new List<Insignia>
                {
                    new() { Emoji = "🏆", Titulo = "Estudiante Productivo", Descripcion = "Completa 10 tareas", Desbloqueada = completadas >= 10 },
                    new() { Emoji = "🚀", Titulo = "Primeros pasos", Descripcion = "Crea tu primera tarea", Desbloqueada = total > 0 },
                    new() { Emoji = "✅", Titulo = "Todo al día", Descripcion = "Sin tareas pendientes", Desbloqueada = total > 0 && completadas == total },
                    new() { Emoji = "📚", Titulo = "Explorador académico", Descripcion = "Tienes cursos registrados", Desbloqueada = cursos.Count > 0 },
                    new() { Emoji = "⭐", Titulo = "Constante", Descripcion = "Completa 5 tareas", Desbloqueada = completadas >= 5 },
                    new() { Emoji = "🎯", Titulo = "Enfocado", Descripcion = "Llega al 80% de progreso", Desbloqueada = total > 0 && (completadas * 100.0 / total) >= 80 }
                }
            };

            return View(modelo);
        }
    }
}