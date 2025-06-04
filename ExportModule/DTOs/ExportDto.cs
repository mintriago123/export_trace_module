namespace ExportModule.DTOs
{
    public class ExportRequestDto
    {
        public string TipoDato { get; set; } = string.Empty; // 'cultivo', 'plaga', 'consultaAPI'
        public string Formato { get; set; } = string.Empty; // 'csv', 'pdf', 'json'
        public int? EntityId { get; set; }
    }

    public class ExportResultDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? Content { get; set; }
        public DateTime FechaExportacion { get; set; }
    }
}