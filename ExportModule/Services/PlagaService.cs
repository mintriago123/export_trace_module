using Microsoft.EntityFrameworkCore;
using ExportModule.Data.Context;
using ExportModule.DTOs;
using ExportModule.Models;

namespace ExportModule.Services
{
    public class PlagaService : IPlagaService
    {
        private readonly ApplicationDbContext _context;

        public PlagaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PlagaDto>> GetAllAsync()
        {
            return await _context.Plagas
                .Include(p => p.Cultivo)
                .Include(p => p.ConsultaAPI)
                .Select(p => new PlagaDto
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Nivel = p.Nivel,
                    CultivoId = p.CultivoId,
                    CultivoNombre = p.Cultivo.Nombre,
                    ConsultaAPIId = p.ConsultaAPIId
                })
                .ToListAsync();
        }

        public async Task<PlagaDto?> GetByIdAsync(int id)
        {
            var plaga = await _context.Plagas
                .Include(p => p.Cultivo)
                .Include(p => p.ConsultaAPI)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (plaga == null) return null;

            return new PlagaDto
            {
                Id = plaga.Id,
                Nombre = plaga.Nombre,
                Nivel = plaga.Nivel,
                CultivoId = plaga.CultivoId,
                CultivoNombre = plaga.Cultivo.Nombre,
                ConsultaAPIId = plaga.ConsultaAPIId
            };
        }

        public async Task<PlagaDto> CreateAsync(CreatePlagaDto createDto)
        {
            // Validar que existan el cultivo y la consulta API
            var cultivo = await _context.Cultivos.FindAsync(createDto.CultivoId);
            var consultaAPI = await _context.ConsultasAPI.FindAsync(createDto.ConsultaAPIId);

            if (cultivo == null)
                throw new ArgumentException($"Cultivo con ID {createDto.CultivoId} no existe");
            
            if (consultaAPI == null)
                throw new ArgumentException($"ConsultaAPI con ID {createDto.ConsultaAPIId} no existe");

            var plaga = new Plaga
            {
                Nombre = createDto.Nombre,
                Nivel = createDto.Nivel,
                CultivoId = createDto.CultivoId,
                ConsultaAPIId = createDto.ConsultaAPIId
            };

            _context.Plagas.Add(plaga);
            await _context.SaveChangesAsync();

            return new PlagaDto
            {
                Id = plaga.Id,
                Nombre = plaga.Nombre,
                Nivel = plaga.Nivel,
                CultivoId = plaga.CultivoId,
                CultivoNombre = cultivo.Nombre,
                ConsultaAPIId = plaga.ConsultaAPIId
            };
        }

        public async Task<PlagaDto?> UpdateAsync(int id, CreatePlagaDto updateDto)
        {
            var plaga = await _context.Plagas
                .Include(p => p.Cultivo)
                .FirstOrDefaultAsync(p => p.Id == id);
            
            if (plaga == null) return null;

            plaga.Nombre = updateDto.Nombre;
            plaga.Nivel = updateDto.Nivel;
            plaga.CultivoId = updateDto.CultivoId;
            plaga.ConsultaAPIId = updateDto.ConsultaAPIId;

            await _context.SaveChangesAsync();

            return new PlagaDto
            {
                Id = plaga.Id,
                Nombre = plaga.Nombre,
                Nivel = plaga.Nivel,
                CultivoId = plaga.CultivoId,
                CultivoNombre = plaga.Cultivo.Nombre,
                ConsultaAPIId = plaga.ConsultaAPIId
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var plaga = await _context.Plagas.FindAsync(id);
            if (plaga == null) return false;

            _context.Plagas.Remove(plaga);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<PlagaDto>> GetPlagasByCultivoAsync(int cultivoId)
        {
            return await _context.Plagas
                .Include(p => p.Cultivo)
                .Where(p => p.CultivoId == cultivoId)
                .Select(p => new PlagaDto
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Nivel = p.Nivel,
                    CultivoId = p.CultivoId,
                    CultivoNombre = p.Cultivo.Nombre,
                    ConsultaAPIId = p.ConsultaAPIId
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<PlagaDto>> GetPlagasByNivelAsync(string nivel)
        {
            return await _context.Plagas
                .Include(p => p.Cultivo)
                .Where(p => p.Nivel.ToLower() == nivel.ToLower())
                .Select(p => new PlagaDto
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Nivel = p.Nivel,
                    CultivoId = p.CultivoId,
                    CultivoNombre = p.Cultivo.Nombre,
                    ConsultaAPIId = p.ConsultaAPIId
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<PlagaDto>> GetPlagasCriticasAsync()
        {
            return await _context.Plagas
                .Include(p => p.Cultivo)
                .Where(p => p.Nivel.ToLower() == "alto" || p.Nivel.ToLower() == "crítico")
                .Select(p => new PlagaDto
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Nivel = p.Nivel,
                    CultivoId = p.CultivoId,
                    CultivoNombre = p.Cultivo.Nombre,
                    ConsultaAPIId = p.ConsultaAPIId
                })
                .ToListAsync();
        }
    }
}