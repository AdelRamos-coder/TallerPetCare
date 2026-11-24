using System.ComponentModel.DataAnnotations;

namespace PetCareWeb.Modelos
{
    /// <summary>
    /// Datos del formulario de registro de cita (F1).
    /// Estas validaciones avisan en pantalla antes de enviar; el dominio vuelve a validar al crear la cita.
    /// </summary>
    public class ModeloCita
    {
        /// <summary>Código de la cita, por ejemplo CIT-001.</summary>
        [Required(ErrorMessage = "El código es obligatorio.")]
        [StringLength(20, ErrorMessage = "El código admite máximo 20 caracteres.")]
        public string Codigo { get; set; } = string.Empty;

        /// <summary>Nombre de la mascota.</summary>
        [Required(ErrorMessage = "El nombre de la mascota es obligatorio.")]
        [StringLength(80, ErrorMessage = "El nombre de la mascota admite máximo 80 caracteres.")]
        public string NombreMascota { get; set; } = string.Empty;

        /// <summary>Cédula del dueño de la mascota, solo números.</summary>
        [Required(ErrorMessage = "La cédula del cliente es obligatoria.")]
        [StringLength(20, ErrorMessage = "La cédula admite máximo 20 caracteres.")]
        [RegularExpression(@"^\s*[0-9]+\s*$", ErrorMessage = "La cédula solo puede tener números, sin puntos, espacios ni letras.")]
        public string CedulaCliente { get; set; } = string.Empty;

        /// <summary>Fecha y hora programada de la cita.</summary>
        [Required(ErrorMessage = "La fecha de la cita es obligatoria.")]
        public DateTime? Fecha { get; set; }

        /// <summary>Motivo de la consulta.</summary>
        [Required(ErrorMessage = "El motivo es obligatorio.")]
        [StringLength(150, ErrorMessage = "El motivo admite máximo 150 caracteres.")]
        public string Motivo { get; set; } = string.Empty;
    }
}
