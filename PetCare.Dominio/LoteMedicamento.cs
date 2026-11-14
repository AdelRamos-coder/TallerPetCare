namespace PetCare
{
    public class LoteMedicamento : IFacturable
    {
        private const int StockMinimo = 10;

        public string Codigo { get; }

        public string NombreMedicamento { get; }

        public TipoMedicamento Tipo { get; }

        public DateOnly FechaVencimiento { get; }

        public decimal PrecioUnitario { get; }

        public bool EnNevera { get; }

        public int StockActual { get; private set; }

        public LoteMedicamento(string codigo, string nombreMedicamento, TipoMedicamento tipo,
                               DateOnly fechaVencimiento, decimal precioUnitario, int stockInicial,
                               bool enNevera = false)
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

            if (stockInicial < 0)
            {
                throw new ArgumentException("El stock inicial no puede ser negativo.");
            }

            Codigo = codigo.Trim();
            NombreMedicamento = nombreMedicamento.Trim();
            Tipo = tipo;
            FechaVencimiento = fechaVencimiento;
            PrecioUnitario = precioUnitario;
            StockActual = stockInicial;
            EnNevera = enNevera;
        }

        public bool EstaVencido(DateOnly fecha)
        {
            return FechaVencimiento < fecha;
        }

        public bool EstaVencido()
        {
            return EstaVencido(DateOnly.FromDateTime(DateTime.Today));
        }

        public void RecibirUnidades(int cantidad)
        {
            if (cantidad <= 0)
            {
                throw new ArgumentException("La cantidad a recibir debe ser mayor que cero.");
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

        public bool NecesitaReabastecimiento()
        {
            return StockActual < StockMinimo;
        }

        public decimal CalcularValorInventario()
        {
            return StockActual * PrecioUnitario;
        }

        public string ObtenerDescripcion()
        {
            return $"{NombreMedicamento} – Lote {Codigo}";
        }

        public decimal ObtenerPrecioUnitario()
        {
            return PrecioUnitario;
        }

        public void ValidarFacturacion(int cantidad)
        {
            if (cantidad <= 0)
            {
                throw new ArgumentException("La cantidad de unidades debe ser mayor que cero.");
            }

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
