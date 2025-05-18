using Microsoft.EntityFrameworkCore;
using Export_trace_module.Models;

namespace Export_trace_module.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Solo las entidades que se almacenan localmente
    public DbSet<ConsultaAPI> ConsultasAPI { get; set; }
    public DbSet<DatosAExportar> DatosAExportar { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuración solo para entidades locales
        modelBuilder.Entity<ConsultaAPI>(entity =>
        {
            entity.HasIndex(e => e.Fecha);
            entity.HasMany(e => e.DatosExportados)
                  .WithOne(d => d.ConsultaAPI)
                  .HasForeignKey(d => d.ConsultaAPIId);
        });

        modelBuilder.Entity<DatosAExportar>(entity =>
        {
            entity.HasIndex(e => e.TipoDato);
            entity.HasIndex(e => e.FechaExportacion);
        });
    }
}