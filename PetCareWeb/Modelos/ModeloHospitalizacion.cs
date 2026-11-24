using System.ComponentModel.DataAnnotations;

namespace PetCareWeb.Modelos
{
    /// <summary>Datos del formulario de ingreso (F2).</summary>
    public class ModeloHospitalizacion
    {
        /// <summary>Código de la hospitalización, por ejemplo HOS-101.</summary>
        [Required(ErrorMessage = "El código es obligatorio.")]
        [StringLength(20, ErrorMessage = "El código admite máximo 20 caracteres.")]
        public string Codigo { get; set; } = string.Empty;

        /// <summary>Nombre de la mascota.</summary>
        [Required(ErrorMessage = "El nombre de la mascota es obligatorio.")]
        [StringLength(80, ErrorMessage = "El nombre de la mascota admite máximo 80 caracteres.")]
        public string NombreMascota { get; set; } = string.Empty;

        /// <summary>Fecha y hora de ingreso.</summary>
        [Required(ErrorMessage = "La fecha de ingreso es obligatoria.")]
        public DateTime? FechaIngreso { get; set; }

        /// <summary>Jaula elegida; de ella sale la tarifa diaria.</summary>
        [Range(1, int.MaxValue, ErrorMessage = "Elige una jaula libre.")]
        public int JaulaId { get; set; }
    }
}
