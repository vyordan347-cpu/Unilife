using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unilife.Services;

namespace Unilife.Controllers
{
    [Authorize]
    public class ChatbotController : Controller
    {
        private readonly ChatbotService _chat;

        public ChatbotController(ChatbotService chat)
        {
            _chat = chat;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Recibe el historial desde la pantalla y devuelve la respuesta del modelo
        [HttpPost]
        public async Task<IActionResult> Enviar([FromBody] ChatRequest req)
        {
            if (req?.Mensajes == null || req.Mensajes.Count == 0)
                return BadRequest(new { error = "El mensaje está vacío." });

            var historial = req.Mensajes
                .Select(m => new OllamaMessage { Role = m.Rol, Content = m.Contenido })
                .ToList();

            var respuesta = await _chat.ResponderAsync(historial);
            return Json(new { respuesta });
        }
    }

    public class ChatRequest
    {
        public List<ChatMensaje> Mensajes { get; set; } = new();
    }

    public class ChatMensaje
    {
        public string Rol { get; set; } = "";
        public string Contenido { get; set; } = "";
    }
}
