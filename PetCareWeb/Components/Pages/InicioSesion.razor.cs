using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using PetCare;
using PetCareWeb.Modelos;
using PetCareWeb.Servicios;
using PetCareWeb.Utilidades;

namespace PetCareWeb.Components.Pages
{
    /// <summary>
    /// F0 · Inicio de sesión (HU-01). Se dibuja sin interactividad porque debe escribir la cookie de sesión.
    /// </summary>
    public partial class InicioSesion
    {
        private const string MensajeCredenciales =
            "Usuario o contraseña incorrectos. Revisa tus datos e inténtalo de nuevo.";

        private const string MensajeSinConexion =
            "No pudimos comunicarnos con la base de datos. Intenta de nuevo en unos minutos o avisa a la administración.";

        [Inject]
        private ServicioAutenticacion ServicioAutenticacion { get; set; } = default!;

        [Inject]
        private NavigationManager Navegacion { get; set; } = default!;

        [Inject]
        private ILogger<InicioSesion> Logger { get; set; } = default!;

        [CascadingParameter]
        private HttpContext HttpContext { get; set; } = default!;

        [SupplyParameterFromForm]
        private ModeloInicioSesion Modelo { get; set; } = default!;

        [SupplyParameterFromQuery]
        private string? ReturnUrl { get; set; }

        private string? mensajeError;

        protected override void OnInitialized()
        {
            Modelo ??= new ModeloInicioSesion();
        }

        private async Task IngresarAsync()
        {
            Usuario? usuario;

            try
            {
                usuario = await ServicioAutenticacion.ValidarAsync(Modelo.NombreUsuario, Modelo.Contrasena);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Falló la validación del inicio de sesión de {Usuario}", Modelo.NombreUsuario);
                mensajeError = MensajeSinConexion;
                return;
            }

            if (usuario == null)
            {
                mensajeError = MensajeCredenciales;
                return;
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new(ClaimTypes.Name, usuario.NombreUsuario),
                new(SesionUsuario.ClaimNombreCompleto, usuario.NombreCompleto),
                new(ClaimTypes.Role, usuario.Rol.ToString())
            };

            var identidad = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identidad));

            Navegacion.NavigateTo(DestinoSeguro(), forceLoad: true);
        }

        // Solo se vuelve a direcciones de la misma aplicación.
        private string DestinoSeguro()
        {
            if (!string.IsNullOrEmpty(ReturnUrl)
                && ReturnUrl.StartsWith('/')
                && !ReturnUrl.StartsWith("//")
                && !ReturnUrl.StartsWith("/\\")
                && !ReturnUrl.StartsWith("/iniciar-sesion"))
            {
                return ReturnUrl;
            }

            return "/";
        }
    }
}
