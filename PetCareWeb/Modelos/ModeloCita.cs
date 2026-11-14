using System.ComponentModel.DataAnnotations;

namespace PetCareWeb.Modelos
{
    /// <summary>Datos del formulario de registro de cita (F1).</summary>
    public class ModeloCita
    {
        [Required(ErrorMessage = "El código es obligatorio.")]
        public string Codigo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre de la mascota es obligatorio.")]
        public string NombreMascota { get; set; } = string.Empty;

        [Required(ErrorMessage = "La cédula del cliente es obligatoria.")]
        public string CedulaCliente { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de la cita es obligatoria.")]
        public DateTime? Fecha { get; set; }

        [Required(ErrorMessage = "El motivo es obligatorio.")]
        public string Motivo { get; set; } = string.Empty;
    }
}
