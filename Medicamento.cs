using System;

namespace PetCar
{
    public class Medicamento
    {
        public const int STOCK_MINIMO = 10;
        public string CodigoInterno { get; init; }
        public  TipoMedicamento TipoMedicamento { get; set; }
        public string NombreComercial { get; set; }
        public string Presentacion { get; set; }
        public decimal PrecioUnidad { get; set; }
        public int StockActual { get; set; }
        public DateTime FechaVencimiento { get; init; }
        public bool EnNevera { get; set; } 

        public Medicamento(string codigoInterno, TipoMedicamento tipoMedicamento, string nombreComercial, string presentacion, decimal precioUnidad, int stockActual, DateTime fechaVencimiento, bool enNevera)
        {
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
            if (cantidad < 0)
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
