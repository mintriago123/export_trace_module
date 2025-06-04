using Microsoft.EntityFrameworkCore;
using ExportModule.Data.Context;
using ExportModule.Services;

var builder = WebApplication.CreateBuilder(args);

// Configurar servicios
builder.Services.AddControllers();

// Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Servicios personalizados
builder.Services.AddScoped<ICultivoService, CultivoService>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Export Module API", Version = "v1" });

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

var app = builder.Build();

// Configurar pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
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
    context.Database.EnsureCreated();
}

app.Run();