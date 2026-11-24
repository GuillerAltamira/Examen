using System.Net.Http.Json;
using Contratos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public LoginModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public LoginDto Credenciales { get; set; } = new();

        public string? ErrorMensaje { get; set; }
        public bool EstaLogueado => !string.IsNullOrWhiteSpace(HttpContext.Session.GetString("JwtToken"));

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var client = _httpClientFactory.CreateClient("ExamenApi");
            var response = await client.PostAsJsonAsync("api/Auth/login", Credenciales);

            if (!response.IsSuccessStatusCode)
            {
                ErrorMensaje = "Usuario o contraseña incorrectos.";
                return Page();
            }

            var data = await response.Content.ReadFromJsonAsync<LoginRespuestaDto>();
            if (data == null || string.IsNullOrWhiteSpace(data.Token))
            {
                ErrorMensaje = "No se recibió el token.";
                return Page();
            }

            HttpContext.Session.SetString("JwtToken", data.Token);
            return RedirectToPage("/Tesis/Index");
        }

        public IActionResult OnPostLogout()
        {
            HttpContext.Session.Remove("JwtToken");
            return RedirectToPage("/Login");
        }
    }
}
