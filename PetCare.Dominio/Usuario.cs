namespace PetCare
{
    /// <summary>
    /// Persona del equipo de PetCare que entra al sistema.
    /// La contraseña nunca se guarda tal cual: solo su hash.
    /// </summary>
    public class Usuario
    {
        /// <summary>Identificador que asigna la base de datos.</summary>
        public int Id { get; private set; }

        /// <summary>Nombre con el que inicia sesión, por ejemplo dgomez.</summary>
        public string NombreUsuario { get; }

        /// <summary>Nombre que se muestra en pantalla y en las facturas.</summary>
        public string NombreCompleto { get; }

        /// <summary>Versión protegida (hash) de la contraseña.</summary>
        public string ContrasenaHash { get; private set; }

        /// <summary>Tipo de usuario.</summary>
        public RolUsuario Rol { get; }

        /// <summary>Indica si el usuario puede hacer las tareas del administrador, como anular facturas.</summary>
        public bool EsAdministrador => Rol == RolUsuario.Administrador;

        /// <summary>
        /// Crea un usuario con su contraseña ya convertida en hash.
        /// </summary>
        /// <exception cref="ArgumentException">Si falta el usuario, el nombre o el hash.</exception>
        public Usuario(string nombreUsuario, string nombreCompleto, string contrasenaHash, RolUsuario rol)
        {
            if (string.IsNullOrWhiteSpace(nombreUsuario))
            {
                throw new ArgumentException("El nombre de usuario es obligatorio");
            }
            if (string.IsNullOrWhiteSpace(nombreCompleto))
            {
                throw new ArgumentException("El nombre completo es obligatorio");
            }
            if (string.IsNullOrWhiteSpace(contrasenaHash))
            {
                throw new ArgumentException("La contraseña es obligatoria");
            }

            NombreUsuario = nombreUsuario.Trim();
            NombreCompleto = nombreCompleto.Trim();
            ContrasenaHash = contrasenaHash;
            Rol = rol;
        }

        /// <summary>
        /// Constructor vacío que solo usa Entity Framework Core al leer de la base de datos.
        /// </summary>
        private Usuario()
        {
            NombreUsuario = string.Empty;
            NombreCompleto = string.Empty;
            ContrasenaHash = string.Empty;
        }

        /// <summary>
        /// Reemplaza el hash de la contraseña (por ejemplo, cuando el hash se actualiza a un formato nuevo).
        /// </summary>
        /// <exception cref="ArgumentException">Si el hash está vacío.</exception>
        public void CambiarContrasenaHash(string contrasenaHash)
        {
            if (string.IsNullOrWhiteSpace(contrasenaHash))
            {
                throw new ArgumentException("La contraseña es obligatoria");
            }

            ContrasenaHash = contrasenaHash;
        }
    }
}
