using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace ExportModule.Test;




public class EvaluacionControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public EvaluacionControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Puedes inyectar servicios de prueba aquí si necesitas reemplazar algo
            });
        }).CreateClient();
    }

    [Fact]
    public async Task GetEvaluacion_ReturnsSuccess()
    {
        // Arrange
        var cultivoId = 1; // Asegúrate de que este ID exista en la BD en memoria
        var response = await _client.GetAsync($" / api/exportar/evaluar/{cultivoId}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
