using ExportModule.Data.Context;
using ExportModule.Models;
using ExportModule.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExportModule.Services.Implementaciones
{
    public class ConsultaApiService : IConsultaApiService
    {
        private readonly AppDbContext _context;

        public ConsultaApiService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> EjecutarConsultaAsync()
        {
            // Simula consulta a una API externa
            var consulta = new ConsultaAPI
            {
                Fecha = DateTime.UtcNow,
                Endpoint = "http://api.simulada.local/cultivos"
            };

            // Simulamos cultivos
            var cultivo1 = new Cultivo { Nombre = "Maíz", Tipo = "Cereal" };
            var cultivo2 = new Cultivo { Nombre = "Papa", Tipo = "Tubérculo" };

            // Simulamos plagas
            var plagas = new List<Plaga>
            {
                new Plaga { Nombre = "Gusano cogollero", Nivel = "moderado", Cultivo = cultivo1, ConsultaAPI = consulta },
                new Plaga { Nombre = "Pulgón", Nivel = "leve", Cultivo = cultivo1, ConsultaAPI = consulta },
                new Plaga { Nombre = "Escarabajo de la papa", Nivel = "crítico", Cultivo = cultivo2, ConsultaAPI = consulta }
            };

            consulta.Plagas = plagas;
            consulta.DatosAExportar = new List<DatosAExportar>(); // por ahora vacío

            // Agregamos entidades relacionadas
            _context.ConsultaAPIs.Add(consulta);
            _context.Cultivos.AddRange(cultivo1, cultivo2);
            _context.Plagas.AddRange(plagas);

            await _context.SaveChangesAsync();

            return consulta.Id;
        }
    }
}
