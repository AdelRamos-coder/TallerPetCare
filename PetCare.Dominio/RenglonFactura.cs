namespace PetCare
{
    /// <summary>
    /// Renglón de una factura. Viene de un servicio o de un producto, nunca de los dos.
    /// El precio unitario se guarda al momento de agregarlo; la descripción y el subtotal
    /// los da el propio elemento a través de IFacturable.
    /// </summary>
    public sealed class RenglonFactura
    {
        /// <summary>Identificador que asigna la base de datos.</summary>
        public int Id { get; private set; }

        /// <summary>Servicio cobrado, si el renglón es de un servicio.</summary>
        public Servicio? Servicio { get; private set; }

        /// <summary>Producto cobrado, si el renglón es de un producto.</summary>
        public ProductoFarmacia? Producto { get; private set; }

        /// <summary>Unidades cobradas.</summary>
        public int Cantidad { get; private set; }

        /// <summary>Precio de una unidad al momento de agregar el renglón.</summary>
        public decimal PrecioUnitario { get; private set; }

        /// <summary>Lo que se cobra en este renglón, sin importar si es servicio o producto.</summary>
        public IFacturable Item => Servicio != null ? Servicio : Producto!;

        /// <summary>Texto del renglón.</summary>
        public string Descripcion => Item.ObtenerDescripcion();

        /// <summary>Precio unitario por cantidad.</summary>
        public decimal Subtotal => PrecioUnitario * Cantidad;

        internal RenglonFactura(Servicio servicio)
        {
            Servicio = servicio;
            Cantidad = 1;
            PrecioUnitario = servicio.ObtenerPrecioUnitario();
        }

        internal RenglonFactura(ProductoFarmacia producto, int cantidad)
        {
            Producto = producto;
            Cantidad = cantidad;
            PrecioUnitario = producto.ObtenerPrecioUnitario();
        }

        /// <summary>
        /// Constructor vacío que solo usa Entity Framework Core al leer de la base de datos.
        /// </summary>
        private RenglonFactura()
        {
        }
    }
}
