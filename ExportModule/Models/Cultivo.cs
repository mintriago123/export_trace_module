using System.ComponentModel.DataAnnotations;

namespace ExportModule.Models
{
    public class Cultivo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Tipo { get; set; } = string.Empty;

        // Navegación
        public virtual ICollection<Plaga> Plagas { get; set; } = new List<Plaga>();
    }
}