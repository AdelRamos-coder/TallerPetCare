namespace PetCare
{
    /// <summary>
    /// Una regla del negocio de la clínica impide la operación (por ejemplo, una duración fuera de rango,
    /// una fecha de cita por fuera del año siguiente o cobrar una cita cancelada).
    /// El mensaje está escrito para mostrarse tal cual al usuario.
    /// Hereda de Exception para separarla de los errores del sistema: quien la atrapa sabe que
    /// no es una falla técnica sino algo que el usuario puede corregir.
    /// </summary>
    public class ReglaNegocioException : Exception
    {
        /// <summary>Crea la excepción con el mensaje para el usuario.</summary>
        /// <param name="mensaje">Mensaje para mostrar al usuario.</param>
        public ReglaNegocioException(string mensaje)
            : base(mensaje)
        {
        }

        /// <summary>Crea la excepción con el mensaje y la excepción que la originó.</summary>
        /// <param name="mensaje">Mensaje para mostrar al usuario.</param>
        /// <param name="causa">Excepción original, por ejemplo la que lanza la base de datos.</param>
        public ReglaNegocioException(string mensaje, Exception causa)
            : base(mensaje, causa)
        {
        }
    }
}
