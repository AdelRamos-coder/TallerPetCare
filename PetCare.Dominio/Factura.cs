namespace PetCare
{
    /// <summary>
    /// Documento de cobro. Junta en un solo lugar consultas, hospitalizaciones
    /// y productos de farmacia. Solo conoce IFacturable para describir y cobrar,
    /// así que un servicio o producto nuevo no obliga a cambiar esta clase.
    /// </summary>
    public class Factura
    {
        private readonly List<RenglonFactura> _renglones = new();

        /// <summary>Identificador que asigna la base de datos.</summary>
        public int Id { get; private set; }

        /// <summary>Número de la factura, por ejemplo FAC-1001.</summary>
        public string Numero { get; }

        /// <summary>Fecha de emisión.</summary>
        public DateTime FechaEmision { get; }

        /// <summary>Nombre del cliente.</summary>
        public string NombreCliente { get; }

        /// <summary>Usuario que emitió la factura (el que tenía la sesión abierta).</summary>
        public Usuario UsuarioEmisor { get; }

        /// <summary>Indica si la factura fue anulada.</summary>
        public bool Anulada { get; private set; }

        /// <summary>Renglones de la factura. Solo se leen.</summary>
        public IReadOnlyList<RenglonFactura> Renglones => _renglones.AsReadOnly();

        /// <summary>Suma de los subtotales.</summary>
        public decimal Total => _renglones.Sum(r => r.Subtotal);

        /// <summary>
        /// Crea una factura vacía a nombre del usuario que la emite.
        /// </summary>
        /// <exception cref="ArgumentException">Si falta el número o el cliente.</exception>
        /// <exception cref="ArgumentNullException">Si no se indica el usuario emisor.</exception>
        public Factura(string numero, DateTime fechaEmision, string nombreCliente, Usuario usuarioEmisor)
        {
            if (string.IsNullOrWhiteSpace(numero))
            {
                throw new ArgumentException("El número de la factura es obligatorio");
            }

            if (string.IsNullOrWhiteSpace(nombreCliente))
            {
                throw new ArgumentException("El nombre del cliente es obligatorio");
            }

            ArgumentNullException.ThrowIfNull(usuarioEmisor);

            Numero = numero.Trim();
            FechaEmision = fechaEmision;
            NombreCliente = nombreCliente.Trim();
            UsuarioEmisor = usuarioEmisor;
        }

        /// <summary>
        /// Constructor vacío que solo usa Entity Framework Core al leer de la base de datos.
        /// </summary>
        private Factura()
        {
            Numero = string.Empty;
            NombreCliente = string.Empty;
            UsuarioEmisor = null!;
        }

        /// <summary>
        /// Agrega un servicio (consulta, hospitalización y, a futuro, peluquería o vacunación).
        /// La recepcionista no indica cantidad: el sistema asume 1.
        /// </summary>
        /// <exception cref="InvalidOperationException">Si la factura está anulada o el servicio no se puede cobrar.</exception>
        public void Agregar(Servicio servicio)
        {
            ArgumentNullException.ThrowIfNull(servicio);
            ValidarQueAdmiteRenglones();

            servicio.ValidarFacturacion(1);

            _renglones.Add(new RenglonFactura(servicio));
        }

        /// <summary>
        /// Agrega un producto de farmacia (lote de medicamento y, a futuro, alimentos o accesorios)
        /// con la cantidad que se lleva el cliente. Cada producto valida la suya.
        /// </summary>
        /// <exception cref="ArgumentException">Si la cantidad no es mayor que cero.</exception>
        /// <exception cref="InvalidOperationException">Si la factura está anulada o el producto no se puede vender.</exception>
        public void Agregar(ProductoFarmacia producto, int cantidad)
        {
            ArgumentNullException.ThrowIfNull(producto);
            ValidarQueAdmiteRenglones();

            producto.ValidarFacturacion(cantidad);

            _renglones.Add(new RenglonFactura(producto, cantidad));
        }

        /// <summary>
        /// Anula la factura. No toca los servicios ni los lotes facturados: siguen existiendo
        /// y se pueden facturar de nuevo en otra factura.
        /// </summary>
        /// <returns>false si ya estaba anulada.</returns>
        /// <exception cref="InvalidOperationException">Si quien anula no es administrador.</exception>
        public bool Anular(Usuario usuario)
        {
            ArgumentNullException.ThrowIfNull(usuario);

            if (!usuario.EsAdministrador)
            {
                throw new InvalidOperationException("Solo el administrador puede anular facturas");
            }

            if (Anulada)
            {
                return false;
            }

            Anulada = true;
            return true;
        }

        private void ValidarQueAdmiteRenglones()
        {
            if (Anulada)
            {
                throw new InvalidOperationException("No se pueden agregar renglones a una factura anulada");
            }
        }
    }
}
