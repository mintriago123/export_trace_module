using ExportModule.Data.Context;
using ExportModule.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace ExportModule.Services.Implementaciones
{
    public class CultivoService : ICultivoService
    {
        private readonly AppDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly string _iaUrl;

        public CultivoService(AppDbContext context, HttpClient httpClient, IConfiguration config)
        {
            _context = context;
            _httpClient = httpClient;
            _iaUrl = config["IA:EndpointEvaluacion"]; // ← desde appsettings.json o variable de entorno
        }

        public async Task<(bool esExportable, string motivo)> EvaluarCultivoAsync(int cultivoId)
        {
            var cultivo = await _context.Cultivos
                .Include(c => c.Plagas)
                .FirstOrDefaultAsync(c => c.Id == cultivoId);

            if (cultivo == null)
                return (false, "Cultivo no encontrado");

            // Armar el JSON
            var payload = new
            {
                cultivo = new
                {
                    id = cultivo.Id,
                    nombre = cultivo.Nombre,
                    tipo = cultivo.Tipo
                },
                plagas = cultivo.Plagas.Select(p => new
                {
                    id = p.Id,
                    nombre = p.Nombre,
                    nivel = p.Nivel.ToString().ToLower()
                }).ToList()
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync(_iaUrl, content);
                response.EnsureSuccessStatusCode();

                var resultString = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<EvaluacionResultado>(resultString,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return (result.AptoParaExportacion, result.Motivo);
            }
            catch (Exception ex)
            {
                // Si falla la IA, podrías retornar falso o hacer fallback
                return (false, $"Error en la evaluación externa: {ex.Message}");
            }
        }

        private class EvaluacionResultado
        {
            public bool AptoParaExportacion { get; set; }
            public string Motivo { get; set; } = "";
        }
    }
}
