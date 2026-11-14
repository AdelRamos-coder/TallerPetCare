using Microsoft.EntityFrameworkCore;
using PetCare;
using PetCareWeb.Datos;

namespace PetCareWeb.Servicios
{
    /// <summary>
    /// Lista unificada del formulario F6. Devuelve todo como IFacturable:
    /// la pantalla no necesita saber de qué clase es cada elemento,
    /// así un servicio o producto nuevo aparece aquí sin tocar nada.
    /// </summary>
    public class ServicioInventario
    {
        private readonly IDbContextFactory<PetCareDbContext> _fabrica;

        public ServicioInventario(IDbContextFactory<PetCareDbContext> fabrica)
        {
            _fabrica = fabrica;
        }

        /// <summary>Todos los servicios de la tabla Servicios.</summary>
        public async Task<List<IFacturable>> ListarServiciosAsync()
        {
            await using PetCareDbContext db = await _fabrica.CreateDbContextAsync();

            List<Servicio> servicios = await db.Servicios
                .AsNoTracking()
                .OrderByDescending(s => s.Fecha)
                .ToListAsync();

            return servicios.ToList<IFacturable>();
        }

        /// <summary>Todos los productos de la tabla Productos.</summary>
        public async Task<List<IFacturable>> ListarProductosAsync()
        {
            await using PetCareDbContext db = await _fabrica.CreateDbContextAsync();

            List<ProductoFarmacia> productos = await db.Productos
                .AsNoTracking()
                .OrderBy(p => p.Codigo)
                .ToListAsync();

            return productos.ToList<IFacturable>();
        }
    }
}
