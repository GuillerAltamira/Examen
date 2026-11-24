using Api.Models;

namespace Api.Factores
{
    public static class TutorFactory
    {
        public static List<Tutor> CrearDiezTutores()
        {
            var especialidades = new[]
            {
                "Ingenieria de Sistemas",
                "Redes y Telecomunicaciones",
                "Bases de Datos e IA",
                "Desarrollo de Software",
                "Seguridad Informatica"
            };

            var tutores = new List<Tutor>();

            for (int i = 1; i <= 10; i++)
            {
                tutores.Add(new Tutor
                {
                    Nombre = $"Tutor Prueba {i}",
                    Especialidad = especialidades[(i - 1) % especialidades.Length],
                    Correo = $"tutor{i}@upds.edu.bo"
                });
            }

            return tutores;
        }
    }
}
