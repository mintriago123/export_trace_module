using ExportModule.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExportModule.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EvaluacionController : ControllerBase
    {
        private readonly ICultivoService _cultivoService;

        public EvaluacionController(ICultivoService cultivoService)
        {
            _cultivoService = cultivoService;
        }

        [HttpGet("evaluar/{cultivoId}")]
        public async Task<IActionResult> EvaluarCultivo(int cultivoId)
        {
            var (esExportable, motivo) = await _cultivoService.EvaluarCultivoAsync(cultivoId);

            return Ok(new
            {
                apto_para_exportacion = esExportable,
                motivo
            });
        }
    }
}
