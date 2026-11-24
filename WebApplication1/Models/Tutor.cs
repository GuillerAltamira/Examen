using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class Tutor
    {
        public int Id { get; set; }

        [Required]
        [StringLength(120)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [StringLength(120)]
        public string Especialidad { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        public string Correo { get; set; } = string.Empty;
    }
}