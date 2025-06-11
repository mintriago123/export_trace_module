using Export_trace_module.Services; // Usa el namespace correcto donde está tu interfaz
using Export_trace_module.Models;  // Para la clase Cultivo

namespace Export_trace_module.Controllers.MinimalAPIs;

public static class CultivosEndpoints
{
    public static void MapCultivosEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/cultivos").WithTags("Cultivos");

        group.MapGet("/", async (ICultivoAPIService service) =>
            await service.GetAllCultivosAsync());

        group.MapGet("/{id}", async (int id, ICultivoAPIService service) =>
        {
            var cultivo = await service.GetCultivoByIdAsync(id);
            return cultivo is not null ? Results.Ok(cultivo) : Results.NotFound();
        });

        group.MapGet("/buscar/{criterio}", async (string criterio, ICultivoAPIService service) =>
            await service.BuscarCultivosAsync(criterio));

        // Cachear respuestas si es apropiado
        group.MapGet("/cached", async (ICultivoAPIService service) =>
            await service.GetAllCultivosAsync())
            .CacheOutput(p => p.Expire(TimeSpan.FromMinutes(5)));
    }
}