using System;

namespace TallerPetcar
{
    /// <summary>
    /// Representa una consulta agendada en la recepcion de la clinica: quien la
    /// trae, que mascota, con que veterinario, cuando y por cuanto
    /// Controla el ciclo de estados Programada - Atendida - Cancelada
    /// </summary>
    public class Cita
    {
        public int Numero { get; private init; }
        public string NombreMascota { get; private set; }
        public string CedulaCliente { get; private set; }
        public string Veterinario { get; private set; }
        public DateTime FechaHora { get; private set; }
        public string Motivo { get; private set; }
        public decimal Tarifa { get; private set; }
        public EstadoCita Estado { get; private set; } = EstadoCita.Programada;

        public Cita(int numero, string nombreMascota, string cedulaCliente, string veterinario, DateTime fechaHora, string motivo, decimal tarifa)
        {
            if (tarifa < 0)
            {
                throw new ArgumentException("La tarifa no puede ser negativa");
            }
            if (numero <= 0)
            {
                throw new ArgumentException("El numero de cita debe ser mayor que cero");
            }
            if (string.IsNullOrWhiteSpace(cedulaCliente))
            {
                throw new ArgumentException("La cedula del cliente es obligatoria");
            }

            Numero = numero;
            NombreMascota = nombreMascota;
            CedulaCliente = cedulaCliente.Trim();
            Veterinario = veterinario;
            FechaHora = fechaHora;
            Motivo = motivo;
            Tarifa = tarifa;
        }

        /// <summary>
        /// Dias que faltan para la cita
        /// Devuelve 0 si es hoy y un numero negativo si la fecha ya paso,
        /// el signo distingue los dos casos
        /// </summary>
        public int CalcularDiasFaltantes()
        {
            TimeSpan diferencia = FechaHora.Date - DateTime.Today;
            return diferencia.Days;
        }

        /// <summary>
        /// Marca la cita como atendida
        /// Devuelve false si ya no estaba programada, para que el sistema avise
        /// en vez de hacerlo callado
        /// </summary>
        public bool MarcarAtendida()
        {
            if (Estado == EstadoCita.Programada)
            {
                Estado = EstadoCita.Atendida;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Cancela la cita
        /// Rechaza cancelar una ya atendida porque se perderia la factura,
        /// y rechaza cancelar dos veces la misma cita
        /// </summary>
        public bool MarcarCancelada()
        {
            if (Estado == EstadoCita.Atendida)
            {
                return false;
            }
            if (Estado == EstadoCita.Cancelada)
            {
                return false;
            }
            Estado = EstadoCita.Cancelada;
            return true;
        }

        /// <summary>
        /// Una cita sigue en pie si esta programada y su fecha no ha pasado
        /// La cita de hoy cuenta como vigente
        /// </summary>
        public bool EstaVigente()
        {
            return Estado == EstadoCita.Programada && CalcularDiasFaltantes() >= 0;
        }

        public void MostrarFicha()
        {
            Console.WriteLine($"Numero:{Numero}");
            Console.WriteLine($"Estado: {Estado}");
            Console.WriteLine($"Nombre de la mascota: {NombreMascota}");
            Console.WriteLine($"Cedula del cliente: {CedulaCliente}");
            Console.WriteLine($"Veterinario: {Veterinario}");
            Console.WriteLine($"Fecha:{FechaHora:dd/MM/yyyy HH:mm}");
            Console.WriteLine($"Motivo: {Motivo}");
            Console.WriteLine($"Tarifa: {Tarifa:C}");
        }
    }
}