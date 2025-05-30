using Export_trace_module.Data;
using Export_trace_module.Models;
using Microsoft.EntityFrameworkCore;

namespace Export_trace_module.Services;

public class DatosExportacionService : IDatosExportacionService
{
    private readonly ApplicationDbContext _context;

    public DatosExportacionService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DatosAExportar> ExportarDatosAsync(int entidadId, string tipoEntidad, string formato)
    {
        var exportacion = new DatosAExportar
        {
            TipoDato = tipoEntidad,
            Formato = formato,
            FechaExportacion = DateTime.UtcNow
        };

        // Asignar el ID correspondiente según el tipo
        switch (tipoEntidad.ToLower())
        {
            case "consultaapi":
                exportacion.ConsultaAPIId = entidadId;
                break;
            case "cultivo":
                exportacion.CultivoId = entidadId;
                break;
            case "plaga":
                exportacion.PlagaId = entidadId;
                break;
        }

        _context.DatosAExportar.Add(exportacion);
        await _context.SaveChangesAsync();
        return exportacion;
    }

    public async Task<IEnumerable<DatosAExportar>> GetHistorialExportacionesAsync()
    {
        return await _context.DatosAExportar
            .OrderByDescending(d => d.FechaExportacion)
            .ToListAsync();
    }

public async Task<string> GenerarContenidoExportacionAsync(int entidadId, string tipoEntidad, string formato)
{
    // Simular operación asíncrona (puedes reemplazar esto con tu lógica real)
    await Task.Delay(1); // Pequeña espera para hacerlo realmente asíncrono
    
    return $"Contenido exportado: {tipoEntidad} {entidadId} en {formato}";
}

    public async Task<bool> EliminarExportacionAsync(int id)
    {
        var exportacion = await _context.DatosAExportar.FindAsync(id);
        if (exportacion == null) return false;
        
        _context.DatosAExportar.Remove(exportacion);
        await _context.SaveChangesAsync();
        return true;
    }}