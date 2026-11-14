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
        public ReglaNegocioException(string mensaje)
            : base(mensaje)
        {
        }

        public ReglaNegocioException(string mensaje, Exception causa)
            : base(mensaje, causa)
        {
        }
    }
}
