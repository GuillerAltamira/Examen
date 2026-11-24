using Api.Data;

namespace Api.Factores
{
    public static class DatosIniciales
    {
        public static void Sembrar(AppDbContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Tutores.Any())
            {
                var tutores = TutorFactory.CrearDiezTutores();
                context.Tutores.AddRange(tutores);
                context.SaveChanges();
            }

            var tutoresBd = context.Tutores.ToList();
            if (!context.Tesis.Any() && tutoresBd.Count > 0)
            {
                var tesis = TesisFactory.CrearDiezTesis(tutoresBd);
                context.Tesis.AddRange(tesis);
                context.SaveChanges();
            }
        }
    }
}