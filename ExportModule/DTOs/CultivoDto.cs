namespace ExportModule.DTOs
{
    public class CultivoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public List<PlagaDto>? Plagas { get; set; }
    }

    public class CreateCultivoDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
    }

    public class UpdateCultivoDto
    {
        public string? Nombre { get; set; }
        public string? Tipo { get; set; }
    }
}