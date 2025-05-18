using Export_trace_module.Services.ExternalAPIs;

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

        // Cachear respuestas si es apropiado
        group.MapGet("/cached", async (ICultivoAPIService service) =>
            await service.GetAllCultivosAsync())
            .CacheOutput(p => p.Expire(TimeSpan.FromMinutes(5)));
    }
}