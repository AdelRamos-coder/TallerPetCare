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
        /// <summary>Minutos que cubre la tarifa base.</summary>
        public const int MinutosBase = 30;

        /// <summary>Precio de la consulta hasta <see cref="MinutosBase"/> minutos.</summary>
        public const decimal TarifaBase = 60000m;

        /// <summary>Precio de cada minuto después de <see cref="MinutosBase"/>.</summary>
        public const decimal TarifaMinutoAdicional = 1500m;

        /// <summary>Largo máximo de la cédula (columna CedulaCliente, NVARCHAR(20)).</summary>
        public const int LargoCedula = 20;

        /// <summary>Largo máximo del motivo (columna Motivo, NVARCHAR(150)).</summary>
        public const int LargoMotivo = 150;

        /// <summary>Largo máximo del veterinario (columna VeterinarioAsignado, NVARCHAR(100)).</summary>
        public const int LargoVeterinario = 100;

        private int _duracionMinutos;

        /// <summary>Cédula del dueño de la mascota.</summary>
        public string CedulaCliente { get; }

        /// <summary>Motivo de la consulta.</summary>
        public string Motivo { get; }

        /// <summary>
        /// Duración de la consulta en minutos. Solo acepta valores entre 15 y 180;
        /// si el valor no sirve, la cita conserva la duración que tenía.
        /// </summary>
        /// <exception cref="ReglaNegocioException">Si la duración está fuera de 15 a 180 minutos.</exception>
        public int DuracionMinutos
        {
            get => _duracionMinutos;
            set
            {
                if (value < DuracionMinima || value > DuracionMaxima)
                {
                    throw new ReglaNegocioException(
                        $"La duración debe estar entre {DuracionMinima} y {DuracionMaxima} minutos");
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
        /// Si falta el código, la mascota, la cédula o el motivo, si alguno supera el largo de su columna
        /// o si la cédula tiene algo distinto de números.
        /// </exception>
        /// <exception cref="ReglaNegocioException">
        /// Si la fecha no está entre hoy y un año hacia adelante, o si la duración está fuera de 15 a 180 minutos.
        /// </exception>
        public CitaVeterinaria(string codigo, string nombreMascota, string cedulaCliente, DateTime fecha,
                               string motivo, int duracionMinutos = MinutosBase)
            : base(codigo, nombreMascota, fecha)
        {
            if (string.IsNullOrWhiteSpace(cedulaCliente))
            {
                throw new ArgumentException("La cédula del cliente es obligatoria");
            }

            string cedula = cedulaCliente.Trim();

            if (cedula.Length > LargoCedula)
            {
                throw new ArgumentException($"La cédula admite máximo {LargoCedula} caracteres");
            }
            if (!cedula.All(char.IsAsciiDigit))
            {
                throw new ArgumentException("La cédula solo puede tener números, sin puntos, espacios ni letras");
            }
            if (string.IsNullOrWhiteSpace(motivo))
            {
                throw new ArgumentException("El motivo de la cita es obligatorio");
            }
            if (motivo.Trim().Length > LargoMotivo)
            {
                throw new ArgumentException($"El motivo admite máximo {LargoMotivo} caracteres");
            }

            ValidarFechaProgramada(fecha);

            CedulaCliente = cedula;
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
        /// <exception cref="ArgumentException">Si el nombre del veterinario está vacío o es demasiado largo.</exception>
        public bool ConfirmarVeterinario(string veterinario)
        {
            if (string.IsNullOrWhiteSpace(veterinario))
            {
                throw new ArgumentException("El nombre del veterinario no puede estar vacío");
            }
            if (veterinario.Trim().Length > LargoVeterinario)
            {
                throw new ArgumentException($"El nombre del veterinario admite máximo {LargoVeterinario} caracteres");
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
            return CostoPara(DuracionMinutos);
        }

        /// <summary>
        /// Precio de una consulta de la duración indicada: tarifa base hasta
        /// <see cref="MinutosBase"/> minutos más cada minuto adicional.
        /// Ejemplo: 45 minutos valen $82.500.
        /// </summary>
        public static decimal CostoPara(int minutos)
        {
            int minutosAdicionales = Math.Max(0, minutos - MinutosBase);
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
        /// <exception cref="ReglaNegocioException">Si la cita está cancelada.</exception>
        public override void ValidarFacturacion(int cantidad)
        {
            base.ValidarFacturacion(cantidad);

            if (Estado == EstadoCita.Cancelada)
            {
                throw new ReglaNegocioException($"La cita {Codigo} está cancelada y no se puede facturar");
            }
        }

        /// <summary>
        /// Una cita se programa desde hoy y hasta un año hacia adelante.
        /// </summary>
        /// <exception cref="ReglaNegocioException">Si la fecha queda por fuera de ese rango.</exception>
        private static void ValidarFechaProgramada(DateTime fecha)
        {
            DateTime hoy = DateTime.Today;
            DateTime limite = hoy.AddYears(1);

            if (fecha.Date < hoy || fecha.Date > limite)
            {
                throw new ReglaNegocioException(
                    $"La fecha de la cita debe estar entre hoy y el {limite:dd/MM/yyyy}");
            }
        }
    }
}
