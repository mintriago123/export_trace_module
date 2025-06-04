using ExportModule.DTOs;

namespace ExportModule.Services
{
    public interface ICultivoService
    {
        Task<IEnumerable<CultivoDto>> GetAllAsync();
        Task<CultivoDto?> GetByIdAsync(int id);
        Task<CultivoDto> CreateAsync(CreateCultivoDto createDto);
        Task<CultivoDto?> UpdateAsync(int id, UpdateCultivoDto updateDto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<CultivoDto>> GetCultivosAfectadosPorPlagasAsync();
        Task<bool> ShouldExportCultivoAsync(int cultivoId);
    }
}