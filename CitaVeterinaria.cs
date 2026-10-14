namespace PetCare
{
    public class CitaVeterinaria : Servicio
    {
        private const int DuracionMinima = 15;
        private const int DuracionMaxima = 180;
        private const int MinutosBase = 30;
        private const decimal TarifaBase = 60000m;
        private const decimal TarifaMinutoAdicional = 1500m;

        public int DuracionMinutos { get; }
        public string? Veterinario { get; private set; }

        public CitaVeterinaria(string codigo, string nombreMascota, DateTime fecha, int duracionMinutos = MinutosBase)
            : base(codigo, nombreMascota, fecha)
        {
            if (duracionMinutos < DuracionMinima || duracionMinutos > DuracionMaxima)
            {
                throw new ArgumentException($"La duracion debe estar entre {DuracionMinima} y {DuracionMaxima} minutos");
            }

            DuracionMinutos = duracionMinutos;
        }

        public bool ConfirmarVeterinario(string veterinario)
        {
            if (string.IsNullOrWhiteSpace(veterinario))
            {
                throw new ArgumentException("El veterinario es obligatorio");
            }
            if (Veterinario != null)
            {
                return false;
            }

            Veterinario = veterinario.Trim();
            return true;
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
    }
}
