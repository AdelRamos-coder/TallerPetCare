using Microsoft.EntityFrameworkCore;
using PetCare;
using PetCareWeb.Datos;

namespace PetCareWeb.Servicios
{
    /// <summary>
    /// Operaciones de los formularios F4 (facturación) y F5 (consulta de facturas).
    /// Las reglas (factura anulada, lote vencido, solo el administrador anula) las aplica Factura.
    /// </summary>
    public class ServicioFacturas
    {
        private const string CollationSinTildes = "Latin1_General_CI_AI";

        private readonly IDbContextFactory<PetCareDbContext> _fabrica;

        /// <summary>Recibe por inyección de dependencias la fábrica que crea un contexto corto por operación.</summary>
        public ServicioFacturas(IDbContextFactory<PetCareDbContext> fabrica)
        {
            _fabrica = fabrica;
        }

        /// <summary>
        /// Crea una factura vacía a nombre del usuario que tiene la sesión abierta.
        /// </summary>
        /// <returns>Id de la factura creada.</returns>
        /// <exception cref="ArgumentException">Si falta el número o el cliente.</exception>
        /// <exception cref="InvalidOperationException">Si el número ya existe.</exception>
        public async Task<int> CrearAsync(string numero, DateTime fechaEmision, string nombreCliente, int usuarioEmisorId)
        {
            await using PetCareDbContext db = await _fabrica.CreateDbContextAsync();

            string numeroLimpio = numero.Trim();

            if (await db.Facturas.AnyAsync(f => f.Numero == numeroLimpio))
            {
                throw new InvalidOperationException(
                    $"Ya existe la factura {numeroLimpio}. Usa otro número.");
            }

            Usuario usuario = await db.Usuarios.FirstOrDefaultAsync(u => u.Id == usuarioEmisorId)
                ?? throw new InvalidOperationException("El usuario de la sesión ya no existe. Inicia sesión de nuevo.");

            var factura = new Factura(numero, fechaEmision, nombreCliente, usuario);

            db.Facturas.Add(factura);
            await db.SaveChangesAsync();

            return factura.Id;
        }

        /// <summary>Factura con su emisor y sus renglones (cada uno con su servicio o producto).</summary>
        public async Task<Factura?> ObtenerAsync(int facturaId)
        {
            await using PetCareDbContext db = await _fabrica.CreateDbContextAsync();

            return await ConsultaCompleta(db)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == facturaId);
        }

        /// <summary>
        /// Agrega un servicio a la factura. La cantidad siempre es 1.
        /// </summary>
        /// <exception cref="InvalidOperationException">Si la factura está anulada o el servicio no se puede cobrar.</exception>
        public async Task AgregarServicioAsync(int facturaId, int servicioId)
        {
            await using PetCareDbContext db = await _fabrica.CreateDbContextAsync();

            Factura factura = await ObtenerParaModificarAsync(db, facturaId);

            Servicio servicio = await db.Servicios.FirstOrDefaultAsync(s => s.Id == servicioId)
                ?? throw new InvalidOperationException("El servicio elegido ya no existe.");

            factura.Agregar(servicio);
            await db.SaveChangesAsync();
        }

        /// <summary>
        /// Agrega un producto de farmacia con su cantidad.
        /// </summary>
        /// <exception cref="ArgumentException">Si la cantidad no es mayor que cero.</exception>
        /// <exception cref="InvalidOperationException">Si la factura está anulada o el producto no se puede vender.</exception>
        public async Task AgregarProductoAsync(int facturaId, int productoId, int cantidad)
        {
            await using PetCareDbContext db = await _fabrica.CreateDbContextAsync();

            Factura factura = await ObtenerParaModificarAsync(db, facturaId);

            ProductoFarmacia producto = await db.Productos.FirstOrDefaultAsync(p => p.Id == productoId)
                ?? throw new InvalidOperationException("El producto elegido ya no existe.");

            factura.Agregar(producto, cantidad);
            await db.SaveChangesAsync();
        }

        /// <summary>
        /// Anula la factura. El dominio rechaza la operación si el usuario no es administrador.
        /// </summary>
        /// <returns>false si la factura ya estaba anulada.</returns>
        /// <exception cref="InvalidOperationException">Si el usuario no es administrador.</exception>
        public async Task<bool> AnularAsync(int facturaId, int usuarioId)
        {
            await using PetCareDbContext db = await _fabrica.CreateDbContextAsync();

            Factura factura = await db.Facturas.FirstOrDefaultAsync(f => f.Id == facturaId)
                ?? throw new InvalidOperationException("La factura ya no existe.");

            Usuario usuario = await db.Usuarios.FirstOrDefaultAsync(u => u.Id == usuarioId)
                ?? throw new InvalidOperationException("El usuario de la sesión ya no existe. Inicia sesión de nuevo.");

            bool anulada = factura.Anular(usuario);

            if (anulada)
            {
                await db.SaveChangesAsync();
            }

            return anulada;
        }

        /// <summary>
        /// Busca por parte del número y por parte del nombre del cliente,
        /// sin distinguir mayúsculas ni tildes. Sin criterios devuelve todas.
        /// </summary>
        public async Task<List<Factura>> BuscarAsync(string? numero, string? cliente)
        {
            await using PetCareDbContext db = await _fabrica.CreateDbContextAsync();

            IQueryable<Factura> consulta = db.Facturas
                .AsNoTracking()
                .Include(f => f.UsuarioEmisor)
                .Include(f => f.Renglones);

            if (!string.IsNullOrWhiteSpace(numero))
            {
                string texto = numero.Trim();
                consulta = consulta.Where(f => f.Numero.Contains(texto));
            }

            if (!string.IsNullOrWhiteSpace(cliente))
            {
                string texto = cliente.Trim();
                consulta = consulta.Where(f =>
                    EF.Functions.Collate(f.NombreCliente, CollationSinTildes).Contains(texto));
            }

            return await consulta
                .OrderByDescending(f => f.FechaEmision)
                .ThenByDescending(f => f.Id)
                .ToListAsync();
        }

        /// <summary>Facturas emitidas hoy, para el resumen del día.</summary>
        public async Task<List<Factura>> ListarDeHoyAsync()
        {
            await using PetCareDbContext db = await _fabrica.CreateDbContextAsync();

            DateTime hoy = DateTime.Today;
            DateTime manana = hoy.AddDays(1);

            return await db.Facturas
                .AsNoTracking()
                .Include(f => f.Renglones)
                .Where(f => f.FechaEmision >= hoy && f.FechaEmision < manana)
                .ToListAsync();
        }

        /// <summary>Servicios que se pueden elegir para un renglón (citas y hospitalizaciones).</summary>
        public async Task<List<Servicio>> ListarServiciosAsync()
        {
            await using PetCareDbContext db = await _fabrica.CreateDbContextAsync();

            return await db.Servicios
                .AsNoTracking()
                .OrderByDescending(s => s.Fecha)
                .ToListAsync();
        }

        /// <summary>Productos de farmacia que se pueden elegir para un renglón.</summary>
        public async Task<List<ProductoFarmacia>> ListarProductosAsync()
        {
            await using PetCareDbContext db = await _fabrica.CreateDbContextAsync();

            return await db.Productos
                .AsNoTracking()
                .OrderBy(p => p.Codigo)
                .ToListAsync();
        }

        private static IQueryable<Factura> ConsultaCompleta(PetCareDbContext db)
        {
            return db.Facturas
                .Include(f => f.UsuarioEmisor)
                .Include(f => f.Renglones).ThenInclude(r => r.Servicio)
                .Include(f => f.Renglones).ThenInclude(r => r.Producto)
                .AsSplitQuery();
        }

        private static async Task<Factura> ObtenerParaModificarAsync(PetCareDbContext db, int facturaId)
        {
            return await db.Facturas
                .Include(f => f.Renglones)
                .FirstOrDefaultAsync(f => f.Id == facturaId)
                ?? throw new InvalidOperationException("La factura ya no existe.");
        }
    }
}
