using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Unilife.Data;

namespace Unilife.Services
{
    public class ChatbotService
    {
        private readonly IHttpClientFactory _httpFactory;
        private readonly ApplicationDbContext _context;
        private readonly string _baseUrl;
        private readonly string _model;

        public ChatbotService(IHttpClientFactory httpFactory,
                              ApplicationDbContext context,
                              IConfiguration config)
        {
            _httpFactory = httpFactory;
            _context = context;
            _baseUrl = config["Ollama:BaseUrl"] ?? "http://localhost:11434";
            _model = config["Ollama:Model"] ?? "llama3.1:8b";
        }

        public async Task<string> ResponderAsync(List<OllamaMessage> historial)
        {
            var contexto = await ConstruirContextoAsync();

            var mensajes = new List<OllamaMessage>
            {
                new() { Role = "system", Content = contexto }
            };
            mensajes.AddRange(historial);

            var peticion = new OllamaChatRequest
            {
                Model = _model,
                Messages = mensajes,
                Stream = false
            };

            try
            {
                var client = _httpFactory.CreateClient();
                client.Timeout = TimeSpan.FromMinutes(3); // el modelo puede tardar la 1ra vez

                var respuesta = await client.PostAsJsonAsync($"{_baseUrl}/api/chat", peticion);
                respuesta.EnsureSuccessStatusCode();

                var data = await respuesta.Content.ReadFromJsonAsync<OllamaChatResponse>();
                return data?.Message?.Content?.Trim()
                       ?? "No obtuve respuesta del asistente.";
            }
            catch (Exception)
            {
                return "No pude conectar con el asistente. Asegúrate de que Ollama esté abierto en tu PC (icono de la llama) y que el modelo esté descargado.";
            }
        }

        // Arma el "system prompt" con los datos reales de UniLife
        private async Task<string> ConstruirContextoAsync()
        {
            var hoy = DateTime.Today;

            var cursos = await _context.Cursos.ToListAsync();
            var eventos = await _context.Eventos
                .Where(e => e.Fecha >= hoy)
                .OrderBy(e => e.Fecha)
                .Take(8)
                .ToListAsync();
            var lugares = await _context.Lugares
                .OrderByDescending(l => l.Calificacion)
                .Take(8)
                .ToListAsync();

            var sb = new System.Text.StringBuilder();
            sb.AppendLine("Eres el asistente virtual de UniLife, una app universitaria para estudiantes.");
            sb.AppendLine("Ayudas con cursos, eventos, lugares cercanos, tareas y uso de la app.");
            sb.AppendLine("Responde SIEMPRE en español, de forma breve, clara y amable. Si no sabes algo, dilo con honestidad.");
            sb.AppendLine($"Fecha de hoy: {hoy:yyyy-MM-dd}.");
            sb.AppendLine();

            sb.AppendLine("CURSOS DISPONIBLES:");
            if (cursos.Any())
                foreach (var c in cursos)
                    sb.AppendLine($"- {c.Nombre} ({c.Docente}), {c.HoraInicio:hh\\:mm}-{c.HoraFin:hh\\:mm}, Aula {c.CodigoAula}");
            else
                sb.AppendLine("- (no hay cursos registrados)");
            sb.AppendLine();

            sb.AppendLine("EVENTOS PRÓXIMOS:");
            if (eventos.Any())
                foreach (var e in eventos)
                    sb.AppendLine($"- {e.Titulo} ({e.TipoEvento}) el {e.Fecha:yyyy-MM-dd} en {e.Lugar}");
            else
                sb.AppendLine("- (no hay eventos próximos)");
            sb.AppendLine();

            sb.AppendLine("LUGARES RECOMENDADOS:");
            if (lugares.Any())
                foreach (var l in lugares)
                    sb.AppendLine($"- {l.Nombre} ({l.Tipo}), calificación {l.Calificacion}");
            else
                sb.AppendLine("- (no hay lugares registrados)");

            return sb.ToString();
        }
    }

    // ---- Modelos para hablar con la API de Ollama ----
    public class OllamaMessage
    {
        [JsonPropertyName("role")] public string Role { get; set; } = "";
        [JsonPropertyName("content")] public string Content { get; set; } = "";
    }

    public class OllamaChatRequest
    {
        [JsonPropertyName("model")] public string Model { get; set; } = "";
        [JsonPropertyName("messages")] public List<OllamaMessage> Messages { get; set; } = new();
        [JsonPropertyName("stream")] public bool Stream { get; set; }
    }

    public class OllamaChatResponse
    {
        [JsonPropertyName("message")] public OllamaMessage? Message { get; set; }
    }
}