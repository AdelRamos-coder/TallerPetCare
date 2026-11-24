using System.ComponentModel.DataAnnotations;

namespace PetCareWeb.Modelos
{
    /// <summary>Datos del formulario F0.</summary>
    public class ModeloInicioSesion
    {
        /// <summary>Nombre de usuario.</summary>
        [Required(ErrorMessage = "Escribe tu usuario.")]
        public string NombreUsuario { get; set; } = string.Empty;

        /// <summary>Contraseña escrita por el usuario. No se guarda; solo se compara con el hash.</summary>
        [Required(ErrorMessage = "Escribe tu contraseña.")]
        public string Contrasena { get; set; } = string.Empty;
    }
}
