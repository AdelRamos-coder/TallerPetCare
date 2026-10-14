namespace PetCare
{
    public class Cita
    {
        public int Numero { get; }
        public string NombreMascota { get; }
        public string CedulaCliente { get; }
        public string Veterinario { get; }
        public DateTime FechaHora { get; }
        public string Motivo { get; }
        public decimal Tarifa { get; }
        public EstadoCita Estado { get; private set; } = EstadoCita.Programada;

        public Cita(int numero, string nombreMascota, string cedulaCliente, string veterinario, DateTime fechaHora, string motivo, decimal tarifa)
        {
            if (numero <= 0)
            {
                throw new ArgumentException("El numero de cita debe ser mayor que cero");
            }
            if (string.IsNullOrWhiteSpace(nombreMascota))
            {
                throw new ArgumentException("El nombre de la mascota es obligatorio");
            }
            if (string.IsNullOrWhiteSpace(cedulaCliente))
            {
                throw new ArgumentException("La cedula del cliente es obligatoria");
            }
            if (string.IsNullOrWhiteSpace(veterinario))
            {
                throw new ArgumentException("El veterinario es obligatorio");
            }
            if (string.IsNullOrWhiteSpace(motivo))
            {
                throw new ArgumentException("El motivo de la cita es obligatorio");
            }
            if (tarifa <= 0)
            {
                throw new ArgumentException("La tarifa debe ser mayor que cero");
            }

            Numero = numero;
            NombreMascota = nombreMascota.Trim();
            CedulaCliente = cedulaCliente.Trim();
            Veterinario = veterinario.Trim();
            FechaHora = fechaHora;
            Motivo = motivo.Trim();
            Tarifa = tarifa;
        }

        public int CalcularDiasFaltantes()
        {
            return (FechaHora.Date - DateTime.Today).Days;
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

        public bool EstaVigente()
        {
            return Estado == EstadoCita.Programada && CalcularDiasFaltantes() >= 0;
        }

        public string ObtenerFicha()
        {
            return $"Numero: {Numero}\n" +
                   $"Estado: {Estado}\n" +
                   $"Nombre de la mascota: {NombreMascota}\n" +
                   $"Cedula del cliente: {CedulaCliente}\n" +
                   $"Veterinario: {Veterinario}\n" +
                   $"Fecha: {FechaHora:dd/MM/yyyy HH:mm}\n" +
                   $"Motivo: {Motivo}\n" +
                   $"Tarifa: {Tarifa:C0}";
        }
    }
}
