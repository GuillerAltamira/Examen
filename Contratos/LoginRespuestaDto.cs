namespace Contratos
{
    public class LoginRespuestaDto
    {
        public string Token { get; set; } = string.Empty;
        public string Usuario { get; set; } = string.Empty;
        public DateTime Expiracion { get; set; }
    }
}