namespace Contratos
{
    public class TesisLecturaDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Estudiante { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string? Imagen { get; set; }
        public int IdTutor { get; set; }
        public string TutorNombre { get; set; } = string.Empty;
        public string TutorEspecialidad { get; set; } = string.Empty;
        public string TutorCorreo { get; set; } = string.Empty;
    }
}
