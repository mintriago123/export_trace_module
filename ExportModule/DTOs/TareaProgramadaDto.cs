namespace ExportModule.DTOs
{
    public class TareaProgramadaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public DateTime FechaEjecucion { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public bool EsEjecutable => FechaEjecucion <= DateTime.Now;
    }

    public class CreateTareaProgramadaDto
    {
        public string Nombre { get; set; } = string.Empty;
        public DateTime FechaEjecucion { get; set; }
        public string Tipo { get; set; } = string.Empty;
    }

    public class UpdateTareaProgramadaDto
    {
        public string? Nombre { get; set; }
        public DateTime? FechaEjecucion { get; set; }
        public string? Tipo { get; set; }
    }
}