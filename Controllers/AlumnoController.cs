using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Unilife.Controllers
{
    [Authorize(Roles = "Alumno")]
    public class AlumnoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}