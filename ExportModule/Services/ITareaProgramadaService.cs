using ExportModule.DTOs;

namespace ExportModule.Services
{
    public interface ITareaProgramadaService
    {
        Task<IEnumerable<TareaProgramadaDto>> GetAllAsync();
        Task<TareaProgramadaDto?> GetByIdAsync(int id);
        Task<TareaProgramadaDto> CreateAsync(CreateTareaProgramadaDto createDto);
        Task<TareaProgramadaDto?> UpdateAsync(int id, UpdateTareaProgramadaDto updateDto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<TareaProgramadaDto>> GetTareasEjecutablesAsync();
        Task<IEnumerable<TareaProgramadaDto>> GetTareasByTipoAsync(string tipo);
        Task<bool> ExecuteTaskAsync(int id);
    }
}