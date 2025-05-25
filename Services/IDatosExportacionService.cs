using Export_trace_module.Models;

namespace Export_trace_module.Services;

public interface IDatosExportacionService
{
    Task<DatosAExportar> ExportarDatosAsync(int entidadId, string tipoEntidad, string formato);
    Task<IEnumerable<DatosAExportar>> GetHistorialExportacionesAsync();
    Task<string> GenerarContenidoExportacionAsync(int entidadId, string tipoEntidad, string formato);
    Task<bool> EliminarExportacionAsync(int id);
}