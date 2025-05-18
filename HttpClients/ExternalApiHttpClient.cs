public static class ExternalApiHttpClient
{
    public static IServiceCollection AddExternalApiHttpClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient("CultivoAPI", client =>
        {
            client.BaseAddress = new Uri(configuration["ExternalAPIs:CultivoAPI:BaseUrl"]);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            // Agrega headers necesarios (API keys, etc.)
        });

        services.AddHttpClient("PlagaAPI", client =>
        {
            client.BaseAddress = new Uri(configuration["ExternalAPIs:PlagaAPI:BaseUrl"]);
            // Configuración específica para la API de Plagas
        });

        return services;
    }
}