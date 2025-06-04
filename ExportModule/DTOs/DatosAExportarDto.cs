namespace ExportModule.DTOs
{
    public class DatosAExportarDto
    {
        public int Id { get; set; }
        public DateTime FechaExportacion { get; set; }
        public string TipoDato { get; set; } = string.Empty;
        public string Formato { get; set; } = string.Empty;
        public string? Contenido { get; set; }
        public int? CultivoId { get; set; }
        public int? PlagaId { get; set; }
        public int? ConsultaAPIId { get; set; }

        // Datos relacionados
        public string? CultivoNombre { get; set; }
        public string? PlagaNombre { get; set; }
        public string? ConsultaEndpoint { get; set; }
    }

    public class CreateDatosAExportarDto
    {
        public string TipoDato { get; set; } = string.Empty;
        public string Formato { get; set; } = string.Empty;
        public string? Contenido { get; set; }
        public int? CultivoId { get; set; }
        public int? PlagaId { get; set; }
        public int? ConsultaAPIId { get; set; }
    }
}