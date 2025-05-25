using Export_trace_module.Models; // Asegúrate de incluir el namespace de tus modelos
using Export_trace_module.Services;

namespace Export_trace_module.Controllers.MinimalAPIs;

public static class ConsultaAPIEndpoints
{
    public static void MapConsultaAPIEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/consultas").WithTags("Consultas API");

        group.MapGet("/", async (IConsultaAPIService service) =>
            await service.GetAllConsultasAsync());

        group.MapGet("/{id}", async (int id, IConsultaAPIService service) =>
            await service.GetConsultaByIdAsync(id) is { } consulta
                ? Results.Ok(consulta)
                : Results.NotFound());

        group.MapPost("/", async (ConsultaAPI consulta, IConsultaAPIService service) =>
        {
            var created = await service.CreateConsultaAsync(consulta.Endpoint);
            return Results.Created($"/api/consultas/{created.Id}", created);
        });
    }
}