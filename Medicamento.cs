namespace PetCare
{
    public class Medicamento
    {
        private const int STOCK_MINIMO = 10;

        public string CodigoInterno { get; }
        public TipoMedicamento TipoMedicamento { get; }
        public string NombreComercial { get; }
        public string Presentacion { get; }
        public decimal PrecioUnidad { get; }
        public int StockActual { get; private set; }
        public DateTime FechaVencimiento { get; }
        public bool EnNevera { get; }

        public Medicamento(string codigoInterno, TipoMedicamento tipoMedicamento, string nombreComercial, string presentacion, decimal precioUnidad, int stockActual, DateTime fechaVencimiento, bool enNevera)
        {
            if (string.IsNullOrWhiteSpace(codigoInterno))
            {
                throw new ArgumentException("El codigo del medicamento es obligatorio");
            }
            if (string.IsNullOrWhiteSpace(nombreComercial))
            {
                throw new ArgumentException("El nombre comercial es obligatorio");
            }
            if (string.IsNullOrWhiteSpace(presentacion))
            {
                throw new ArgumentException("La presentacion es obligatoria");
            }
            if (precioUnidad <= 0)
            {
                throw new ArgumentException("El precio por unidad debe ser mayor que cero");
            }
            if (stockActual < 0)
            {
                throw new ArgumentException("El stock inicial no puede ser negativo");
            }

            CodigoInterno = codigoInterno.Trim();
            TipoMedicamento = tipoMedicamento;
            NombreComercial = nombreComercial.Trim();
            Presentacion = presentacion.Trim();
            PrecioUnidad = precioUnidad;
            StockActual = stockActual;
            FechaVencimiento = fechaVencimiento;
            EnNevera = enNevera;
        }

        public void RecibirUnidades(int cantidad)
        {
            if (cantidad <= 0)
            {
                throw new ArgumentException("La cantidad a recibir debe ser mayor que cero");
            }

            StockActual += cantidad;
        }

        public bool DespacharUnidades(int cantidad)
        {
            if (cantidad <= 0 || cantidad > StockActual || EstaVencido())
            {
                return false;
            }

            StockActual -= cantidad;
            return true;
        }

        public bool EstaVencido()
        {
            return FechaVencimiento.Date < DateTime.Today;
        }

        public bool NecesitaReabastecimiento()
        {
            return StockActual < STOCK_MINIMO;
        }

        public decimal CalcularValorInventario()
        {
            return StockActual * PrecioUnidad;
        }
    }
}
