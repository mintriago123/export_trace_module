using System.ComponentModel.DataAnnotations; // Para los DataAnnotations
namespace Export_trace_module.Models;

public class Cultivo
{
    public int Id { get; set; } // ID en el sistema externo

    [Required]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    public string Tipo { get; set; } = string.Empty;

    // Propiedades adicionales de la API externa
    public string? Descripcion { get; set; }
    public string? TemporadaRecomendada { get; set; }
    public string? ZonaClimatica { get; set; }
    public int? DiasCosecha { get; set; }

    // Lista de plagas asociadas (solo referencia)
    public List<Plaga>? PlagasAsociadas { get; set; }

    // Metadata de la API externa (opcional)
    public DateTime? UltimaActualizacion { get; set; }
    public string? Fuente { get; set; } = "API Externa";
}