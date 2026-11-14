using System.ComponentModel.DataAnnotations;

namespace PetCareWeb.Modelos
{
    /// <summary>Datos para agregar un medicamento a la factura (F4).</summary>
    public class ModeloRenglonProducto
    {
        [Range(1, int.MaxValue, ErrorMessage = "Elige un lote.")]
        public int ProductoId { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        [Range(1, 100000, ErrorMessage = "La cantidad debe ser un número entero mayor que cero.")]
        public int? Cantidad { get; set; } = 1;
    }
}
