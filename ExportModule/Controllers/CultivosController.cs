using Microsoft.AspNetCore.Mvc;
using ExportModule.DTOs;
using ExportModule.Services;

namespace ExportModule.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CultivosController : ControllerBase
    {
        private readonly ICultivoService _cultivoService;

        public CultivosController(ICultivoService cultivoService)
        {
            _cultivoService = cultivoService;
        }

        /// <summary>
        /// Obtiene todos los cultivos
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CultivoDto>>> GetCultivos()
        {
            var cultivos = await _cultivoService.GetAllAsync();
            return Ok(cultivos);
        }

        /// <summary>
        /// Obtiene un cultivo por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<CultivoDto>> GetCultivo(int id)
        {
            var cultivo = await _cultivoService.GetByIdAsync(id);
            if (cultivo == null)
                return NotFound($"Cultivo con ID {id} no encontrado");

            return Ok(cultivo);
        }

        /// <summary>
        /// Crea un nuevo cultivo
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<CultivoDto>> CreateCultivo(CreateCultivoDto createDto)
        {
            var cultivo = await _cultivoService.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetCultivo), new { id = cultivo.Id }, cultivo);
        }

        /// <summary>
        /// Actualiza un cultivo existente
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<CultivoDto>> UpdateCultivo(int id, UpdateCultivoDto updateDto)
        {
            var cultivo = await _cultivoService.UpdateAsync(id, updateDto);
            if (cultivo == null)
                return NotFound($"Cultivo con ID {id} no encontrado");

            return Ok(cultivo);
        }

        /// <summary>
        /// Elimina un cultivo
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCultivo(int id)
        {
            var result = await _cultivoService.DeleteAsync(id);
            if (!result)
                return NotFound($"Cultivo con ID {id} no encontrado");

            return NoContent();
        }

        /// <summary>
        /// Obtiene cultivos afectados por plagas
        /// </summary>
        [HttpGet("afectados-por-plagas")]
        public async Task<ActionResult<IEnumerable<CultivoDto>>> GetCultivosAfectados()
        {
            var cultivos = await _cultivoService.GetCultivosAfectadosPorPlagasAsync();
            return Ok(cultivos);
        }

        /// <summary>
        /// Verifica si un cultivo debe ser exportado
        /// </summary>
        [HttpGet("{id}/should-export")]
        public async Task<ActionResult<bool>> ShouldExport(int id)
        {
            var shouldExport = await _cultivoService.ShouldExportCultivoAsync(id);
            return Ok(new { cultivoId = id, shouldExport });
        }
    }
}