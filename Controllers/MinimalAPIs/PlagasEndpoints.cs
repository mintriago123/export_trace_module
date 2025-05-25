using Export_trace_module.Services;

namespace Export_trace_module.Controllers.MinimalAPIs;

public static class PlagasEndpoints
{
    public static void MapPlagasEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/plagas").WithTags("Plagas");

        group.MapGet("/{id}", async (int id, IPlagaAPIService service) =>
            await service.GetPlagaByIdAsync(id) is { } plaga
                ? Results.Ok(plaga)
                : Results.NotFound());

        group.MapGet("/cultivo/{cultivoId}", async (int cultivoId, IPlagaAPIService service) =>
            Results.Ok(await service.GetPlagasPorCultivoAsync(cultivoId)));
    }
}