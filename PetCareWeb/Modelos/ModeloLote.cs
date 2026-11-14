using System.ComponentModel.DataAnnotations;
using PetCare;

namespace PetCareWeb.Modelos
{
    /// <summary>Datos del formulario de registro de lote (F3).</summary>
    public class ModeloLote
    {
        [Required(ErrorMessage = "El código es obligatorio.")]
        [StringLength(20, ErrorMessage = "El código admite máximo 20 caracteres.")]
        public string Codigo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre del medicamento es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre admite máximo 100 caracteres.")]
        public string NombreMedicamento { get; set; } = string.Empty;

        [Required(ErrorMessage = "Elige el tipo de medicamento.")]
        public TipoMedicamento? Tipo { get; set; }

        [Required(ErrorMessage = "La fecha de vencimiento es obligatoria.")]
        public DateOnly? FechaVencimiento { get; set; }

        [Required(ErrorMessage = "El precio por unidad es obligatorio.")]
        [Range(typeof(decimal), "1", "100000000", ErrorMessage = "El precio por unidad debe ser mayor que cero.")]
        public decimal? PrecioUnitario { get; set; }

        [Required(ErrorMessage = "Las unidades son obligatorias.")]
        [Range(0, 1000000, ErrorMessage = "Las unidades no pueden ser negativas.")]
        public int? StockInicial { get; set; }

        public bool EnNevera { get; set; }
    }
}
