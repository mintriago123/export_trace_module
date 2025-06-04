using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExportModule.Models
{
    public class Plaga
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Nivel { get; set; } = string.Empty;

        // Claves foráneas
        [ForeignKey("ConsultaAPI")]
        public int ConsultaAPIId { get; set; }

        [ForeignKey("Cultivo")]
        public int CultivoId { get; set; }

        // Navegación
        public virtual ConsultaAPI ConsultaAPI { get; set; } = null!;
        public virtual Cultivo Cultivo { get; set; } = null!;
    }
}