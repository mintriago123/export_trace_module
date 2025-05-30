using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Export_trace_module.Models;

public class ConsultaAPI
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [Column(TypeName = "varchar(255)")]
    public string Endpoint { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "timestamp with time zone")]
    public DateTime Fecha { get; set; } = DateTime.UtcNow;

    // Relación con Plagas (solo referencia, no se mapea como FK)
    public virtual ICollection<Plaga> Plagas { get; set; } = new List<Plaga>();

    // Relación con DatosAExportar
    public virtual ICollection<DatosAExportar> DatosExportados { get; set; } = new List<DatosAExportar>();
}