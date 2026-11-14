using System.ComponentModel.DataAnnotations;

namespace PetCareWeb.Modelos
{
    /// <summary>Datos del formulario F0.</summary>
    public class ModeloInicioSesion
    {
        [Required(ErrorMessage = "Escribe tu usuario.")]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "Escribe tu contraseña.")]
        public string Contrasena { get; set; } = string.Empty;
    }
}
