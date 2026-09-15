
using System;

namespace TallerPetcar
{
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
                throw new ArgumentException("El codigo del medicamento es obligatorio.");
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

        public void RecibirUnidades(int cantidad)
        {
            if (cantidad <= 0)
            {
                throw new ArgumentException("La cantidad a recibir no puede ser negativa.");
            }
                
            StockActual += cantidad;
        }

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