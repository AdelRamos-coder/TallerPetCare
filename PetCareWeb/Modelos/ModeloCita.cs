using System.ComponentModel.DataAnnotations;

namespace PetCareWeb.Modelos
{
    /// <summary>
    /// Datos del formulario de registro de cita (F1).
    /// Estas validaciones avisan en pantalla antes de enviar; el dominio vuelve a validar al crear la cita.
    /// </summary>
    public class ModeloCita
    {
        [Required(ErrorMessage = "El código es obligatorio.")]
        [StringLength(20, ErrorMessage = "El código admite máximo 20 caracteres.")]
        public string Codigo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre de la mascota es obligatorio.")]
        [StringLength(80, ErrorMessage = "El nombre de la mascota admite máximo 80 caracteres.")]
        public string NombreMascota { get; set; } = string.Empty;

        [Required(ErrorMessage = "La cédula del cliente es obligatoria.")]
        [StringLength(20, ErrorMessage = "La cédula admite máximo 20 caracteres.")]
        [RegularExpression(@"^\s*[0-9]+\s*$", ErrorMessage = "La cédula solo puede tener números, sin puntos, espacios ni letras.")]
        public string CedulaCliente { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de la cita es obligatoria.")]
        public DateTime? Fecha { get; set; }

        [Required(ErrorMessage = "El motivo es obligatorio.")]
        [StringLength(150, ErrorMessage = "El motivo admite máximo 150 caracteres.")]
        public string Motivo { get; set; } = string.Empty;
    }
}
