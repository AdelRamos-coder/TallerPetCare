using System.ComponentModel.DataAnnotations;

namespace PetCareWeb.Modelos
{
    /// <summary>Datos del formulario de ingreso (F2).</summary>
    public class ModeloHospitalizacion
    {
        [Required(ErrorMessage = "El código es obligatorio.")]
        [StringLength(20, ErrorMessage = "El código admite máximo 20 caracteres.")]
        public string Codigo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre de la mascota es obligatorio.")]
        [StringLength(80, ErrorMessage = "El nombre de la mascota admite máximo 80 caracteres.")]
        public string NombreMascota { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de ingreso es obligatoria.")]
        public DateTime? FechaIngreso { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Elige una jaula libre.")]
        public int JaulaId { get; set; }
    }
}
