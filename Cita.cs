using System;

namespace TallerPetcar
{
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

        public int CalcularDiasFaltantes()
        {
            TimeSpan diferencia = FechaHora.Date - DateTime.Today;
            return diferencia.Days;
        }

        public bool MarcarAtendida()
        {
            if (Estado == EstadoCita.Programada)
            {
                Estado = EstadoCita.Atendida;
                return true;
            }
            return false;
        }
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
