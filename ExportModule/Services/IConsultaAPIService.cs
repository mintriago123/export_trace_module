using ExportModule.DTOs;

namespace ExportModule.Services
{
    public interface IConsultaAPIService
    {
        Task<IEnumerable<ConsultaAPIDto>> GetAllAsync();
        Task<ConsultaAPIDto?> GetByIdAsync(int id);
        Task<ConsultaAPIDto> CreateAsync(CreateConsultaAPIDto createDto);
        Task<ConsultaAPIDto?> UpdateAsync(int id, UpdateConsultaAPIDto updateDto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<ConsultaAPIDto>> GetConsultasByFechaAsync(DateTime fecha);
        Task<IEnumerable<ConsultaAPIDto>> GetConsultasConPlagasAsync();
    }
}