using Api.Models;

namespace Api.Factores
{
    public static class TesisFactory
    {
        public static List<Tesis> CrearDiezTesis(List<Tutor> tutores)
        {
            var estados = new[] { "EnCurso", "Aprobada", "Revision" };
            var tesisLista = new List<Tesis>();

            if (tutores.Count == 0)
                return tesisLista;

            
            for (int i = 1; i <= 10; i++)
            {
                var tutorAsignado = tutores[(i - 1) % tutores.Count];

                tesisLista.Add(new Tesis
                {
                    Titulo = $"Sistema de Gestion Tesis {i}",
                    Estudiante = $"Estudiante Prueba {i}",
                    Estado = estados[(i - 1) % estados.Length],
                    Imagen = null,
                    IdTutor = tutorAsignado.Id
                });
            }

            return tesisLista;
        }
    }
}