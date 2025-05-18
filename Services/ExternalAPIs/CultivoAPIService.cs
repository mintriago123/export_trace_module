using System.Net.Http.Json;
using Export_trace_module.Models;

namespace Export_trace_module.Services.ExternalAPIs;

public interface ICultivoAPIService
{
    Task<IEnumerable<Cultivo>> GetAllCultivosAsync();
    Task<Cultivo?> GetCultivoByIdAsync(int id);
    // Otros métodos según necesites
}

public class CultivoAPIService : ICultivoAPIService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CultivoAPIService> _logger;

    public CultivoAPIService(IHttpClientFactory httpClientFactory, 
                           ILogger<CultivoAPIService> logger)
    {
        _httpClient = httpClientFactory.CreateClient("CultivoAPI");
        _logger = logger;
    }

    public async Task<IEnumerable<Cultivo>> GetAllCultivosAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/cultivos");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<Cultivo>>() ?? [];
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error al obtener cultivos de API externa");
            throw; // O maneja el error como prefieras
        }
    }

    public async Task<Cultivo?> GetCultivoByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"api/cultivos/{id}");
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<Cultivo>();
    }
}