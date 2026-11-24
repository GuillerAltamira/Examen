using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Contratos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.Pages.Tesis
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IWebHostEnvironment _environment;

        public IndexModel(IHttpClientFactory httpClientFactory, IWebHostEnvironment environment)
        {
            _httpClientFactory = httpClientFactory;
            _environment = environment;
        }

        public List<TesisLecturaDto> ListaTesis { get; set; } = new();
        public List<SelectListItem> OpcionesTutores { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? Buscar { get; set; }

        [BindProperty]
        public TesisDto NuevaTesis { get; set; } = new();

        [BindProperty]
        public IFormFile? ArchivoImagen { get; set; }

        public string? Mensaje { get; set; }

        private HttpClient? CrearCliente()
        {
            var token = HttpContext.Session.GetString("JwtToken");
            if (string.IsNullOrWhiteSpace(token))
                return null;

            var client = _httpClientFactory.CreateClient("ExamenApi");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return client;
        }

        private async Task<bool> CargarAsync(HttpClient client)
        {
            var respTutores = await client.GetAsync("api/Tutores");
            if (respTutores.StatusCode == HttpStatusCode.Unauthorized)
                return false;

            if (respTutores.IsSuccessStatusCode)
            {
                var tutores = await respTutores.Content.ReadFromJsonAsync<List<TutorLecturaDto>>() ?? new();
                OpcionesTutores = tutores.Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = $"{t.Nombre} ({t.Especialidad})"
                }).ToList();
            }

            var url = string.IsNullOrWhiteSpace(Buscar)
                ? "api/Tesis"
                : $"api/Tesis?buscar={Uri.EscapeDataString(Buscar.Trim())}";

            var respTesis = await client.GetAsync(url);
            if (respTesis.StatusCode == HttpStatusCode.Unauthorized)
                return false;

            if (respTesis.IsSuccessStatusCode)
                ListaTesis = await respTesis.Content.ReadFromJsonAsync<List<TesisLecturaDto>>() ?? new();

            return true;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var client = CrearCliente();
            if (client == null)
                return RedirectToPage("/Login");

            if (!await CargarAsync(client))
                return RedirectToPage("/Login");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var client = CrearCliente();
            if (client == null)
                return RedirectToPage("/Login");

            if (ArchivoImagen != null && ArchivoImagen.Length > 0)
            {
                var carpeta = Path.Combine(_environment.WebRootPath, "portadas");
                Directory.CreateDirectory(carpeta);
                var ext = Path.GetExtension(ArchivoImagen.FileName);
                var nombreArchivo = $"{Guid.NewGuid()}{ext}";
                var ruta = Path.Combine(carpeta, nombreArchivo);

                using var stream = new FileStream(ruta, FileMode.Create);
                await ArchivoImagen.CopyToAsync(stream);

                NuevaTesis.Imagen = $"/portadas/{nombreArchivo}";
            }

            var resp = await client.PostAsJsonAsync("api/Tesis", NuevaTesis);
            if (resp.StatusCode == HttpStatusCode.Unauthorized)
                return RedirectToPage("/Login");

            if (resp.IsSuccessStatusCode)
            {
                Mensaje = "Tesis registrada correctamente.";
                NuevaTesis = new TesisDto();
                ArchivoImagen = null;
            }
            else
            {
                var err = await resp.Content.ReadAsStringAsync();
                Mensaje = string.IsNullOrWhiteSpace(err) ? "No se pudo registrar la tesis." : err.Trim('"');
            }

            if (!await CargarAsync(client))
                return RedirectToPage("/Login");

            return Page();
        }
    }
}
