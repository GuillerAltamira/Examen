using System.ComponentModel.DataAnnotations;

namespace Contratos
{
    public class TutorDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(120, MinimumLength = 2)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La especialidad es obligatoria.")]
        [StringLength(120, MinimumLength = 2)]
        public string Especialidad { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "Correo no valido.")]
        [StringLength(150)]
        public string Correo { get; set; } = string.Empty;
    }
}