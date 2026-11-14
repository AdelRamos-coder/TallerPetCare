namespace PetCare
{
    public class CitaVeterinaria : Servicio
    {
        private const int DuracionMinima = 15;
        private const int DuracionMaxima = 180;
        private const int MinutosBase = 30;
        private const decimal TarifaBase = 60000m;
        private const decimal TarifaMinutoAdicional = 1500m;

        public string CedulaCliente { get; }
        public string Motivo { get; }
        public int DuracionMinutos { get; }
        public string? Veterinario { get; private set; }
        public EstadoCita Estado { get; private set; } = EstadoCita.Programada;

        public CitaVeterinaria(string codigo, string nombreMascota, string cedulaCliente, DateTime fecha,
                               string motivo, int duracionMinutos = MinutosBase)
            : base(codigo, nombreMascota, fecha)
        {
            if (string.IsNullOrWhiteSpace(cedulaCliente))
            {
                throw new ArgumentException("La cedula del cliente es obligatoria");
            }
            if (string.IsNullOrWhiteSpace(motivo))
            {
                throw new ArgumentException("El motivo de la cita es obligatorio");
            }
            if (duracionMinutos < DuracionMinima || duracionMinutos > DuracionMaxima)
            {
                throw new ArgumentException($"La duracion debe estar entre {DuracionMinima} y {DuracionMaxima} minutos");
            }

            CedulaCliente = cedulaCliente.Trim();
            Motivo = motivo.Trim();
            DuracionMinutos = duracionMinutos;
        }

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

        public bool MarcarAtendida()
        {
            if (Estado != EstadoCita.Programada)
            {
                return false;
            }

            Estado = EstadoCita.Atendida;
            return true;
        }

        public bool MarcarCancelada()
        {
            if (Estado != EstadoCita.Programada)
            {
                return false;
            }

            Estado = EstadoCita.Cancelada;
            return true;
        }

        public int CalcularDiasFaltantes()
        {
            return (Fecha.Date - DateTime.Today).Days;
        }

        public bool EstaVigente()
        {
            return Estado == EstadoCita.Programada && CalcularDiasFaltantes() >= 0;
        }

        public override decimal CalcularCosto()
        {
            int minutosAdicionales = Math.Max(0, DuracionMinutos - MinutosBase);
            return TarifaBase + minutosAdicionales * TarifaMinutoAdicional;
        }

        public override string ObtenerDescripcion()
        {
            return $"Consulta – {base.ObtenerDescripcion()} – {Fecha:dd/MM/yyyy HH:mm} – {Veterinario ?? "Sin veterinario"}";
        }

        public override void ValidarFacturacion(int cantidad)
        {
            base.ValidarFacturacion(cantidad);

            if (Estado == EstadoCita.Cancelada)
            {
                throw new InvalidOperationException($"La cita {Codigo} esta cancelada, no se puede facturar");
            }
        }
    }
}
