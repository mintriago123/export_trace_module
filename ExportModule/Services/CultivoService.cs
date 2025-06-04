using Microsoft.EntityFrameworkCore;
using ExportModule.Data.Context;
using ExportModule.DTOs;
using ExportModule.Models;

namespace ExportModule.Services
{
    public class CultivoService : ICultivoService
    {
        private readonly ApplicationDbContext _context;

        public CultivoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CultivoDto>> GetAllAsync()
        {
            return await _context.Cultivos
                .Include(c => c.Plagas)
                .Select(c => new CultivoDto
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Tipo = c.Tipo,
                    Plagas = c.Plagas.Select(p => new PlagaDto
                    {
                        Id = p.Id,
                        Nombre = p.Nombre,
                        Nivel = p.Nivel,
                        CultivoId = p.CultivoId,
                        ConsultaAPIId = p.ConsultaAPIId
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<CultivoDto?> GetByIdAsync(int id)
        {
            var cultivo = await _context.Cultivos
                .Include(c => c.Plagas)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cultivo == null) return null;

            return new CultivoDto
            {
                Id = cultivo.Id,
                Nombre = cultivo.Nombre,
                Tipo = cultivo.Tipo,
                Plagas = cultivo.Plagas.Select(p => new PlagaDto
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Nivel = p.Nivel,
                    CultivoId = p.CultivoId,
                    ConsultaAPIId = p.ConsultaAPIId
                }).ToList()
            };
        }

        public async Task<CultivoDto> CreateAsync(CreateCultivoDto createDto)
        {
            var cultivo = new Cultivo
            {
                Nombre = createDto.Nombre,
                Tipo = createDto.Tipo
            };

            _context.Cultivos.Add(cultivo);
            await _context.SaveChangesAsync();

            return new CultivoDto
            {
                Id = cultivo.Id,
                Nombre = cultivo.Nombre,
                Tipo = cultivo.Tipo,
                Plagas = new List<PlagaDto>()
            };
        }

        public async Task<CultivoDto?> UpdateAsync(int id, UpdateCultivoDto updateDto)
        {
            var cultivo = await _context.Cultivos.FindAsync(id);
            if (cultivo == null) return null;

            if (!string.IsNullOrEmpty(updateDto.Nombre))
                cultivo.Nombre = updateDto.Nombre;
            
            if (!string.IsNullOrEmpty(updateDto.Tipo))
                cultivo.Tipo = updateDto.Tipo;

            await _context.SaveChangesAsync();

            return new CultivoDto
            {
                Id = cultivo.Id,
                Nombre = cultivo.Nombre,
                Tipo = cultivo.Tipo
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var cultivo = await _context.Cultivos.FindAsync(id);
            if (cultivo == null) return false;

            _context.Cultivos.Remove(cultivo);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<CultivoDto>> GetCultivosAfectadosPorPlagasAsync()
        {
            return await _context.Cultivos
                .Include(c => c.Plagas)
                .Where(c => c.Plagas.Any())
                .Select(c => new CultivoDto
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Tipo = c.Tipo,
                    Plagas = c.Plagas.Select(p => new PlagaDto
                    {
                        Id = p.Id,
                        Nombre = p.Nombre,
                        Nivel = p.Nivel,
                        CultivoId = p.CultivoId,
                        ConsultaAPIId = p.ConsultaAPIId
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<bool> ShouldExportCultivoAsync(int cultivoId)
        {
            var cultivo = await _context.Cultivos
                .Include(c => c.Plagas)
                .FirstOrDefaultAsync(c => c.Id == cultivoId);

            if (cultivo == null) return false;

            // Lógica: No exportar si tiene plagas de nivel "Alto" o "Crítico"
            var tienePlagasGraves = cultivo.Plagas
                .Any(p => p.Nivel.ToLower() == "alto" || p.Nivel.ToLower() == "crítico");

            return !tienePlagasGraves;
        }
    }
}