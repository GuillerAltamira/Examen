using Api.Data;
using Api.Interfaces;
using Api.Models;
using Contratos;
using Microsoft.EntityFrameworkCore;

namespace Api.Services
{
    public class TutorService : ITutorInterface
    {
        private readonly AppDbContext _context;

        public TutorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<TutorLecturaDto>> ObtenerTodosAsync()
        {
            return await _context.Tutores
                .OrderBy(t => t.Nombre)
                .Select(t => new TutorLecturaDto
                {
                    Id = t.Id,
                    Nombre = t.Nombre,
                    Especialidad = t.Especialidad,
                    Correo = t.Correo
                })
                .ToListAsync();
        }

        public async Task<(bool Exito, string Mensaje, TutorLecturaDto? Tutor)> CrearAsync(TutorDto dto)
        {
            var nombreLimpio = dto.Nombre.Trim();
            var especialidadLimpia = dto.Especialidad.Trim();
            var correoLimpio = dto.Correo.Trim().ToLower();

            var existeCorreo = await _context.Tutores
                .AnyAsync(t => t.Correo.ToLower() == correoLimpio);
            if (existeCorreo)
                return (false, $"Ya existe un tutor registrado con el correo '{dto.Correo.Trim()}'.", null);

            var existeNombre = await _context.Tutores
                .AnyAsync(t => t.Nombre.ToLower() == nombreLimpio.ToLower());
            if (existeNombre)
                return (false, $"Ya existe un tutor registrado con el nombre '{nombreLimpio}'.", null);

            var tutor = new Tutor
            {
                Nombre = nombreLimpio,
                Especialidad = especialidadLimpia,
                Correo = correoLimpio
            };

            _context.Tutores.Add(tutor);
            await _context.SaveChangesAsync();

            return (true, "Tutor registrado correctamente.", new TutorLecturaDto
            {
                Id = tutor.Id,
                Nombre = tutor.Nombre,
                Especialidad = tutor.Especialidad,
                Correo = tutor.Correo
            });
        }
    }
}