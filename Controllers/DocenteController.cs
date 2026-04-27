using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Unilife.Controllers
{
    [Authorize(Roles = "Docente")]
    public class DocenteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}