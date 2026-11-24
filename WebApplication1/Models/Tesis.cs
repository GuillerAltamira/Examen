using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    public class Tesis
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Titulo { get; set; } = string.Empty;

        [Required]
        [StringLength(120)]
        public string Estudiante { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Estado { get; set; } = "EnCurso";

        public string? Imagen { get; set; }

        public int IdTutor { get; set; }

        [ForeignKey(nameof(IdTutor))]
        public Tutor? Tutor { get; set; }
    }
}