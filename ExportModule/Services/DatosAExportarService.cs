using Microsoft.EntityFrameworkCore;
using ExportModule.Data.Context;
using ExportModule.DTOs;
using ExportModule.Models;
using System.Text.Json;

namespace ExportModule.Services
{
    public class DatosAExportarService : IDatosAExportarService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICultivoService _cultivoService;

        public DatosAExportarService(ApplicationDbContext context, ICultivoService cultivoService)
        {
            _context = context;
            _cultivoService = cultivoService;
        }

        public async Task<IEnumerable<DatosAExportarDto>> GetAllAsync()
        {
            return await _context.DatosAExportar
                .Include(d => d.Cultivo)
                .Include(d => d.Plaga)
                .Include(d => d.ConsultaAPI)
                .Select(d => new DatosAExportarDto
                {
                    Id = d.Id,
                    FechaExportacion = d.FechaExportacion,
                    TipoDato = d.TipoDato,
                    Formato = d.Formato,
                    Contenido = d.Contenido,
                    CultivoId = d.CultivoId,
                    PlagaId = d.PlagaId,
                    ConsultaAPIId = d.ConsultaAPIId,
                    CultivoNombre = d.Cultivo != null ? d.Cultivo.Nombre : null,
                    PlagaNombre = d.Plaga != null ? d.Plaga.Nombre : null,
                    ConsultaEndpoint = d.ConsultaAPI != null ? d.ConsultaAPI.Endpoint : null
                })
                .ToListAsync();
        }

        public async Task<DatosAExportarDto?> GetByIdAsync(int id)
        {
            var datos = await _context.DatosAExportar
                .Include(d => d.Cultivo)
                .Include(d => d.Plaga)
                .Include(d => d.ConsultaAPI)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (datos == null) return null;

            return new DatosAExportarDto
            {
                Id = datos.Id,
                FechaExportacion = datos.FechaExportacion,
                TipoDato = datos.TipoDato,
                Formato = datos.Formato,
                Contenido = datos.Contenido,
                CultivoId = datos.CultivoId,
                PlagaId = datos.PlagaId,
                ConsultaAPIId = datos.ConsultaAPIId,
                CultivoNombre = datos.Cultivo?.Nombre,
                PlagaNombre = datos.Plaga?.Nombre,
                ConsultaEndpoint = datos.ConsultaAPI?.Endpoint
            };
        }

        public async Task<DatosAExportarDto> CreateAsync(CreateDatosAExportarDto createDto)
        {
            var datos = new DatosAExportar
            {
                TipoDato = createDto.TipoDato,
                Formato = createDto.Formato,
                Contenido = createDto.Contenido,
                CultivoId = createDto.CultivoId,
                PlagaId = createDto.PlagaId,
                ConsultaAPIId = createDto.ConsultaAPIId,
                FechaExportacion = DateTime.Now
            };

            _context.DatosAExportar.Add(datos);
            await _context.SaveChangesAsync();

            return new DatosAExportarDto
            {
                Id = datos.Id,
                FechaExportacion = datos.FechaExportacion,
                TipoDato = datos.TipoDato,
                Formato = datos.Formato,
                Contenido = datos.Contenido,
                CultivoId = datos.CultivoId,
                PlagaId = datos.PlagaId,
                ConsultaAPIId = datos.ConsultaAPIId
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var datos = await _context.DatosAExportar.FindAsync(id);
            if (datos == null) return false;

            _context.DatosAExportar.Remove(datos);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<DatosAExportarDto>> GetByTipoDatoAsync(string tipoDato)
        {
            return await _context.DatosAExportar
                .Include(d => d.Cultivo)
                .Include(d => d.Plaga)
                .Include(d => d.ConsultaAPI)
                .Where(d => d.TipoDato.ToLower() == tipoDato.ToLower())
                .Select(d => new DatosAExportarDto
                {
                    Id = d.Id,
                    FechaExportacion = d.FechaExportacion,
                    TipoDato = d.TipoDato,
                    Formato = d.Formato,
                    CultivoNombre = d.Cultivo != null ? d.Cultivo.Nombre : null,
                    PlagaNombre = d.Plaga != null ? d.Plaga.Nombre : null,
                    ConsultaEndpoint = d.ConsultaAPI != null ? d.ConsultaAPI.Endpoint : null
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<DatosAExportarDto>> GetByFormatoAsync(string formato)
        {
            return await _context.DatosAExportar
                .Where(d => d.Formato.ToLower() == formato.ToLower())
                .Select(d => new DatosAExportarDto
                {
                    Id = d.Id,
                    FechaExportacion = d.FechaExportacion,
                    TipoDato = d.TipoDato,
                    Formato = d.Formato
                })
                .ToListAsync();
        }

        public async Task<ExportResultDto> ExportDataAsync(ExportRequestDto request)
        {
            try
            {
                string contenido = string.Empty;
                
                switch (request.TipoDato.ToLower())
                {
                    case "cultivo":
                        if (request.EntityId.HasValue)
                        {
                            // Verificar si el cultivo debe exportarse
                            var shouldExport = await _cultivoService.ShouldExportCultivoAsync(request.EntityId.Value);
                            if (!shouldExport)
                            {
                                return new ExportResultDto
                                {
                                    Success = false,
                                    Message = "El cultivo no puede ser exportado debido a plagas críticas",
                                    FechaExportacion = DateTime.Now
                                };
                            }
                            
                            var cultivo = await _cultivoService.GetByIdAsync(request.EntityId.Value);
                            if (cultivo == null)
                            {
                                return new ExportResultDto
                                {
                                    Success = false,
                                    Message = "Cultivo no encontrado",
                                    FechaExportacion = DateTime.Now
                                };
                            }
                            contenido = GenerateContent(cultivo, request.Formato);
                        }
                        else
                        {
                            var cultivos = await _cultivoService.GetAllAsync();
                            // Filtrar solo cultivos exportables
                            var cultivosExportables = new List<CultivoDto>();
                            foreach (var c in cultivos)
                            {
                                if (await _cultivoService.ShouldExportCultivoAsync(c.Id))
                                {
                                    cultivosExportables.Add(c);
                                }
                            }
                            contenido = GenerateContent(cultivosExportables, request.Formato);
                        }
                        break;
                        
                    case "plaga":
                        // Lógica para exportar plagas
                        contenido = "Exportación de plagas - Implementar según necesidades";
                        break;
                        
                    case "consultaapi":
                        // Lógica para exportar consultas API
                        contenido = "Exportación de consultas API - Implementar según necesidades";
                        break;
                        
                    default:
                        return new ExportResultDto
                        {
                            Success = false,
                            Message = "Tipo de dato no válido",
                            FechaExportacion = DateTime.Now
                        };
                }

                // Guardar el registro de exportación
                var datosExportacion = new CreateDatosAExportarDto
                {
                    TipoDato = request.TipoDato,
                    Formato = request.Formato,
                    Contenido = contenido,
                    CultivoId = request.TipoDato.ToLower() == "cultivo" ? request.EntityId : null
                };

                await CreateAsync(datosExportacion);

                return new ExportResultDto
                {
                    Success = true,
                    Message = "Exportación completada exitosamente",
                    Content = contenido,
                    FechaExportacion = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                return new ExportResultDto
                {
                    Success = false,
                    Message = $"Error durante la exportación: {ex.Message}",
                    FechaExportacion = DateTime.Now
                };
            }
        }

        private string GenerateContent<T>(T data, string format)
        {
            return format.ToLower() switch
            {
                "json" => JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true }),
                "csv" => GenerateCSV(data),
                "pdf" => "PDF generation not implemented", // Implementar con librerías como iTextSharp
                _ => JsonSerializer.Serialize(data)
            };
        }

        private string GenerateCSV<T>(T data)
        {
            // Implementación básica de CSV - puedes mejorar según necesidades
            if (data is IEnumerable<CultivoDto> cultivos)
            {
                var csv = "Id,Nombre,Tipo,CantidadPlagas\n";
                foreach (var cultivo in cultivos)
                {
                    csv += $"{cultivo.Id},{cultivo.Nombre},{cultivo.Tipo},{cultivo.Plagas?.Count ?? 0}\n";
                }
                return csv;
            }
            return JsonSerializer.Serialize(data);
        }
    }
}