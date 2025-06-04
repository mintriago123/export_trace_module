using Microsoft.EntityFrameworkCore;
using ExportModule.Models;

namespace ExportModule.Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<ConsultaAPI> ConsultasAPI { get; set; }
        public DbSet<Cultivo> Cultivos { get; set; }
        public DbSet<Plaga> Plagas { get; set; }
        public DbSet<DatosAExportar> DatosAExportar { get; set; }
        public DbSet<TareaProgramada> TareasProgramadas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de relaciones
            modelBuilder.Entity<Plaga>()
                .HasOne(p => p.ConsultaAPI)
                .WithMany(c => c.Plagas)
                .HasForeignKey(p => p.ConsultaAPIId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Plaga>()
                .HasOne(p => p.Cultivo)
                .WithMany(c => c.Plagas)
                .HasForeignKey(p => p.CultivoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DatosAExportar>()
                .HasOne(d => d.Cultivo)
                .WithMany()
                .HasForeignKey(d => d.CultivoId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<DatosAExportar>()
                .HasOne(d => d.Plaga)
                .WithMany()
                .HasForeignKey(d => d.PlagaId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<DatosAExportar>()
                .HasOne(d => d.ConsultaAPI)
                .WithMany(c => c.DatosExportados)
                .HasForeignKey(d => d.ConsultaAPIId)
                .OnDelete(DeleteBehavior.SetNull);

            // Configuración de fechas por defecto
            modelBuilder.Entity<DatosAExportar>()
                .Property(d => d.FechaExportacion)
                .HasDefaultValueSql("NOW()");

            modelBuilder.Entity<ConsultaAPI>()
                .Property(c => c.Fecha)
                .HasDefaultValueSql("NOW()");
        }
    }
}