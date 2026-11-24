using System.ComponentModel.DataAnnotations;

namespace Contratos
{
    public class RegistroDto
    {
        [Required(ErrorMessage = "El usuario es obligatorio.")]
        [StringLength(80, MinimumLength = 3)]
        public string Usuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(100, MinimumLength = 3)]
        public string Password { get; set; } = string.Empty;
    }
}
