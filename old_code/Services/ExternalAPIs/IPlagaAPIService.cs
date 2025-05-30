using Export_trace_module.Models;

namespace Export_trace_module.Services;

public interface IPlagaAPIService
{
    Task<Plaga> GetPlagaByIdAsync(int id);
    Task<IEnumerable<Plaga>> GetPlagasPorCultivoAsync(int cultivoId);
    Task<IEnumerable<Plaga>> BuscarPlagasAsync(string criterio);
    Task<IEnumerable<Plaga>> GetPlagasPorNivelAsync(string nivel);
}