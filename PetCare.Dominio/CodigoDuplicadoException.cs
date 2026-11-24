namespace PetCare
{
    /// <summary>
    /// Ya existe un registro con el mismo código. Se lanza tanto cuando el servicio lo detecta antes
    /// de guardar como cuando la base de datos rechaza el registro por la restricción de unicidad.
    /// </summary>
    public class CodigoDuplicadoException : ReglaNegocioException
    {
        /// <summary>Código repetido.</summary>
        public string Codigo { get; }

        /// <summary>Crea la excepción con el mensaje para el usuario.</summary>
        /// <param name="codigo">Código repetido.</param>
        public CodigoDuplicadoException(string codigo)
            : base($"Ya existe un registro con el código {codigo}. Usa otro código.")
        {
            Codigo = codigo;
        }

        /// <summary>Crea la excepción con el mensaje y la excepción que la originó.</summary>
        /// <param name="codigo">Código repetido.</param>
        /// <param name="causa">Excepción original, por ejemplo la que lanza la base de datos.</param>
        public CodigoDuplicadoException(string codigo, Exception causa)
            : base($"Ya existe un registro con el código {codigo}. Usa otro código.", causa)
        {
            Codigo = codigo;
        }
    }
}
