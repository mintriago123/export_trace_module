using Export_trace_module.Models;

namespace Export_trace_module.Services;

public interface IConsultaAPIService
{
    Task<ConsultaAPI> CreateConsultaAsync(string endpoint);
    Task<ConsultaAPI?> GetConsultaByIdAsync(int id);
    Task<IEnumerable<ConsultaAPI>> GetAllConsultasAsync();
    Task<bool> DeleteConsultaAsync(int id);
    Task<ConsultaAPI> AddPlagasToConsultaAsync(int consultaId, IEnumerable<int> plagaIds);
}