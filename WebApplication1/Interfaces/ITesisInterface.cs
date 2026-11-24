using Contratos;

namespace Api.Interfaces
{
    public interface ITesisInterface
    {
        Task<List<TesisLecturaDto>> BuscarAsync(string? buscar = null);
        Task<TesisLecturaDto?> ObtenerPorIdAsync(int id);
        Task<(bool Exito, string Mensaje, TesisLecturaDto? Tesis)> CrearAsync(TesisDto dto);
    }
}