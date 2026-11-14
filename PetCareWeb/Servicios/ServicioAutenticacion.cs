using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PetCare;
using PetCareWeb.Datos;

namespace PetCareWeb.Servicios
{
    /// <summary>
    /// Revisa usuario y contraseña contra la base de datos.
    /// La contraseña se compara con PasswordHasher de ASP.NET Core (PBKDF2), nunca en texto plano.
    /// </summary>
    public class ServicioAutenticacion
    {
        private readonly IDbContextFactory<PetCareDbContext> _fabrica;
        private readonly PasswordHasher<Usuario> _hasher = new();

        public ServicioAutenticacion(IDbContextFactory<PetCareDbContext> fabrica)
        {
            _fabrica = fabrica;
        }

        /// <summary>
        /// Devuelve el usuario si las credenciales son correctas; si no, null.
        /// No dice si lo equivocado fue el usuario o la contraseña.
        /// </summary>
        public async Task<Usuario?> ValidarAsync(string nombreUsuario, string contrasena)
        {
            await using PetCareDbContext db = await _fabrica.CreateDbContextAsync();

            Usuario? usuario = await db.Usuarios
                .FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario.Trim());

            if (usuario == null)
            {
                return null;
            }

            PasswordVerificationResult resultado =
                _hasher.VerifyHashedPassword(usuario, usuario.ContrasenaHash, contrasena);

            if (resultado == PasswordVerificationResult.Failed)
            {
                return null;
            }

            if (resultado == PasswordVerificationResult.SuccessRehashNeeded)
            {
                usuario.CambiarContrasenaHash(_hasher.HashPassword(usuario, contrasena));
                await db.SaveChangesAsync();
            }

            return usuario;
        }
    }
}
