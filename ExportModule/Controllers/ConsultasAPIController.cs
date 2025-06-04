using Microsoft.AspNetCore.Mvc;
using ExportModule.DTOs;
using ExportModule.Services;

namespace ExportModule.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsultasAPIController : ControllerBase
    {
        private readonly IConsultaAPIService _consultaAPIService;

        public ConsultasAPIController(IConsultaAPIService consultaAPIService)
        {
            _consultaAPIService = consultaAPIService;
        }

        /// <summary>
        /// Obtiene todas las consultas API
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConsultaAPIDto>>> GetConsultas()
        {
            var consultas = await _consultaAPIService.GetAllAsync();
            return Ok(consultas);
        }

        /// <summary>
        /// Obtiene una consulta API por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ConsultaAPIDto>> GetConsulta(int id)
        {
            var consulta = await _consultaAPIService.GetByIdAsync(id);
            if (consulta == null)
                return NotFound($"Consulta API con ID {id} no encontrada");

            return Ok(consulta);
        }

        /// <summary>
        /// Crea una nueva consulta API
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ConsultaAPIDto>> CreateConsulta(CreateConsultaAPIDto createDto)
        {
            var consulta = await _consultaAPIService.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetConsulta), new { id = consulta.Id }, consulta);
        }

        /// <summary>
        /// Actualiza una consulta API existente
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<ConsultaAPIDto>> UpdateConsulta(int id, UpdateConsultaAPIDto updateDto)
        {
            var consulta = await _consultaAPIService.UpdateAsync(id, updateDto);
            if (consulta == null)
                return NotFound($"Consulta API con ID {id} no encontrada");

            return Ok(consulta);
        }

        /// <summary>
        /// Elimina una consulta API
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConsulta(int id)
        {
            var result = await _consultaAPIService.DeleteAsync(id);
            if (!result)
                return NotFound($"Consulta API con ID {id} no encontrada");

            return NoContent();
        }

        /// <summary>
        /// Obtiene consultas por fecha
        /// </summary>
        [HttpGet("fecha/{fecha}")]
        public async Task<ActionResult<IEnumerable<ConsultaAPIDto>>> GetConsultasByFecha(DateTime fecha)
        {
            var consultas = await _consultaAPIService.GetConsultasByFechaAsync(fecha);
            return Ok(consultas);
        }

        /// <summary>
        /// Obtiene consultas que tienen plagas registradas
        /// </summary>
        [HttpGet("con-plagas")]
        public async Task<ActionResult<IEnumerable<ConsultaAPIDto>>> GetConsultasConPlagas()
        {
            var consultas = await _consultaAPIService.GetConsultasConPlagasAsync();
            return Ok(consultas);
        }
    }
}