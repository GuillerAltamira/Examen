using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Contratos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Tutores
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public List<TutorLecturaDto> Tutores { get; set; } = new();

        [BindProperty]
        public TutorDto NuevoTutor { get; set; } = new();

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

        public async Task<IActionResult> OnGetAsync()
        {
            var client = CrearCliente();
            if (client == null)
                return RedirectToPage("/Login");

            var resp = await client.GetAsync("api/Tutores");
            if (resp.StatusCode == HttpStatusCode.Unauthorized)
                return RedirectToPage("/Login");

            if (resp.IsSuccessStatusCode)
                Tutores = await resp.Content.ReadFromJsonAsync<List<TutorLecturaDto>>() ?? new();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var client = CrearCliente();
            if (client == null)
                return RedirectToPage("/Login");

            var resp = await client.PostAsJsonAsync("api/Tutores", NuevoTutor);
            if (resp.StatusCode == HttpStatusCode.Unauthorized)
                return RedirectToPage("/Login");

            if (resp.IsSuccessStatusCode)
            {
                Mensaje = "Tutor registrado correctamente.";
                NuevoTutor = new TutorDto();
            }
            else
            {
                var err = await resp.Content.ReadAsStringAsync();
                Mensaje = string.IsNullOrWhiteSpace(err) ? "No se pudo registrar el tutor." : err.Trim('"');
            }

            var lista = await client.GetAsync("api/Tutores");
            if (lista.IsSuccessStatusCode)
                Tutores = await lista.Content.ReadFromJsonAsync<List<TutorLecturaDto>>() ?? new();

            return Page();
        }
    }
}