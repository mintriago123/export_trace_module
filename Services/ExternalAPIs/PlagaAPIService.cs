using Export_trace_module.Models;

namespace Export_trace_module.Services;

public class PlagaAPIService : IPlagaAPIService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<PlagaAPIService> _logger;

    public PlagaAPIService(HttpClient httpClient, ILogger<PlagaAPIService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<Plaga> GetPlagaByIdAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/api/plagas/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Plaga>() ??
                throw new Exception("Plaga no encontrada");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener plaga con ID {PlagaId}", id);
            throw;
        }
    }


    public PlagaAPIService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<Plaga>> GetPlagasPorCultivoAsync(int cultivoId)
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<Plaga>>($"/api/plagas/cultivo/{cultivoId}")
               ?? Enumerable.Empty<Plaga>();
    }

    public async Task<IEnumerable<Plaga>> BuscarPlagasAsync(string criterio)
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<Plaga>>($"/api/plagas/buscar?q={criterio}")
               ?? Enumerable.Empty<Plaga>();
    }

    public async Task<IEnumerable<Plaga>> GetPlagasPorNivelAsync(string nivel)
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<Plaga>>($"/api/plagas/nivel/{nivel}")
               ?? Enumerable.Empty<Plaga>();
    }
}