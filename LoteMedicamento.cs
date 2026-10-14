namespace PetCare
{
    public class LoteMedicamento : IFacturable
    {
        public string Codigo { get; }

        public string NombreMedicamento { get; }

        public DateOnly FechaVencimiento { get; }

        public decimal PrecioUnitario { get; }

        public LoteMedicamento(string codigo, string nombreMedicamento,
                               DateOnly fechaVencimiento, decimal precioUnitario)
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
            FechaVencimiento = fechaVencimiento;
            PrecioUnitario = precioUnitario;
        }

        public bool EstaVencido(DateOnly fecha)
        {
            return FechaVencimiento < fecha;
        }

        public bool EstaVencido()
        {
            return EstaVencido(DateOnly.FromDateTime(DateTime.Today));
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
        }
    }
}
