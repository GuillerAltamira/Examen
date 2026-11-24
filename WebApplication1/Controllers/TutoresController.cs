using Api.Interfaces;
using Contratos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TutoresController : ControllerBase
    {
        private readonly ITutorInterface _tutorService;

        public TutoresController(ITutorInterface tutorService)
        {
            _tutorService = tutorService;
        }

        [HttpGet]
        public async Task<ActionResult<List<TutorLecturaDto>>> Get()
        {
            var lista = await _tutorService.ObtenerTodosAsync();
            return Ok(lista);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TutorDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (exito, mensaje, tutor) = await _tutorService.CrearAsync(dto);
            if (!exito)
                return BadRequest(mensaje);

            return Ok(tutor);
        }
    }
}