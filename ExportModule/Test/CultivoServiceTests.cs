using Xunit;
using Moq;
using ExportModule.Services.Implementaciones;
using ExportModule.Data.Context;
using ExportModule.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;
using ExportModule.Services.Interfaces;

namespace ExportModule.Test;

public class CultivoServiceTests
{
    [Fact]
    public async Task EvaluarCultivoAsync_ReturnsTrue_WhenNoPlagas()
    {
        // Arrange: usamos una BD en memoria para test
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        using var context = new AppDbContext(options);
        var cultivo = new Cultivo { Id = 1, Nombre = "Maíz", Tipo = "Grano" };
        context.Cultivos.Add(cultivo);
        context.SaveChanges();

        var mockHttpClient = new HttpClient(new FakeHttpHandler());
        var configValues = new Dictionary<string, string?>
        {
            ["IA:EndpointEvaluacion"] = "http://localhost:8000/evaluar-cultivo"
        };
        var mockConfig = new ConfigurationBuilder()
            .AddInMemoryCollection(configValues)
            .Build();

        var mockExportService = new Mock<IDatosAExportarService>();

        var service = new CultivoService(context, mockHttpClient, mockConfig, mockExportService.Object);

        // Act
        var resultado = await service.EvaluarCultivoAsync(1);

        // Assert
        Assert.True(resultado.esExportable);
    }

    // Fake handler que simula la IA externa sin hacer request real
    public class FakeHttpHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var json = "{\"aptoParaExportacion\": true, \"motivo\": \"Sin plagas\"}";
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
            };
            return Task.FromResult(response);
        }
    }
}
