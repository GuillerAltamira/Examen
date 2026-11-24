using Contratos;

namespace Api.Interfaces
{
    public interface ITutorInterface
    {
        Task<List<TutorLecturaDto>> ObtenerTodosAsync();
        Task<(bool Exito, string Mensaje, TutorLecturaDto? Tutor)> CrearAsync(TutorDto dto);
    }
}