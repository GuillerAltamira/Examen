using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Api.Data;
using Api.Models;
using Contratos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistroDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var nombreLimpio = dto.Usuario.Trim();
            var nombreLower = nombreLimpio.ToLower();

            var usuarioAdmin = (_configuration["AdminPorDefecto:Usuario"] ?? "admin").Trim().ToLower();
            var existeEnBd = await _context.Usuarios.AnyAsync(u => u.NombreUsuario.ToLower() == nombreLower);

            if (nombreLower == usuarioAdmin || existeEnBd)
                return BadRequest($"El usuario '{nombreLimpio}' ya existe.");

            var nuevoUsuario = new Usuario
            {
                NombreUsuario = nombreLimpio,
                Password = dto.Password,
                Rol = "Usuario"
            };

            _context.Usuarios.Add(nuevoUsuario);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = $"Usuario '{nuevoUsuario.NombreUsuario}' registrado correctamente." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usuarioIngresado = dto.Usuario.Trim();
            var usuarioAdmin = _configuration["AdminPorDefecto:Usuario"] ?? "admin";
            var passwordAdmin = _configuration["AdminPorDefecto:Password"] ?? "123";

            string? rolAsignado = null;

            if (usuarioIngresado.Equals(usuarioAdmin, StringComparison.OrdinalIgnoreCase) && dto.Password == passwordAdmin)
            {
                rolAsignado = "Admin";
            }
            else
            {
                var usuarioBd = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.NombreUsuario.ToLower() == usuarioIngresado.ToLower()
                                           && u.Password == dto.Password);
                if (usuarioBd != null)
                {
                    rolAsignado = usuarioBd.Rol;
                    usuarioIngresado = usuarioBd.NombreUsuario;
                }
            }

            if (rolAsignado == null)
                return Unauthorized("Usuario o contraseña incorrectos.");

            var jwtKey = _configuration["Jwt:Key"]!;
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var expireMinutes = int.TryParse(_configuration["Jwt:ExpireMinutes"], out var mins) ? mins : 120;

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, usuarioIngresado),
                new Claim(ClaimTypes.Role, rolAsignado)
            };

            var expiracion = DateTime.UtcNow.AddMinutes(expireMinutes);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expiracion,
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new LoginRespuestaDto
            {
                Token = tokenString,
                Usuario = usuarioIngresado,
                Expiracion = expiracion
            });
        }
    }
}