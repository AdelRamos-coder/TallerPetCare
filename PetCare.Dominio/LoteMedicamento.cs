namespace PetCare
{
    /// <summary>
    /// Lote de un medicamento en la bodega de la farmacia.
    /// Se puede registrar ya vencido para llevar el control, pero un lote vencido no se vende.
    /// </summary>
    public class LoteMedicamento : ProductoFarmacia
    {
        private const int StockMinimo = 10;

        /// <summary>Tipo de medicamento.</summary>
        public TipoMedicamento Tipo { get; }

        /// <summary>Fecha de vencimiento del lote.</summary>
        public DateOnly FechaVencimiento { get; }

        /// <summary>Indica si el lote se guarda en nevera.</summary>
        public bool EnNevera { get; }

        /// <summary>Unidades que quedan en bodega.</summary>
        public int StockActual { get; private set; }

        /// <summary>
        /// Registra un lote nuevo.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// Si falta el código o el nombre, si el precio no es mayor que cero o si el stock inicial es negativo.
        /// </exception>
        public LoteMedicamento(string codigo, string nombreMedicamento, TipoMedicamento tipo,
                               DateOnly fechaVencimiento, decimal precioUnitario, int stockInicial,
                               bool enNevera = false)
            : base(codigo, nombreMedicamento, precioUnitario)
        {
            if (stockInicial < 0)
            {
                throw new ArgumentException("El stock inicial no puede ser negativo.");
            }

            Tipo = tipo;
            FechaVencimiento = fechaVencimiento;
            StockActual = stockInicial;
            EnNevera = enNevera;
        }

        /// <summary>
        /// Constructor vacío que solo usa Entity Framework Core al leer de la base de datos.
        /// </summary>
        private LoteMedicamento()
        {
        }

        /// <summary>
        /// Indica si el lote ya estaba vencido en la fecha dada.
        /// </summary>
        public bool EstaVencido(DateOnly fecha)
        {
            return FechaVencimiento < fecha;
        }

        /// <summary>
        /// Indica si el lote está vencido hoy.
        /// </summary>
        public bool EstaVencido()
        {
            return EstaVencido(DateOnly.FromDateTime(DateTime.Today));
        }

        /// <summary>
        /// Suma unidades al lote.
        /// </summary>
        /// <exception cref="ArgumentException">Si la cantidad no es mayor que cero.</exception>
        public void RecibirUnidades(int cantidad)
        {
            if (cantidad <= 0)
            {
                throw new ArgumentException("La cantidad a recibir debe ser mayor que cero.");
            }

            StockActual += cantidad;
        }

        /// <summary>
        /// Saca unidades del lote.
        /// </summary>
        /// <returns>false si la cantidad no es válida, no alcanza el stock o el lote está vencido.</returns>
        public bool DespacharUnidades(int cantidad)
        {
            if (cantidad <= 0 || cantidad > StockActual || EstaVencido())
            {
                return false;
            }

            StockActual -= cantidad;
            return true;
        }

        /// <summary>
        /// Indica si quedan menos de 10 unidades.
        /// </summary>
        public bool NecesitaReabastecimiento()
        {
            return StockActual < StockMinimo;
        }

        /// <summary>
        /// Valor de lo que hay en bodega: unidades por precio.
        /// </summary>
        public decimal CalcularValorInventario()
        {
            return StockActual * PrecioUnitario;
        }

        /// <summary>
        /// Ejemplo: "Amoxicilina 500 mg – Lote L-203".
        /// </summary>
        public override string ObtenerDescripcion()
        {
            return $"{NombreMedicamento} – Lote {Codigo}";
        }

        /// <summary>
        /// Además de la regla de todo producto, un lote vencido no se vende
        /// y no se pueden vender más unidades de las que hay.
        /// </summary>
        /// <exception cref="ArgumentException">Si la cantidad no es mayor que cero.</exception>
        /// <exception cref="InvalidOperationException">Si el lote está vencido o no alcanzan las unidades.</exception>
        public override void ValidarFacturacion(int cantidad)
        {
            base.ValidarFacturacion(cantidad);

            if (EstaVencido())
            {
                throw new InvalidOperationException(
                    $"El lote {Codigo} de {NombreMedicamento} venció el " +
                    $"{FechaVencimiento:dd/MM/yyyy}. No se puede vender ni una unidad.");
            }

            if (cantidad > StockActual)
            {
                throw new InvalidOperationException(
                    $"El lote {Codigo} solo tiene {StockActual} unidades. No se pueden vender {cantidad}.");
            }
        }
    }
}
