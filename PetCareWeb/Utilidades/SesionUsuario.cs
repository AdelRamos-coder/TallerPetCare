using System.Security.Claims;
using PetCare;

namespace PetCareWeb.Utilidades
{
    /// <summary>
    /// Lectura de los datos del usuario que tiene la sesión abierta.
    /// Los datos se guardan como claims en la cookie al iniciar sesión.
    /// </summary>
    public static class SesionUsuario
    {
        /// <summary>Claim con el nombre completo del usuario.</summary>
        public const string ClaimNombreCompleto = "NombreCompleto";

        /// <summary>Id del usuario en la tabla Usuarios.</summary>
        /// <exception cref="InvalidOperationException">Si no hay sesión iniciada.</exception>
        public static int ObtenerUsuarioId(this ClaimsPrincipal usuario)
        {
            string? valor = usuario.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(valor, out int id))
            {
                throw new InvalidOperationException("No hay una sesión iniciada");
            }

            return id;
        }

        /// <summary>Nombre completo, por ejemplo "Diana Gomez".</summary>
        public static string ObtenerNombreCompleto(this ClaimsPrincipal usuario)
        {
            return usuario.FindFirstValue(ClaimNombreCompleto) ?? usuario.Identity?.Name ?? string.Empty;
        }

        /// <summary>Rol guardado en la sesión ("Recepcionista" o "Administrador").</summary>
        public static string? ObtenerRol(this ClaimsPrincipal usuario)
        {
            return usuario.FindFirstValue(ClaimTypes.Role);
        }

        /// <summary>Indica si el usuario es administrador.</summary>
        public static bool EsAdministrador(this ClaimsPrincipal usuario)
        {
            return usuario.IsInRole(nameof(RolUsuario.Administrador));
        }
    }
}
