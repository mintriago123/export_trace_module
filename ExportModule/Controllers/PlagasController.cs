using Microsoft.AspNetCore.Mvc;
using ExportModule.DTOs;
using ExportModule.Services;

namespace ExportModule.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlagasController : ControllerBase
    {
        private readonly IPlagaService _plagaService;

        public PlagasController(IPlagaService plagaService)
        {
            _plagaService = plagaService;
        }

        /// <summary>
        /// Obtiene todas las plagas
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlagaDto>>> GetPlagas()
        {
            var plagas = await _plagaService.GetAllAsync();
            return Ok(plagas);
        }

        /// <summary>
        /// Obtiene una plaga por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<PlagaDto>> GetPlaga(int id)
        {
            var plaga = await _plagaService.GetByIdAsync(id);
            if (plaga == null)
                return NotFound($"Plaga con ID {id} no encontrada");

            return Ok(plaga);
        }

        /// <summary>
        /// Crea una nueva plaga
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<PlagaDto>> CreatePlaga(CreatePlagaDto createDto)
        {
            try
            {
                var plaga = await _plagaService.CreateAsync(createDto);
                return CreatedAtAction(nameof(GetPlaga), new { id = plaga.Id }, plaga);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Actualiza una plaga existente
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<PlagaDto>> UpdatePlaga(int id, CreatePlagaDto updateDto)
        {
            var plaga = await _plagaService.UpdateAsync(id, updateDto);
            if (plaga == null)
                return NotFound($"Plaga con ID {id} no encontrada");

            return Ok(plaga);
        }

        /// <summary>
        /// Elimina una plaga
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlaga(int id)
        {
            var result = await _plagaService.DeleteAsync(id);
            if (!result)
                return NotFound($"Plaga con ID {id} no encontrada");

            return NoContent();
        }

        /// <summary>
        /// Obtiene plagas por cultivo
        /// </summary>
        [HttpGet("cultivo/{cultivoId}")]
        public async Task<ActionResult<IEnumerable<PlagaDto>>> GetPlagasByCultivo(int cultivoId)
        {
            var plagas = await _plagaService.GetPlagasByCultivoAsync(cultivoId);
            return Ok(plagas);
        }

        /// <summary>
        /// Obtiene plagas por nivel de severidad
        /// </summary>
        [HttpGet("nivel/{nivel}")]
        public async Task<ActionResult<IEnumerable<PlagaDto>>> GetPlagasByNivel(string nivel)
        {
            var plagas = await _plagaService.GetPlagasByNivelAsync(nivel);
            return Ok(plagas);
        }

        /// <summary>
        /// Obtiene plagas críticas (Alto y Crítico)
        /// </summary>
        [HttpGet("criticas")]
        public async Task<ActionResult<IEnumerable<PlagaDto>>> GetPlagasCriticas()
        {
            var plagas = await _plagaService.GetPlagasCriticasAsync();
            return Ok(plagas);
        }
    }
}