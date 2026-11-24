using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required]
        [StringLength(80)]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Password { get; set; } = string.Empty;

        [StringLength(30)]
        public string Rol { get; set; } = "Usuario";
    }
}
