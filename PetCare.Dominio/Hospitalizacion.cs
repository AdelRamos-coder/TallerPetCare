namespace PetCare
{
    /// <summary>
    /// Estadía de una mascota en una jaula de la clínica.
    /// La jaula es el lugar físico; la hospitalización es la estadía que se cobra por días.
    /// La tarifa diaria sale del tamaño de la jaula y queda fija al ingresar.
    /// </summary>
    public class Hospitalizacion : Servicio
    {
        /// <summary>Jaula donde está (o estuvo) la mascota.</summary>
        public Jaula Jaula { get; }

        /// <summary>Tarifa por día, copiada de la jaula al momento del ingreso.</summary>
        public decimal TarifaDiaria { get; }

        /// <summary>Fecha de alta. Mientras no tenga, la mascota sigue hospitalizada.</summary>
        public DateTime? FechaAlta { get; private set; }

        /// <summary>
        /// Registra el ingreso de una mascota a una jaula libre.
        /// </summary>
        /// <exception cref="ArgumentException">Si falta el código o la mascota.</exception>
        /// <exception cref="ArgumentNullException">Si no se indica la jaula.</exception>
        /// <exception cref="InvalidOperationException">Si la jaula ya está ocupada.</exception>
        public Hospitalizacion(string codigo, string nombreMascota, DateTime fechaIngreso, Jaula jaula)
            : base(codigo, nombreMascota, fechaIngreso)
        {
            ArgumentNullException.ThrowIfNull(jaula);

            Jaula = jaula;
            TarifaDiaria = jaula.TarifaDia;

            jaula.IngresarPaciente(this);
        }

        /// <summary>
        /// Constructor vacío que solo usa Entity Framework Core al leer de la base de datos.
        /// </summary>
        private Hospitalizacion()
        {
            Jaula = null!;
        }

        /// <summary>
        /// Registra el alta y libera la jaula. El alta se da una sola vez.
        /// </summary>
        /// <returns>true si se registró; false si ya tenía alta (se conserva la primera).</returns>
        /// <exception cref="ArgumentException">Si la fecha de alta es anterior al ingreso.</exception>
        public bool DarDeAlta(DateTime fechaAlta)
        {
            if (FechaAlta != null)
            {
                return false;
            }
            if (fechaAlta < Fecha)
            {
                throw new ArgumentException("La fecha de alta no puede ser anterior al ingreso");
            }

            FechaAlta = fechaAlta;
            Jaula.Liberar();
            return true;
        }

        /// <summary>
        /// Días que se cobran: hasta el alta o, si sigue hospitalizada, hasta hoy.
        /// Si estuvo menos de un día, se cobra uno como mínimo.
        /// </summary>
        public int CalcularDias()
        {
            DateTime fin = FechaAlta ?? DateTime.Today;
            return Math.Max(1, (fin.Date - Fecha.Date).Days);
        }

        /// <summary>
        /// Días por la tarifa diaria.
        /// </summary>
        public override decimal CalcularCosto()
        {
            return CalcularDias() * TarifaDiaria;
        }

        /// <summary>
        /// Ejemplo: "Hospitalización – Luna – 3 días".
        /// </summary>
        public override string ObtenerDescripcion()
        {
            int dias = CalcularDias();
            string textoDias = dias == 1 ? "1 día" : $"{dias} días";
            return $"Hospitalización – {base.ObtenerDescripcion()} – {textoDias}";
        }
    }
}
