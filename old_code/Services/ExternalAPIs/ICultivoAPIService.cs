using Export_trace_module.Models;

namespace Export_trace_module.Services;

public interface ICultivoAPIService
{
    Task<Cultivo> GetCultivoByIdAsync(int id);
    Task<IEnumerable<Cultivo>> GetAllCultivosAsync();
    Task<IEnumerable<Cultivo>> BuscarCultivosAsync(string criterio);
}