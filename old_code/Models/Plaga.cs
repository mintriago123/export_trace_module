namespace Export_trace_module.Models;
using System.ComponentModel.DataAnnotations; // Para los DataAnnotations

public class Plaga
{
    public int Id { get; set; } // ID en el sistema externo
    
    [Required]
    public string Nombre { get; set; } = string.Empty;
    
    [Required]
    public string Nivel { get; set; } = "medio"; // bajo, medio, alto
    
    // Propiedades adicionales de la API externa
    public string? NombreCientifico { get; set; }
    public string? TipoPlaga { get; set; } // insecto, hongo, bacteria, etc.
    public string? Sintomas { get; set; }
    public string? TratamientoRecomendado { get; set; }
    
    // Relación con cultivo (solo referencia)
    public int CultivoId { get; set; } // ID del cultivo en sistema externo
    public string? CultivoNombre { get; set; }
    
    // Metadata de la API externa (opcional)
    public DateTime? DetectadaPorPrimeraVez { get; set; }
    public bool? EsEndemica { get; set; }
    public string? Fuente { get; set; } = "API Externa";
}