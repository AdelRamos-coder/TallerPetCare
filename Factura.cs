using System;
using System.Collections.Generic;
using System.Linq;
 
namespace PetCare
{
    public class Factura
    {
        private readonly List<RenglonFactura> _renglones = new();
 
        public string Numero { get; }
 
        public DateTime FechaEmision { get; }
 
        public string NombreCliente { get; }
 
        public bool Anulada { get; private set; }
 
        public IReadOnlyList<RenglonFactura> Renglones => _renglones.AsReadOnly();
 
        public decimal Total => _renglones.Sum(r => r.Subtotal);
 
        public Factura(string numero, DateTime fechaEmision, string nombreCliente)
        {
            if (string.IsNullOrWhiteSpace(numero))
            {
                throw new ArgumentException("El numero de factura es obligatorio");
            }
 
            if (string.IsNullOrWhiteSpace(nombreCliente))
            {
                throw new ArgumentException("El nombre del cliente es obligatorio");
            }
 
            Numero = numero.Trim();
            FechaEmision = fechaEmision;
            NombreCliente = nombreCliente.Trim();
        }
 
        // Servicios (consulta, hospitalizacion y, a futuro, peluqueria, vacunacion...):
        // la recepcionista no indica cantidad, el sistema asume 1.
        public void Agregar(Servicio servicio)
        {
            ArgumentNullException.ThrowIfNull(servicio);
            Agregar(servicio, 1);
        }
 
        // Cualquier cosa facturable (medicamentos y, a futuro, alimentos, accesorios...):
        // la cantidad es obligatoria y cada item valida la suya con ValidarFacturacion.
        public void Agregar(IFacturable item, int cantidad)
        {
            ArgumentNullException.ThrowIfNull(item);
 
            if (Anulada)
            {
                throw new InvalidOperationException("No se pueden agregar renglones a una factura anulada");
            }
 
            item.ValidarFacturacion(cantidad);
 
            _renglones.Add(new RenglonFactura(
                item.ObtenerDescripcion(),
                cantidad,
                item.ObtenerPrecioUnitario() * cantidad));
        }
 
        // Anula la factura. No toca los servicios ni los lotes facturados: siguen existiendo
        // y se pueden facturar de nuevo en otra factura. Devuelve false si ya estaba anulada.
        public bool Anular()
        {
            if (Anulada)
            {
                return false;
            }
 
            Anulada = true;
            return true;
        }
    }
}
 
