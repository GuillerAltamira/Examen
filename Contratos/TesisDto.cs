using System.ComponentModel.DataAnnotations;

namespace Contratos
{
    public class TesisDto
    {
        [Required(ErrorMessage = "El titulo es obligatorio.")]
        [StringLength(200, MinimumLength = 3)]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El estudiante es obligatorio.")]
        [StringLength(120, MinimumLength = 2)]
        public string Estudiante { get; set; } = string.Empty;

        [Required(ErrorMessage = "El estado es obligatorio.")]
        [StringLength(50)]
        public string Estado { get; set; } = "EnCurso";

        public string? Imagen { get; set; }

        [Required(ErrorMessage = "Debe asignar un tutor.")]
        public int IdTutor { get; set; }
    }
}
