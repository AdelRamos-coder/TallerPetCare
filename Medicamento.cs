using System;

namespace TallerPetcar
{
    /// <summary>
    /// Representa una referencia de la farmacia interna: su codigo fijo, de que
    /// tipo es, en que presentacion viene, cuanto vale y cuantas unidades hay
    /// Impide que el stock quede en negativo
    /// </summary>
    public class Medicamento
    {
        private const int STOCK_MINIMO = 10;

        public string CodigoInterno { get; private init; }
        public TipoMedicamento TipoMedicamento { get; private init; }
        public string NombreComercial { get; private set; }
        public string Presentacion { get; private set; }
        public decimal PrecioUnidad { get; private set; }
        public int StockActual { get; private set; }
        public DateTime FechaVencimiento { get; private set; }
        public bool EnNevera { get; private set; }

        public Medicamento(string codigoInterno, TipoMedicamento tipoMedicamento, string nombreComercial, string presentacion, decimal precioUnidad, int stockActual, DateTime fechaVencimiento, bool enNevera)
        {
            if (string.IsNullOrEmpty(codigoInterno))
            {
                throw new ArgumentException("El codigo del medicamento es obligatorio");
            }

            CodigoInterno = codigoInterno;
            TipoMedicamento = tipoMedicamento;
            NombreComercial = nombreComercial;
            Presentacion = presentacion;
            PrecioUnidad = precioUnidad;
            StockActual = stockActual;
            FechaVencimiento = fechaVencimiento;
            EnNevera = enNevera;
        }

        /// <summary>
        /// Suma al stock lo que llego del proveedor
        /// Rechaza cantidades en cero o negativas, que serian una entrada al reves
        /// </summary>
        public void RecibirUnidades(int cantidad)
        {
            if (cantidad <= 0)
            {
                throw new ArgumentException("La cantidad a recibir no puede ser negativa");
            }

            StockActual += cantidad;
        }

        /// <summary>
        /// Descuenta del stock lo que se despacho
        /// Devuelve false si se piden mas unidades de las que hay, porque el
        /// stock no puede ir a negativo
        /// </summary>
        public bool DespacharUnidades(int cantidad)
        {
            if (cantidad < 0 || cantidad > StockActual)
            {
                return false;
            }

            StockActual -= cantidad;
            return true;
        }

        public bool EstaVencido()
        {
            return FechaVencimiento.Date < DateTime.Now.Date;
        }

        /// <summary>
        /// Avisa cuando hay que hacer pedido
        /// El minimo de diez unidades es la regla de la casa, igual para todas
        /// las referencias, por eso vive dentro del metodo y no es un atributo
        /// de cada medicamento
        /// </summary>
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