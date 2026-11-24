using Api.Interfaces;
using Contratos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TesisController : ControllerBase
    {
        private readonly ITesisInterface _tesisService;

        public TesisController(ITesisInterface tesisService)
        {
            _tesisService = tesisService;
        }

        [HttpGet]
        public async Task<ActionResult<List<TesisLecturaDto>>> Get([FromQuery] string? buscar = null)
        {
            var resultado = await _tesisService.BuscarAsync(buscar);
            return Ok(resultado);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TesisLecturaDto>> GetPorId(int id)
        {
            var tesis = await _tesisService.ObtenerPorIdAsync(id);
            if (tesis == null)
                return NotFound("La tesis no existe.");

            return Ok(tesis);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TesisDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (exito, mensaje, tesis) = await _tesisService.CrearAsync(dto);
            if (!exito)
                return BadRequest(mensaje);

            return Ok(tesis);
        }
    }
}