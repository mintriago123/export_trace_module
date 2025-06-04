using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExportModule.Models
{
    public class DatosAExportar
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime FechaExportacion { get; set; }

        [Required]
        [MaxLength(50)]
        public string TipoDato { get; set; } = string.Empty; // 'cultivo', 'plaga', 'consultaAPI'

        [Required]
        [MaxLength(20)]
        public string Formato { get; set; } = string.Empty; // 'csv', 'pdf', 'json'

        [Column(TypeName = "text")]
        public string? Contenido { get; set; }

        // Claves foráneas opcionales
        public int? CultivoId { get; set; }
        public int? PlagaId { get; set; }
        public int? ConsultaAPIId { get; set; }

        // Navegación
        public virtual Cultivo? Cultivo { get; set; }
        public virtual Plaga? Plaga { get; set; }
        public virtual ConsultaAPI? ConsultaAPI { get; set; }
    }
}