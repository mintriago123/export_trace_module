using Microsoft.EntityFrameworkCore;
using Export_trace_module.Data;
using Export_trace_module.Models;

namespace Export_trace_module.Services;

public class ConsultaAPIService : IConsultaAPIService
{
    private readonly ApplicationDbContext _context;

    public ConsultaAPIService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ConsultaAPI> CreateConsultaAsync(string endpoint)
    {
        var consulta = new ConsultaAPI
        {
            Endpoint = endpoint,
            Fecha = DateTime.UtcNow
        };

        _context.ConsultasAPI.Add(consulta);
        await _context.SaveChangesAsync();
        return consulta;
    }

    public async Task<ConsultaAPI?> GetConsultaByIdAsync(int id)
    {
        return await _context.ConsultasAPI
            .Include(c => c.Plagas)
            .Include(c => c.DatosExportados)
            .FirstOrDefaultAsync(c => c.Id == id);
    }


    public async Task<IEnumerable<ConsultaAPI>> GetAllConsultasAsync()
    {
        return await _context.ConsultasAPI.ToListAsync();
    }

    public async Task<bool> DeleteConsultaAsync(int id)
    {
        var consulta = await _context.ConsultasAPI.FindAsync(id);
        if (consulta == null) return false;
        
        _context.ConsultasAPI.Remove(consulta);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<ConsultaAPI> AddPlagasToConsultaAsync(int consultaId, IEnumerable<int> plagaIds)
    {
        var consulta = await _context.ConsultasAPI
            .Include(c => c.Plagas)
            .FirstOrDefaultAsync(c => c.Id == consultaId);
        
        if (consulta == null)
            throw new KeyNotFoundException("Consulta no encontrada");

        // Lógica para agregar plagas
        // (Nota: Esto asume que las plagas ya existen)
        
        await _context.SaveChangesAsync();
        return consulta;
    }
}