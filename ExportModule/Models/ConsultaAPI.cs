using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExportModule.Models
{
    public class ConsultaAPI
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Endpoint { get; set; } = string.Empty;

        [Required]
        public DateTime Fecha { get; set; }

        // Navegación
        public virtual ICollection<Plaga> Plagas { get; set; } = new List<Plaga>();
        public virtual ICollection<DatosAExportar> DatosExportados { get; set; } = new List<DatosAExportar>();
    }
}