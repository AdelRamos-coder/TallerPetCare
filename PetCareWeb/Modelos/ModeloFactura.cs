using System.ComponentModel.DataAnnotations;

namespace PetCareWeb.Modelos
{
    /// <summary>Datos para crear una factura (F4). El emisor sale de la sesión.</summary>
    public class ModeloFactura
    {
        [Required(ErrorMessage = "El número de la factura es obligatorio.")]
        [StringLength(20, ErrorMessage = "El número admite máximo 20 caracteres.")]
        public string Numero { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre del cliente es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre del cliente admite máximo 100 caracteres.")]
        public string NombreCliente { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de emisión es obligatoria.")]
        public DateTime? FechaEmision { get; set; } = DateTime.Today;
    }
}
