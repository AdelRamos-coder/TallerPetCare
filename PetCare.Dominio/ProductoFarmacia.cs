namespace PetCare
{
    /// <summary>
    /// Lo común de todo producto que vende la farmacia de la clínica
    /// (hoy el lote de medicamento; a futuro alimentos o accesorios).
    /// Es abstracta: siempre se vende un producto concreto.
    /// </summary>
    public abstract class ProductoFarmacia : IFacturable
    {
        /// <summary>Identificador que asigna la base de datos.</summary>
        public int Id { get; private set; }

        /// <summary>Código único del producto, por ejemplo L-203.</summary>
        public string Codigo { get; }

        /// <summary>Nombre con el que se vende el producto.</summary>
        public string NombreMedicamento { get; }

        /// <summary>Precio de una unidad.</summary>
        public decimal PrecioUnitario { get; }

        /// <summary>
        /// Constructor que usan las clases hijas. Valida lo común a todo producto.
        /// </summary>
        /// <exception cref="ArgumentException">Si falta el código o el nombre, o si el precio no es mayor que cero.</exception>
        protected ProductoFarmacia(string codigo, string nombreMedicamento, decimal precioUnitario)
        {
            if (string.IsNullOrWhiteSpace(codigo))
            {
                throw new ArgumentException("El código del lote es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(nombreMedicamento))
            {
                throw new ArgumentException("El nombre del medicamento es obligatorio.");
            }

            if (precioUnitario <= 0)
            {
                throw new ArgumentException("El precio por unidad debe ser mayor que cero.");
            }

            Codigo = codigo.Trim();
            NombreMedicamento = nombreMedicamento.Trim();
            PrecioUnitario = precioUnitario;
        }

        /// <summary>
        /// Constructor vacío que solo usa Entity Framework Core al leer de la base de datos.
        /// </summary>
        protected ProductoFarmacia()
        {
            Codigo = string.Empty;
            NombreMedicamento = string.Empty;
        }

        /// <summary>
        /// Texto del renglón de la factura. Cada producto lo arma a su manera.
        /// </summary>
        public abstract string ObtenerDescripcion();

        /// <summary>
        /// Precio de una unidad del producto.
        /// </summary>
        public decimal ObtenerPrecioUnitario()
        {
            return PrecioUnitario;
        }

        /// <summary>
        /// Regla común a todo producto: se vende al menos una unidad.
        /// </summary>
        /// <exception cref="ArgumentException">Si la cantidad no es mayor que cero.</exception>
        public virtual void ValidarFacturacion(int cantidad)
        {
            if (cantidad <= 0)
            {
                throw new ArgumentException("La cantidad de unidades debe ser mayor que cero.");
            }
        }
    }
}
