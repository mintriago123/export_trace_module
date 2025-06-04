using Microsoft.EntityFrameworkCore;
using ExportModule.Data.Context;
using ExportModule.Services;

var builder = WebApplication.CreateBuilder(args);

// Configurar servicios
builder.Services.AddControllers();

// Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Servicios personalizados - Registro de todas las interfaces
builder.Services.AddScoped<ICultivoService, CultivoService>();
builder.Services.AddScoped<IPlagaService, PlagaService>();
builder.Services.AddScoped<IConsultaAPIService, ConsultaAPIService>();
builder.Services.AddScoped<IDatosAExportarService, DatosAExportarService>();
builder.Services.AddScoped<ITareaProgramadaService, TareaProgramadaService>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { 
        Title = "Export Module API", 
        Version = "v1",
        Description = "API para gestión de cultivos, plagas y exportación de datos"
    });
    
    // Incluir comentarios XML para documentación
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// CORS (opcional)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Logging
builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
    logging.AddDebug();
});

var app = builder.Build();

// Configurar pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Export Module API V1");
        c.RoutePrefix = "swagger";
    });
}

// NO usar HTTPS
// app.UseHttpsRedirection(); // Comentado para no usar HTTPS

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

// Crear base de datos automáticamente en desarrollo
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    
    try
    {
        context.Database.EnsureCreated();
        
        // Datos de prueba opcionales
        if (!context.Cultivos.Any())
        {
            await SeedDataAsync(context);
        }
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Error creando la base de datos");
    }
}

app.Run();

// Método para datos de prueba
static async Task SeedDataAsync(ApplicationDbContext context)
{
    // Crear datos de ejemplo
    var cultivos = new[]
    {
        new ExportModule.Models.Cultivo { Nombre = "Maíz", Tipo = "Cereal" },
        new ExportModule.Models.Cultivo { Nombre = "Trigo", Tipo = "Cereal" },
        new ExportModule.Models.Cultivo { Nombre = "Tomate", Tipo = "Hortaliza" },
        new ExportModule.Models.Cultivo { Nombre = "Lechuga", Tipo = "Hortaliza" }
    };

    context.Cultivos.AddRange(cultivos);
    await context.SaveChangesAsync();

    var consultasAPI = new[]
    {
        new ExportModule.Models.ConsultaAPI { Endpoint = "/api/plagas/detect", Fecha = DateTime.Now.AddDays(-1) },
        new ExportModule.Models.ConsultaAPI { Endpoint = "/api/cultivos/status", Fecha = DateTime.Now.AddHours(-2) }
    };

    context.ConsultasAPI.AddRange(consultasAPI);
    await context.SaveChangesAsync();

    var plagas = new[]
    {
        new ExportModule.Models.Plaga { Nombre = "Pulgón", Nivel = "Bajo", CultivoId = cultivos[0].Id, ConsultaAPIId = consultasAPI[0].Id },
        new ExportModule.Models.Plaga { Nombre = "Roya", Nivel = "Alto", CultivoId = cultivos[1].Id, ConsultaAPIId = consultasAPI[0].Id },
        new ExportModule.Models.Plaga { Nombre = "Mosca Blanca", Nivel = "Medio", CultivoId = cultivos[2].Id, ConsultaAPIId = consultasAPI[1].Id }
    };

    context.Plagas.AddRange(plagas);
    await context.SaveChangesAsync();

    var tareas = new[]
    {
        new ExportModule.Models.TareaProgramada { Nombre = "Exportar Cultivos Diario", FechaEjecucion = DateTime.Now.AddHours(1), Tipo = "exportacion" },
        new ExportModule.Models.TareaProgramada { Nombre = "Limpiar Datos Antiguos", FechaEjecucion = DateTime.Now.AddDays(1), Tipo = "limpieza" },
        new ExportModule.Models.TareaProgramada { Nombre = "Reporte Semanal Plagas", FechaEjecucion = DateTime.Now.AddDays(7), Tipo = "reporte" }
    };

    context.TareasProgramadas.AddRange(tareas);
    await context.SaveChangesAsync();
}