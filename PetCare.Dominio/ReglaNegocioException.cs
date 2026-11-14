namespace PetCare
{
    /// <summary>
    /// Una regla del negocio de la clínica impide la operación (por ejemplo, una duración fuera de rango
    /// o cobrar una cita cancelada). El mensaje está escrito para mostrarse tal cual al usuario.
    /// Hereda de InvalidOperationException porque la operación no es válida en el estado actual,
    /// así el código que ya atrapaba InvalidOperationException sigue funcionando.
    /// </summary>
    public class ReglaNegocioException : InvalidOperationException
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
