using System.ComponentModel.DataAnnotations;

namespace PetCareWeb.Modelos
{
    /// <summary>Datos para agregar una consulta u hospitalización a la factura (F4).</summary>
    public class ModeloRenglonServicio
    {
        [Range(1, int.MaxValue, ErrorMessage = "Elige una cita u hospitalización.")]
        public int ServicioId { get; set; }
    }
}
