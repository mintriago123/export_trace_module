namespace ExportModule.DTOs
{
    public class ConsultaAPIDto
    {
        public int Id { get; set; }
        public string Endpoint { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public List<PlagaDto>? Plagas { get; set; }
        public List<DatosAExportarDto>? DatosExportados { get; set; }
    }

    public class CreateConsultaAPIDto
    {
        public string Endpoint { get; set; } = string.Empty;
        public DateTime? Fecha { get; set; }
    }

    public class UpdateConsultaAPIDto
    {
        public string? Endpoint { get; set; }
        public DateTime? Fecha { get; set; }
    }
}