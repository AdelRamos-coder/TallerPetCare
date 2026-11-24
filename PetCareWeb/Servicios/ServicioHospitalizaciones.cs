using Microsoft.EntityFrameworkCore;
using PetCare;
using PetCareWeb.Datos;

namespace PetCareWeb.Servicios
{
    /// <summary>
    /// Cómo está una jaula en este momento, para la pantalla de hospitalizaciones.
    /// </summary>
    public record EstadoJaula(int Id, string Codigo, TamanoJaula Tamano, decimal TarifaDia,
                              string? CodigoHospitalizacion, string? NombreOcupante)
    {
        /// <summary>Indica si la jaula tiene una hospitalización sin alta.</summary>
        public bool Ocupada => CodigoHospitalizacion != null;
    }

    /// <summary>
    /// Operaciones del formulario F2 (hospitalizaciones).
    /// </summary>
    public class ServicioHospitalizaciones
    {
        private readonly IDbContextFactory<PetCareDbContext> _fabrica;

        /// <summary>Recibe por inyección de dependencias la fábrica que crea un contexto corto por operación.</summary>
        public ServicioHospitalizaciones(IDbContextFactory<PetCareDbContext> fabrica)
        {
            _fabrica = fabrica;
        }

        /// <summary>Todas las hospitalizaciones con su jaula; primero las que siguen activas.</summary>
        public async Task<List<Hospitalizacion>> ListarAsync()
        {
            await using PetCareDbContext db = await _fabrica.CreateDbContextAsync();

            return await db.Hospitalizaciones
                .AsNoTracking()
                .Include(h => h.Jaula)
                .OrderBy(h => h.FechaAlta != null)
                .ThenByDescending(h => h.Fecha)
                .ToListAsync();
        }

        /// <summary>Las 14 jaulas con su ocupante actual, si lo tienen.</summary>
        public async Task<List<EstadoJaula>> ListarJaulasAsync()
        {
            await using PetCareDbContext db = await _fabrica.CreateDbContextAsync();

            List<Jaula> jaulas = await db.Jaulas
                .AsNoTracking()
                .OrderBy(j => j.Codigo)
                .ToListAsync();

            var activas = await db.Hospitalizaciones
                .AsNoTracking()
                .Where(h => h.FechaAlta == null)
                .Select(h => new
                {
                    JaulaId = EF.Property<int>(h, "JaulaId"),
                    h.Codigo,
                    h.NombreMascota
                })
                .ToListAsync();

            return jaulas
                .Select(j =>
                {
                    var activa = activas.FirstOrDefault(a => a.JaulaId == j.Id);
                    return new EstadoJaula(j.Id, j.Codigo, j.Tamano, j.TarifaDia, activa?.Codigo, activa?.NombreMascota);
                })
                .ToList();
        }

        /// <summary>
        /// Registra el ingreso de una mascota a una jaula libre. La tarifa la fija la jaula.
        /// </summary>
        /// <exception cref="ArgumentException">Si faltan datos.</exception>
        /// <exception cref="InvalidOperationException">Si el código ya existe o la jaula está ocupada.</exception>
        public async Task<Hospitalizacion> RegistrarIngresoAsync(string codigo, string nombreMascota,
                                                                 DateTime fechaIngreso, int jaulaId)
        {
            await using PetCareDbContext db = await _fabrica.CreateDbContextAsync();

            string codigoLimpio = codigo.Trim();

            if (await db.Servicios.AnyAsync(s => s.Codigo == codigoLimpio))
            {
                throw new InvalidOperationException(
                    $"Ya existe un servicio con el código {codigoLimpio}. Usa otro código.");
            }

            Jaula jaula = await db.Jaulas.FirstOrDefaultAsync(j => j.Id == jaulaId)
                ?? throw new InvalidOperationException("La jaula elegida no existe.");

            string? ocupante = await db.Hospitalizaciones
                .Where(h => h.FechaAlta == null && EF.Property<int>(h, "JaulaId") == jaulaId)
                .Select(h => h.NombreMascota)
                .FirstOrDefaultAsync();

            if (ocupante != null)
            {
                throw new InvalidOperationException(
                    $"La jaula {jaula.Codigo} ya está ocupada por {ocupante}. Elige otra jaula.");
            }

            var hospitalizacion = new Hospitalizacion(codigo, nombreMascota, fechaIngreso, jaula);

            db.Hospitalizaciones.Add(hospitalizacion);
            await db.SaveChangesAsync();

            return hospitalizacion;
        }

        /// <summary>
        /// Registra el alta. Devuelve false si la hospitalización ya tenía alta (se conserva la primera).
        /// </summary>
        /// <exception cref="ArgumentException">Si la fecha de alta es anterior al ingreso.</exception>
        public async Task<bool> DarDeAltaAsync(int hospitalizacionId, DateTime fechaAlta)
        {
            await using PetCareDbContext db = await _fabrica.CreateDbContextAsync();

            Hospitalizacion hospitalizacion = await db.Hospitalizaciones
                .Include(h => h.Jaula)
                .FirstOrDefaultAsync(h => h.Id == hospitalizacionId)
                ?? throw new InvalidOperationException("La hospitalización ya no existe. Actualiza la lista.");

            bool registrada = hospitalizacion.DarDeAlta(fechaAlta);

            if (registrada)
            {
                await db.SaveChangesAsync();
            }

            return registrada;
        }
    }
}
