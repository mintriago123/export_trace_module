using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Export_trace_module.Data;
using Export_trace_module.Services;
using Export_trace_module.Services.ExternalAPIs; // Añade este namespace
using Export_trace_module.Controllers.MinimalAPIs; // O el namespace correcto donde están tus endpoints

var builder = WebApplication.CreateBuilder(args);

// Configuración de PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQLConnection")));

// Configuración de HttpClient para APIs externas
var plagaApiUrl = builder.Configuration["ExternalAPIs:PlagaAPI:BaseUrl"] 
    ?? throw new InvalidOperationException("PlagaAPI BaseUrl no configurada");
var cultivoApiUrl = builder.Configuration["ExternalAPIs:CultivoAPI:BaseUrl"]
    ?? throw new InvalidOperationException("CultivoAPI BaseUrl no configurada");

builder.Services.AddHttpClient<IPlagaAPIService, PlagaAPIService>(client => 
{
    client.BaseAddress = new Uri(plagaApiUrl);
});

builder.Services.AddHttpClient<ICultivoAPIService, CultivoAPIService>(client =>
{
    client.BaseAddress = new Uri(cultivoApiUrl);
});

// Registrar servicios
builder.Services.AddScoped<IConsultaAPIService, ConsultaAPIService>();
builder.Services.AddScoped<IDatosExportacionService, DatosExportacionService>();
builder.Services.AddScoped<IPlagaAPIService, PlagaAPIService>();
builder.Services.AddScoped<ICultivoAPIService, CultivoAPIService>();

// Configuración de Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configurar el pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Mapear endpoints
app.MapConsultaAPIEndpoints();
app.MapPlagasEndpoints();
app.MapExportacionesEndpoints();

app.Run();