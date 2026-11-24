namespace PetCare
{
    /// <summary>
    /// Lugar físico donde se queda una mascota hospitalizada.
    /// Su tamaño define la tarifa por día y no puede tener dos animales a la vez.
    /// </summary>
    public class Jaula
    {
        private static readonly Dictionary<TamanoJaula, decimal> TarifasPorTamano = new()
        {
            { TamanoJaula.Pequeno, 25000m },
            { TamanoJaula.Mediano, 38000m },
            { TamanoJaula.Grande, 52000m }
        };

        /// <summary>
        /// Tarifa por día de una jaula del tamaño indicado.
        /// La usa la página pública para mostrar los mismos precios que cobra el sistema.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Si el tamaño no existe.</exception>
        public static decimal TarifaDiaPara(TamanoJaula tamano)
        {
            return TarifasPorTamano.TryGetValue(tamano, out decimal tarifa)
                ? tarifa
                : throw new ArgumentOutOfRangeException(nameof(tamano), tamano, "El tamaño de la jaula no es válido");
        }

        /// <summary>Identificador que asigna la base de datos.</summary>
        public int Id { get; private set; }

        /// <summary>Código de la jaula, por ejemplo J-07.</summary>
        public string Codigo { get; }

        /// <summary>Tamaño de la jaula.</summary>
        public TamanoJaula Tamano { get; }

        /// <summary>Tarifa por día según el tamaño.</summary>
        public decimal TarifaDia { get; }

        /// <summary>
        /// Hospitalización que está usando la jaula en este momento.
        /// No se guarda en la base de datos: allá la jaula está ocupada
        /// cuando tiene una hospitalización sin fecha de alta.
        /// </summary>
        public Hospitalizacion? Ocupante { get; private set; }

        /// <summary>Indica si la jaula tiene un paciente en este momento.</summary>
        public bool Ocupado => Ocupante != null;

        /// <summary>
        /// Crea una jaula con la tarifa que corresponde a su tamaño.
        /// </summary>
        /// <exception cref="ArgumentException">Si falta el código o el tamaño no es válido.</exception>
        public Jaula(string codigo, TamanoJaula tamano)
        {
            if (string.IsNullOrWhiteSpace(codigo))
            {
                throw new ArgumentException("El código de la jaula es obligatorio");
            }
            if (!TarifasPorTamano.ContainsKey(tamano))
            {
                throw new ArgumentException("El tamaño de la jaula no es válido");
            }

            Codigo = codigo.Trim();
            Tamano = tamano;
            TarifaDia = TarifasPorTamano[tamano];
        }

        /// <summary>
        /// Constructor vacío que solo usa Entity Framework Core al leer de la base de datos.
        /// </summary>
        private Jaula()
        {
            Codigo = string.Empty;
        }

        /// <summary>
        /// Ocupa la jaula con una hospitalización.
        /// </summary>
        /// <exception cref="InvalidOperationException">Si la jaula ya está ocupada.</exception>
        internal void IngresarPaciente(Hospitalizacion hospitalizacion)
        {
            ArgumentNullException.ThrowIfNull(hospitalizacion);

            if (Ocupante != null)
            {
                throw new InvalidOperationException(
                    $"La jaula {Codigo} ya está ocupada por '{Ocupante.NombreMascota}', no se puede ingresar otro paciente");
            }

            Ocupante = hospitalizacion;
        }

        /// <summary>
        /// Deja la jaula libre cuando la mascota recibe el alta.
        /// </summary>
        internal void Liberar()
        {
            Ocupante = null;
        }

        /// <summary>
        /// Resumen de la jaula para mostrar en pantalla.
        /// </summary>
        public string ConsultarEstado()
        {
            if (Ocupante == null)
            {
                return $"Jaula {Codigo} [{Tamano}] - Tarifa: {TarifaDia:C0}/dia | LIBRE";
            }

            return $"Jaula {Codigo} [{Tamano}] - Tarifa: {TarifaDia:C0}/dia | " +
                   $"OCUPADA por '{Ocupante.NombreMascota}' | Dias: {Ocupante.CalcularDias()} | " +
                   $"Cuenta actual: {Ocupante.CalcularCosto():C0}";
        }
    }
}
