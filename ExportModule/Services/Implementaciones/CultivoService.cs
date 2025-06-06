using ExportModule.Data.Context;
using ExportModule.Models;
using ExportModule.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExportModule.Services.Implementaciones
{
    public class CultivoService : ICultivoService
    {
        private readonly AppDbContext _context;

        public CultivoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(bool esExportable, string motivo)> EvaluarCultivoAsync(int cultivoId)
        {
            var cultivo = await _context.Cultivos
                .Include(c => c.Plagas)
                .FirstOrDefaultAsync(c => c.Id == cultivoId);

            if (cultivo == null)
                return (false, "Cultivo no encontrado");

            if (cultivo.Plagas.Any(p => p.Nivel.ToLower() == "crítico" || p.Nivel.ToLower() == "critico"))
                return (false, "El cultivo tiene plagas de nivel crítico");

            if (!cultivo.Plagas.Any())
                return (true, "El cultivo no tiene plagas asociadas");

            return (true, "El cultivo solo tiene plagas de nivel leve o moderado");
        }
    }
}
