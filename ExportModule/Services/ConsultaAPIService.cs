using Microsoft.EntityFrameworkCore;
using ExportModule.Data.Context;
using ExportModule.DTOs;
using ExportModule.Models;

namespace ExportModule.Services
{
    public class ConsultaAPIService : IConsultaAPIService
    {
        private readonly ApplicationDbContext _context;

        public ConsultaAPIService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ConsultaAPIDto>> GetAllAsync()
        {
            return await _context.ConsultasAPI
                .Include(c => c.Plagas)
                    .ThenInclude(p => p.Cultivo)
                .Include(c => c.DatosExportados)
                .Select(c => new ConsultaAPIDto
                {
                    Id = c.Id,
                    Endpoint = c.Endpoint,
                    Fecha = c.Fecha,
                    Plagas = c.Plagas.Select(p => new PlagaDto
                    {
                        Id = p.Id,
                        Nombre = p.Nombre,
                        Nivel = p.Nivel,
                        CultivoId = p.CultivoId,
                        CultivoNombre = p.Cultivo.Nombre,
                        ConsultaAPIId = p.ConsultaAPIId
                    }).ToList(),
                    DatosExportados = c.DatosExportados.Select(d => new DatosAExportarDto
                    {
                        Id = d.Id,
                        FechaExportacion = d.FechaExportacion,
                        TipoDato = d.TipoDato,
                        Formato = d.Formato,
                        ConsultaAPIId = d.ConsultaAPIId
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<ConsultaAPIDto?> GetByIdAsync(int id)
        {
            var consulta = await _context.ConsultasAPI
                .Include(c => c.Plagas)
                    .ThenInclude(p => p.Cultivo)
                .Include(c => c.DatosExportados)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (consulta == null) return null;

            return new ConsultaAPIDto
            {
                Id = consulta.Id,
                Endpoint = consulta.Endpoint,
                Fecha = consulta.Fecha,
                Plagas = consulta.Plagas.Select(p => new PlagaDto
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Nivel = p.Nivel,
                    CultivoId = p.CultivoId,
                    CultivoNombre = p.Cultivo.Nombre,
                    ConsultaAPIId = p.ConsultaAPIId
                }).ToList(),
                DatosExportados = consulta.DatosExportados.Select(d => new DatosAExportarDto
                {
                    Id = d.Id,
                    FechaExportacion = d.FechaExportacion,
                    TipoDato = d.TipoDato,
                    Formato = d.Formato,
                    ConsultaAPIId = d.ConsultaAPIId
                }).ToList()
            };
        }

        public async Task<ConsultaAPIDto> CreateAsync(CreateConsultaAPIDto createDto)
        {
            var consulta = new ConsultaAPI
            {
                Endpoint = createDto.Endpoint,
                Fecha = createDto.Fecha ?? DateTime.Now
            };

            _context.ConsultasAPI.Add(consulta);
            await _context.SaveChangesAsync();

            return new ConsultaAPIDto
            {
                Id = consulta.Id,
                Endpoint = consulta.Endpoint,
                Fecha = consulta.Fecha,
                Plagas = new List<PlagaDto>(),
                DatosExportados = new List<DatosAExportarDto>()
            };
        }

        public async Task<ConsultaAPIDto?> UpdateAsync(int id, UpdateConsultaAPIDto updateDto)
        {
            var consulta = await _context.ConsultasAPI.FindAsync(id);
            if (consulta == null) return null;

            if (!string.IsNullOrEmpty(updateDto.Endpoint))
                consulta.Endpoint = updateDto.Endpoint;
            
            if (updateDto.Fecha.HasValue)
                consulta.Fecha = updateDto.Fecha.Value;

            await _context.SaveChangesAsync();

            return new ConsultaAPIDto
            {
                Id = consulta.Id,
                Endpoint = consulta.Endpoint,
                Fecha = consulta.Fecha
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var consulta = await _context.ConsultasAPI.FindAsync(id);
            if (consulta == null) return false;

            _context.ConsultasAPI.Remove(consulta);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ConsultaAPIDto>> GetConsultasByFechaAsync(DateTime fecha)
        {
            return await _context.ConsultasAPI
                .Where(c => c.Fecha.Date == fecha.Date)
                .Select(c => new ConsultaAPIDto
                {
                    Id = c.Id,
                    Endpoint = c.Endpoint,
                    Fecha = c.Fecha
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<ConsultaAPIDto>> GetConsultasConPlagasAsync()
        {
            return await _context.ConsultasAPI
                .Include(c => c.Plagas)
                    .ThenInclude(p => p.Cultivo)
                .Where(c => c.Plagas.Any())
                .Select(c => new ConsultaAPIDto
                {
                    Id = c.Id,
                    Endpoint = c.Endpoint,
                    Fecha = c.Fecha,
                    Plagas = c.Plagas.Select(p => new PlagaDto
                    {
                        Id = p.Id,
                        Nombre = p.Nombre,
                        Nivel = p.Nivel,
                        CultivoId = p.CultivoId,
                        CultivoNombre = p.Cultivo.Nombre,
                        ConsultaAPIId = p.ConsultaAPIId
                    }).ToList()
                })
                .ToListAsync();
        }
    }
}