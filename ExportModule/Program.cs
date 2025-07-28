using ExportModule.Data.Context;
usinbuilder.Services.AddAuthorization();

// Configuración de servicios
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor(); // Para acceder al contexto HTTP
builder.Services.AddControllers();ortModule.Services.Implementaciones;
using ExportModule.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configuraci�n JWT
var jwtConfig = builder.Configuration.GetSection("Jwt");
var keyValue = jwtConfig["Key"];
if (string.IsNullOrEmpty(keyValue))
    throw new InvalidOperationException("JWT Key is not configured. Please check your configuration files.");

var key = Encoding.UTF8.GetBytes(keyValue);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // Cambiar a true en producci�n
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };

});

builder.Services.AddAuthorization();

// Configuraci�n de servicios
builder.Services.AddHttpClient();
builder.Services.AddControllers();

builder.Services.AddScoped<IConsultaApiService, ConsultaApiService>();
builder.Services.AddScoped<IDatosAExportarService, DatosAExportarService>();
builder.Services.AddScoped<ICultivoService, CultivoService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "ExportModule API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new()
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Introduce 'Bearer {token}'"
    });

    c.AddSecurityRequirement(new()
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

// Debug: Verificar la cadena de conexi�n
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine($"Connection String: {connectionString}");

// Configuraci�n de SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

// Asegurar que la base de datos existe
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        context.Database.EnsureCreated();
        Console.WriteLine("Database created successfully");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error creating database: {ex.Message}");
        throw;
    }
}

// Configuraci�n del pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

// Necesario para pruebas de integraci�n
public partial class Program { }