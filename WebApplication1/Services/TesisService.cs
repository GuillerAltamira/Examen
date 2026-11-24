using Api.Data;
using Api.Interfaces;
using Api.Models;
using Contratos;
using Microsoft.EntityFrameworkCore;

namespace Api.Services
{
    public class TesisService : ITesisInterface
    {
        private readonly AppDbContext _context;

        public TesisService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<TesisLecturaDto>> BuscarAsync(string? buscar = null)
        {
            var query = _context.Tesis
                .Include(t => t.Tutor)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(buscar))
            {
                var termino = buscar.Trim().ToLower();
                query = query.Where(t =>
                    t.Titulo.ToLower().Contains(termino) ||
                    t.Estudiante.ToLower().Contains(termino) ||
                    (t.Tutor != null && t.Tutor.Nombre.ToLower().Contains(termino)));
            }

            return await query
                .OrderByDescending(t => t.Id)
                .Select(t => new TesisLecturaDto
                {
                    Id = t.Id,
                    Titulo = t.Titulo,
                    Estudiante = t.Estudiante,
                    Estado = t.Estado,
                    Imagen = t.Imagen,
                    IdTutor = t.IdTutor,
                    TutorNombre = t.Tutor != null ? t.Tutor.Nombre : string.Empty,
                    TutorEspecialidad = t.Tutor != null ? t.Tutor.Especialidad : string.Empty,
                    TutorCorreo = t.Tutor != null ? t.Tutor.Correo : string.Empty
                })
                .ToListAsync();
        }

        public async Task<TesisLecturaDto?> ObtenerPorIdAsync(int id)
        {
            return await _context.Tesis
                .Include(t => t.Tutor)
                .Where(t => t.Id == id)
                .Select(t => new TesisLecturaDto
                {
                    Id = t.Id,
                    Titulo = t.Titulo,
                    Estudiante = t.Estudiante,
                    Estado = t.Estado,
                    Imagen = t.Imagen,
                    IdTutor = t.IdTutor,
                    TutorNombre = t.Tutor != null ? t.Tutor.Nombre : string.Empty,
                    TutorEspecialidad = t.Tutor != null ? t.Tutor.Especialidad : string.Empty,
                    TutorCorreo = t.Tutor != null ? t.Tutor.Correo : string.Empty
                })
                .FirstOrDefaultAsync();
        }

        public async Task<(bool Exito, string Mensaje, TesisLecturaDto? Tesis)> CrearAsync(TesisDto dto)
        {
            var tituloLimpio = dto.Titulo.Trim();
            var estudianteLimpio = dto.Estudiante.Trim();
            var estadoLimpio = string.IsNullOrWhiteSpace(dto.Estado) ? "EnCurso" : dto.Estado.Trim();

            var tituloDuplicado = await _context.Tesis
                .AnyAsync(t => t.Titulo.ToLower() == tituloLimpio.ToLower());
            if (tituloDuplicado)
                return (false, $"Ya existe una tesis registrada con el título '{tituloLimpio}'.", null);

            var tutor = await _context.Tutores.FindAsync(dto.IdTutor);
            if (tutor == null)
                return (false, "El tutor seleccionado no existe.", null);

            var cantidadTesisTutor = await _context.Tesis
                .CountAsync(t => t.IdTutor == dto.IdTutor);
            if (cantidadTesisTutor >= 3)
                return (false, $"El tutor '{tutor.Nombre}' ya tiene el máximo de 3 tesis asignadas.", null);

            var nuevaTesis = new Tesis
            {
                Titulo = tituloLimpio,
                Estudiante = estudianteLimpio,
                Estado = estadoLimpio,
                Imagen = string.IsNullOrWhiteSpace(dto.Imagen) ? null : dto.Imagen.Trim(),
                IdTutor = dto.IdTutor
            };

            _context.Tesis.Add(nuevaTesis);
            await _context.SaveChangesAsync();

            return (true, "Tesis registrada correctamente.", new TesisLecturaDto
            {
                Id = nuevaTesis.Id,
                Titulo = nuevaTesis.Titulo,
                Estudiante = nuevaTesis.Estudiante,
                Estado = nuevaTesis.Estado,
                Imagen = nuevaTesis.Imagen,
                IdTutor = nuevaTesis.IdTutor,
                TutorNombre = tutor.Nombre,
                TutorEspecialidad = tutor.Especialidad,
                TutorCorreo = tutor.Correo
            });
        }
    }
}