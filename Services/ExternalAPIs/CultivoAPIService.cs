using Export_trace_module.Models;
using System.Net.Http.Json;

namespace Export_trace_module.Services.ExternalAPIs;

public class CultivoAPIService : ICultivoAPIService
{
    private readonly HttpClient _httpClient;

    public CultivoAPIService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Cultivo> GetCultivoByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<Cultivo>($"/api/cultivos/{id}") 
               ?? throw new Exception("Cultivo no encontrado");
    }

    public async Task<IEnumerable<Cultivo>> GetAllCultivosAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<Cultivo>>("/api/cultivos") 
               ?? Enumerable.Empty<Cultivo>();
    }

    public async Task<IEnumerable<Cultivo>> BuscarCultivosAsync(string criterio)
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<Cultivo>>($"/api/cultivos/buscar?q={criterio}") 
               ?? Enumerable.Empty<Cultivo>();
    }
}