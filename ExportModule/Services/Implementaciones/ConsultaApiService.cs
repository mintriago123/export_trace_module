using ExportModule.Data.Context;
using ExportModule.Models;
using ExportModule.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace ExportModule.Services.Implementaciones
{
    public class ConsultaApiService : IConsultaApiService
    {
        private readonly AppDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ConsultaApiService(AppDbContext context, HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<int> EjecutarConsultaAsync()
        {
            // Consulta real a la API externa
            var endpoint = "http://localhost:4000/cultivo";
            var consulta = new ConsultaAPI
            {
                Fecha = DateTime.UtcNow,
                Endpoint = endpoint
            };

            try
            {
                // Obtener el token de autenticación del contexto HTTP actual
                var authorizationHeader = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].FirstOrDefault();
                string token = null;
                
                if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
                {
                    token = authorizationHeader.Substring("Bearer ".Length).Trim();
                    Console.WriteLine($"🔐 Token obtenido: {token.Substring(0, Math.Min(20, token.Length))}...");
                }
                else
                {
                    Console.WriteLine($"⚠️ No se encontró token de autenticación en la petición");
                }

                // Realizar la consulta HTTP a localhost:4000/cultivo
                Console.WriteLine($"Realizando consulta a: {endpoint}");
                
                // Configurar las cabeceras de autenticación si tenemos token
                if (!string.IsNullOrEmpty(token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    Console.WriteLine($"✅ Token agregado a las cabeceras de la petición");
                }
                
                var response = await _httpClient.GetAsync(endpoint);
                
                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"✅ Consulta exitosa a {endpoint}");
                    Console.WriteLine($"📄 Respuesta recibida: {jsonContent}");
                    
                    // Aquí podrías deserializar la respuesta JSON si necesitas procesar los datos
                    // Ejemplo: var cultivosFromApi = JsonSerializer.Deserialize<List<dynamic>>(jsonContent);
                }
                else
                {
                    Console.WriteLine($"❌ Error en la consulta HTTP: {response.StatusCode} - {response.ReasonPhrase}");
                }

                // Limpiar las cabeceras después de la petición
                _httpClient.DefaultRequestHeaders.Authorization = null;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"🔌 Error de conexión: {ex.Message}");
                Console.WriteLine($"💡 Asegúrate de que el servidor esté ejecutándose en localhost:4000");
            }
            catch (TaskCanceledException ex)
            {
                Console.WriteLine($"⏱️ Timeout en la consulta: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error inesperado: {ex.Message}");
            }

            // Simulamos cultivos
            var cultivo1 = new Cultivo { Nombre = "Maíz", Tipo = "Cereal" };
            var cultivo2 = new Cultivo { Nombre = "Papa", Tipo = "Tubérculo" };

            // Simulamos plagas
            var plagas = new List<Plaga>
            {
                new Plaga { Nombre = "Gusano cogollero", Nivel = "moderado", Cultivo = cultivo1, ConsultaAPI = consulta },
                new Plaga { Nombre = "Pulgón", Nivel = "leve", Cultivo = cultivo1, ConsultaAPI = consulta },
                new Plaga { Nombre = "Escarabajo de la papa", Nivel = "crítico", Cultivo = cultivo2, ConsultaAPI = consulta }
            };

            consulta.Plagas = plagas;
            consulta.DatosAExportar = new List<DatosAExportar>(); // por ahora vacío

            // Agregamos entidades relacionadas
            _context.ConsultaAPIs.Add(consulta);
            _context.Cultivos.AddRange(cultivo1, cultivo2);
            _context.Plagas.AddRange(plagas);

            await _context.SaveChangesAsync();

            return consulta.Id;
        }
    }
}
