using Microsoft.EntityFrameworkCore;
using PetCare;
using PetCareWeb.Datos;

namespace PetCareWeb.Servicios
{
    /// <summary>
    /// Operaciones del formulario F3 (lotes de medicamento).
    /// </summary>
    public class ServicioLotes
    {
        private readonly IDbContextFactory<PetCareDbContext> _fabrica;

        public ServicioLotes(IDbContextFactory<PetCareDbContext> fabrica)
        {
            _fabrica = fabrica;
        }

        /// <summary>Todos los lotes, ordenados por fecha de vencimiento.</summary>
        public async Task<List<LoteMedicamento>> ListarAsync()
        {
            await using PetCareDbContext db = await _fabrica.CreateDbContextAsync();

            return await db.Lotes
                .AsNoTracking()
                .OrderBy(l => l.FechaVencimiento)
                .ToListAsync();
        }

        /// <summary>
        /// Registra un lote nuevo. Se permite registrar un lote ya vencido (no se podrá vender).
        /// </summary>
        /// <exception cref="ArgumentException">Si faltan datos o el precio no es mayor que cero.</exception>
        /// <exception cref="InvalidOperationException">Si el código ya existe.</exception>
        public async Task<LoteMedicamento> RegistrarAsync(string codigo, string nombreMedicamento, TipoMedicamento tipo,
                                                          DateOnly fechaVencimiento, decimal precioUnitario,
                                                          int stockInicial, bool enNevera)
        {
            await using PetCareDbContext db = await _fabrica.CreateDbContextAsync();

            string codigoLimpio = codigo.Trim();

            if (await db.Productos.AnyAsync(p => p.Codigo == codigoLimpio))
            {
                throw new InvalidOperationException(
                    $"Ya existe un lote con el código {codigoLimpio}. Usa otro código.");
            }

            var lote = new LoteMedicamento(codigo, nombreMedicamento, tipo, fechaVencimiento,
                                           precioUnitario, stockInicial, enNevera);

            db.Lotes.Add(lote);
            await db.SaveChangesAsync();

            return lote;
        }
    }
}
