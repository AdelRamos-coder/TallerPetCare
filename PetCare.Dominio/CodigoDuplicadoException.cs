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

        public CodigoDuplicadoException(string codigo)
            : base($"Ya existe un registro con el código {codigo}. Usa otro código.")
        {
            Codigo = codigo;
        }

        public CodigoDuplicadoException(string codigo, Exception causa)
            : base($"Ya existe un registro con el código {codigo}. Usa otro código.", causa)
        {
            Codigo = codigo;
        }
    }
}
