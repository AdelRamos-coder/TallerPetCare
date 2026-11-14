using Microsoft.EntityFrameworkCore;
using PetCare;

namespace PetCareWeb.Datos
{
    /// <summary>
    /// Contexto de Entity Framework Core de PetCare Web.
    /// Las tablas y restricciones siguen el script BaseDeDatos.sql;
    /// cada tabla tiene su clase de configuración en la carpeta Configuraciones.
    /// </summary>
    public class PetCareDbContext : DbContext
    {
        public PetCareDbContext(DbContextOptions<PetCareDbContext> opciones)
            : base(opciones)
        {
        }

        /// <summary>Usuarios que pueden entrar al sistema.</summary>
        public DbSet<Usuario> Usuarios => Set<Usuario>();

        /// <summary>Jaulas de la clínica.</summary>
        public DbSet<Jaula> Jaulas => Set<Jaula>();

        /// <summary>Facturas emitidas.</summary>
        public DbSet<Factura> Facturas => Set<Factura>();

        /// <summary>Todos los servicios (tabla Servicios, citas y hospitalizaciones).</summary>
        public DbSet<Servicio> Servicios => Set<Servicio>();

        /// <summary>Solo las citas de consulta.</summary>
        public DbSet<CitaVeterinaria> Citas => Set<CitaVeterinaria>();

        /// <summary>Solo las hospitalizaciones.</summary>
        public DbSet<Hospitalizacion> Hospitalizaciones => Set<Hospitalizacion>();

        /// <summary>Todos los productos de farmacia (tabla Productos).</summary>
        public DbSet<ProductoFarmacia> Productos => Set<ProductoFarmacia>();

        /// <summary>Solo los lotes de medicamento.</summary>
        public DbSet<LoteMedicamento> Lotes => Set<LoteMedicamento>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PetCareDbContext).Assembly);
        }
    }
}
