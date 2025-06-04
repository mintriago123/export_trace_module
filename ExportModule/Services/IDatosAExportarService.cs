using ExportModule.DTOs;

namespace ExportModule.Services
{
    public interface IDatosAExportarService
    {
        Task<IEnumerable<DatosAExportarDto>> GetAllAsync();
        Task<DatosAExportarDto?> GetByIdAsync(int id);
        Task<DatosAExportarDto> CreateAsync(CreateDatosAExportarDto createDto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<DatosAExportarDto>> GetByTipoDatoAsync(string tipoDato);
        Task<IEnumerable<DatosAExportarDto>> GetByFormatoAsync(string formato);
        Task<ExportResultDto> ExportDataAsync(ExportRequestDto request);
    }
}