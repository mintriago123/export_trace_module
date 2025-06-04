using Microsoft.AspNetCore.Mvc;
using ExportModule.DTOs;
using ExportModule.Services;

namespace ExportModule.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TareasProgramadasController : ControllerBase
    {
        private readonly ITareaProgramadaService _tareaProgramadaService;

        public TareasProgramadasController(ITareaProgramadaService tareaProgramadaService)
        {
            _tareaProgramadaService = tareaProgramadaService;
        }

        /// <summary>
        /// Obtiene todas las tareas programadas
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TareaProgramadaDto>>> GetTareas()
        {
            var tareas = await _tareaProgramadaService.GetAllAsync();
            return Ok(tareas);
        }

        /// <summary>
        /// Obtiene una tarea programada por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<TareaProgramadaDto>> GetTarea(int id)
        {
            var tarea = await _tareaProgramadaService.GetByIdAsync(id);
            if (tarea == null)
                return NotFound($"Tarea programada con ID {id} no encontrada");

            return Ok(tarea);
        }

        /// <summary>
        /// Crea una nueva tarea programada
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<TareaProgramadaDto>> CreateTarea(CreateTareaProgramadaDto createDto)
        {
            var tarea = await _tareaProgramadaService.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetTarea), new { id = tarea.Id }, tarea);
        }

        /// <summary>
        /// Actualiza una tarea programada existente
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<TareaProgramadaDto>> UpdateTarea(int id, UpdateTareaProgramadaDto updateDto)
        {
            var tarea = await _tareaProgramadaService.UpdateAsync(id, updateDto);
            if (tarea == null)
                return NotFound($"Tarea programada con ID {id} no encontrada");

            return Ok(tarea);
        }

        /// <summary>
        /// Elimina una tarea programada
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTarea(int id)
        {
            var result = await _tareaProgramadaService.DeleteAsync(id);
            if (!result)
                return NotFound($"Tarea programada con ID {id} no encontrada");

            return NoContent();
        }

        /// <summary>
        /// Obtiene tareas que pueden ejecutarse ahora
        /// </summary>
        [HttpGet("ejecutables")]
        public async Task<ActionResult<IEnumerable<TareaProgramadaDto>>> GetTareasEjecutables()
        {
            var tareas = await _tareaProgramadaService.GetTareasEjecutablesAsync();
            return Ok(tareas);
        }

        /// <summary>
        /// Obtiene tareas por tipo
        /// </summary>
        [HttpGet("tipo/{tipo}")]
        public async Task<ActionResult<IEnumerable<TareaProgramadaDto>>> GetTareasByTipo(string tipo)
        {
            var tareas = await _tareaProgramadaService.GetTareasByTipoAsync(tipo);
            return Ok(tareas);
        }

        /// <summary>
        /// Ejecuta una tarea programada
        /// </summary>
        [HttpPost("{id}/ejecutar")]
        public async Task<ActionResult> EjecutarTarea(int id)
        {
            var result = await _tareaProgramadaService.ExecuteTaskAsync(id);
            if (!result)
                return BadRequest($"No se pudo ejecutar la tarea con ID {id}");

            return Ok(new { message = "Tarea ejecutada exitosamente", tareaId = id });
        }
    }
}