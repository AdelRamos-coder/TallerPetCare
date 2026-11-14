namespace PetCare
{
    /// <summary>
    /// Consulta veterinaria programada para una mascota.
    /// Nace con 30 minutos, sin veterinario y en estado Programada.
    /// </summary>
    public class CitaVeterinaria : Servicio
    {
        private const int DuracionMinima = 15;
        private const int DuracionMaxima = 180;
        private const int MinutosBase = 30;
        private const decimal TarifaBase = 60000m;
        private const decimal TarifaMinutoAdicional = 1500m;

        private int _duracionMinutos;

        /// <summary>Cédula del dueño de la mascota.</summary>
        public string CedulaCliente { get; }

        /// <summary>Motivo de la consulta.</summary>
        public string Motivo { get; }

        /// <summary>
        /// Duración de la consulta en minutos. Solo acepta valores entre 15 y 180;
        /// si el valor no sirve, la cita conserva la duración que tenía.
        /// </summary>
        /// <exception cref="ArgumentException">Si la duración está fuera de 15 a 180 minutos.</exception>
        public int DuracionMinutos
        {
            get => _duracionMinutos;
            set
            {
                if (value < DuracionMinima || value > DuracionMaxima)
                {
                    throw new ArgumentException($"La duración debe estar entre {DuracionMinima} y {DuracionMaxima} minutos");
                }

                _duracionMinutos = value;
            }
        }

        /// <summary>Veterinario que atiende la cita. Se asigna una sola vez.</summary>
        public string? Veterinario { get; private set; }

        /// <summary>Estado de la cita: Programada, Atendida o Cancelada.</summary>
        public EstadoCita Estado { get; private set; } = EstadoCita.Programada;

        /// <summary>
        /// Registra una cita nueva.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// Si falta el código, la mascota, la cédula o el motivo, o si la duración está fuera de 15 a 180 minutos.
        /// </exception>
        public CitaVeterinaria(string codigo, string nombreMascota, string cedulaCliente, DateTime fecha,
                               string motivo, int duracionMinutos = MinutosBase)
            : base(codigo, nombreMascota, fecha)
        {
            if (string.IsNullOrWhiteSpace(cedulaCliente))
            {
                throw new ArgumentException("La cédula del cliente es obligatoria");
            }
            if (string.IsNullOrWhiteSpace(motivo))
            {
                throw new ArgumentException("El motivo de la cita es obligatorio");
            }

            CedulaCliente = cedulaCliente.Trim();
            Motivo = motivo.Trim();
            DuracionMinutos = duracionMinutos;
        }

        /// <summary>
        /// Constructor vacío que solo usa Entity Framework Core al leer de la base de datos.
        /// </summary>
        private CitaVeterinaria()
        {
            CedulaCliente = string.Empty;
            Motivo = string.Empty;
        }

        /// <summary>
        /// Asigna el veterinario que atiende la cita. Solo se puede hacer una vez
        /// y solo mientras la cita está programada.
        /// </summary>
        /// <returns>true si quedó asignado; false si la cita ya tenía veterinario o ya no está programada.</returns>
        /// <exception cref="ArgumentException">Si el nombre del veterinario está vacío.</exception>
        public bool ConfirmarVeterinario(string veterinario)
        {
            if (string.IsNullOrWhiteSpace(veterinario))
            {
                throw new ArgumentException("El veterinario es obligatorio");
            }
            if (Veterinario != null || Estado != EstadoCita.Programada)
            {
                return false;
            }

            Veterinario = veterinario.Trim();
            return true;
        }

        /// <summary>
        /// Marca la cita como atendida.
        /// </summary>
        /// <returns>false si la cita ya no estaba programada.</returns>
        public bool MarcarAtendida()
        {
            if (Estado != EstadoCita.Programada)
            {
                return false;
            }

            Estado = EstadoCita.Atendida;
            return true;
        }

        /// <summary>
        /// Cancela la cita.
        /// </summary>
        /// <returns>false si la cita ya no estaba programada.</returns>
        public bool MarcarCancelada()
        {
            if (Estado != EstadoCita.Programada)
            {
                return false;
            }

            Estado = EstadoCita.Cancelada;
            return true;
        }

        /// <summary>
        /// Días que faltan para la cita (negativo si ya pasó).
        /// </summary>
        public int CalcularDiasFaltantes()
        {
            return (Fecha.Date - DateTime.Today).Days;
        }

        /// <summary>
        /// Indica si la cita sigue programada y su fecha no ha pasado.
        /// </summary>
        public bool EstaVigente()
        {
            return Estado == EstadoCita.Programada && CalcularDiasFaltantes() >= 0;
        }

        /// <summary>
        /// $60.000 hasta 30 minutos y $1.500 por cada minuto adicional.
        /// Ejemplo: 45 minutos valen $82.500.
        /// </summary>
        public override decimal CalcularCosto()
        {
            int minutosAdicionales = Math.Max(0, DuracionMinutos - MinutosBase);
            return TarifaBase + minutosAdicionales * TarifaMinutoAdicional;
        }

        /// <summary>
        /// Ejemplo: "Consulta – Firulais – 29/09/2026 10:00 – Dr. Restrepo".
        /// </summary>
        public override string ObtenerDescripcion()
        {
            return $"Consulta – {base.ObtenerDescripcion()} – {Fecha:dd/MM/yyyy HH:mm} – {Veterinario ?? "Sin veterinario"}";
        }

        /// <summary>
        /// Además de la regla de todo servicio, una cita cancelada no se cobra.
        /// </summary>
        /// <exception cref="ArgumentException">Si la cantidad es distinta de 1.</exception>
        /// <exception cref="InvalidOperationException">Si la cita está cancelada.</exception>
        public override void ValidarFacturacion(int cantidad)
        {
            base.ValidarFacturacion(cantidad);

            if (Estado == EstadoCita.Cancelada)
            {
                throw new InvalidOperationException($"La cita {Codigo} está cancelada y no se puede facturar");
            }
        }
    }
}
