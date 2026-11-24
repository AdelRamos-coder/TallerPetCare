using System.ComponentModel.DataAnnotations;
using PetCare;

namespace PetCareWeb.Modelos
{
    /// <summary>Datos del formulario de registro de lote (F3).</summary>
    public class ModeloLote
    {
        /// <summary>Código del lote, por ejemplo L-301.</summary>
        [Required(ErrorMessage = "El código es obligatorio.")]
        [StringLength(20, ErrorMessage = "El código admite máximo 20 caracteres.")]
        public string Codigo { get; set; } = string.Empty;

        /// <summary>Nombre del medicamento.</summary>
        [Required(ErrorMessage = "El nombre del medicamento es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre admite máximo 100 caracteres.")]
        public string NombreMedicamento { get; set; } = string.Empty;

        /// <summary>Tipo de medicamento.</summary>
        [Required(ErrorMessage = "Elige el tipo de medicamento.")]
        public TipoMedicamento? Tipo { get; set; }

        /// <summary>Fecha de vencimiento del lote.</summary>
        [Required(ErrorMessage = "La fecha de vencimiento es obligatoria.")]
        public DateOnly? FechaVencimiento { get; set; }

        /// <summary>Precio de venta por unidad.</summary>
        [Required(ErrorMessage = "El precio por unidad es obligatorio.")]
        [Range(typeof(decimal), "1", "100000000", ErrorMessage = "El precio por unidad debe ser mayor que cero.")]
        public decimal? PrecioUnitario { get; set; }

        /// <summary>Unidades con las que entra el lote.</summary>
        [Required(ErrorMessage = "Las unidades son obligatorias.")]
        [Range(0, 1000000, ErrorMessage = "Las unidades no pueden ser negativas.")]
        public int? StockInicial { get; set; }

        /// <summary>Si el lote se guarda en nevera.</summary>
        public bool EnNevera { get; set; }
    }
}
