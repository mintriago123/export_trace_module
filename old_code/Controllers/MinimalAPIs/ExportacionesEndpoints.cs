using Export_trace_module.Services;

namespace Export_trace_module.Controllers.MinimalAPIs;

public static class ExportacionesEndpoints
{
    public static void MapExportacionesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/exportaciones").WithTags("Exportaciones");

        group.MapGet("/", async (IDatosExportacionService service) =>
            await service.GetHistorialExportacionesAsync());

        group.MapPost("/", async (ExportacionRequest request, IDatosExportacionService service) =>
        {
            var result = await service.ExportarDatosAsync(
                request.EntidadId,
                request.TipoEntidad,
                request.Formato);
            return Results.Created($"/api/exportaciones/{result.Id}", result);
        });
    }
}

public record ExportacionRequest(int EntidadId, string TipoEntidad, string Formato);