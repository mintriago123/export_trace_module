using System.ComponentModel.DataAnnotations;

namespace ExportModule.Models
{
    public class TareaProgramada
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public DateTime FechaEjecucion { get; set; }

        [Required]
        [MaxLength(50)]
        public string Tipo { get; set; } = string.Empty;
    }
}