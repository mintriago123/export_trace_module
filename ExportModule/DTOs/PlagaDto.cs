namespace ExportModule.DTOs
{
    public class PlagaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Nivel { get; set; } = string.Empty;
        public int CultivoId { get; set; }
        public string? CultivoNombre { get; set; }
        public int ConsultaAPIId { get; set; }
    }

    public class CreatePlagaDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Nivel { get; set; } = string.Empty;
        public int CultivoId { get; set; }
        public int ConsultaAPIId { get; set; }
    }
}