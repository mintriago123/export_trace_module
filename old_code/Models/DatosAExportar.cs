using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Export_trace_module.Models;

public class DatosAExportar
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    [Column(TypeName = "timestamp with time zone")]
    public DateTime FechaExportacion { get; set; } = DateTime.UtcNow;

    [Required]
    [Column(TypeName = "varchar(50)")]
    public string TipoDato { get; set; } = string.Empty; // 'cultivo', 'plaga', 'consultaAPI'

    [Required]
    [Column(TypeName = "varchar(10)")]
    public string Formato { get; set; } = string.Empty; // 'csv', 'pdf', 'json'

    [Column(TypeName = "text")]
    public string? Contenido { get; set; }

    // Referencias a IDs de entidades externas (no son FKs reales)
    public int? CultivoId { get; set; }
    public int? PlagaId { get; set; }
    public int? ConsultaAPIId { get; set; }

    // Propiedades de navegación (solo para ConsultaAPI que es local)
    [ForeignKey("ConsultaAPIId")]
    public virtual ConsultaAPI? ConsultaAPI { get; set; }
}