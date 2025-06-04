using Microsoft.AspNetCore.Mvc;
using ExportModule.DTOs;
using ExportModule.Services;

namespace ExportModule.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExportController : ControllerBase
    {
        private readonly IDatosAExportarService _datosAExportarService;

        public ExportController(IDatosAExportarService datosAExportarService)
        {
            _datosAExportarService = datosAExportarService;
        }

        /// <summary>
        /// Obtiene todos los registros de exportación
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DatosAExportarDto>>> GetExportaciones()
        {
            var exportaciones = await _datosAExportarService.GetAllAsync();
            return Ok(exportaciones);
        }

        /// <summary>
        /// Obtiene un registro de exportación por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<DatosAExportarDto>> GetExportacion(int id)
        {
            var exportacion = await _datosAExportarService.GetByIdAsync(id);
            if (exportacion == null)
                return NotFound($"Exportación con ID {id} no encontrada");

            return Ok(exportacion);
        }

        /// <summary>
        /// Exporta datos según los parámetros especificados
        /// </summary>
        [HttpPost("exportar")]
        public async Task<ActionResult<ExportResultDto>> ExportarDatos(ExportRequestDto request)
        {
            var result = await _datosAExportarService.ExportDataAsync(request);
            
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Obtiene exportaciones por tipo de dato
        /// </summary>
        [HttpGet("tipo/{tipoDato}")]
        public async Task<ActionResult<IEnumerable<DatosAExportarDto>>> GetExportacionesByTipo(string tipoDato)
        {
            var exportaciones = await _datosAExportarService.GetByTipoDatoAsync(tipoDato);
            return Ok(exportaciones);
        }

        /// <summary>
        /// Obtiene exportaciones por formato
        /// </summary>
        [HttpGet("formato/{formato}")]
        public async Task<ActionResult<IEnumerable<DatosAExportarDto>>> GetExportacionesByFormato(string formato)
        {
            var exportaciones = await _datosAExportarService.GetByFormatoAsync(formato);
            return Ok(exportaciones);
        }

        /// <summary>
        /// Elimina un registro de exportación
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExportacion(int id)
        {
            var result = await _datosAExportarService.DeleteAsync(id);
            if (!result)
                return NotFound($"Exportación con ID {id} no encontrada");

            return NoContent();
        }
    }
}