using System;

namespace TallerPetcar
{
    /// <summary>
    /// Representa una jaula de la sala de hospitalizacion: su codigo, tamano y
    /// tarifa diaria, mas el paciente que tiene adentro y los dias que lleva
    /// Impide que entren dos animales a la misma jaula
    /// </summary>
    public class Jaula
    {
        public string Codigo { get; private init; }
        public TamanoJaula Tamano { get; private init; }
        public decimal TarifaDia { get; private set; }

        public bool Ocupado { get; private set; }
        public string? NombreMascota { get; private set; }
        public int DiasEstancia { get; private set; }

        public Jaula(string codigo, TamanoJaula tamano, decimal tarifaDia)
        {
            if (string.IsNullOrEmpty(codigo))
            {
                throw new ArgumentException("El codigo de la jaula es obligatorio");
            }

            if (tarifaDia <= 0)
            {
                throw new ArgumentException("La tarifa por dia debe ser mayor a cero");
            }

            Codigo = codigo;
            Tamano = tamano;
            TarifaDia = tarifaDia;

            Ocupado = false;
            NombreMascota = null;
            DiasEstancia = 0;
        }

        /// <summary>
        /// Ingresa un paciente a la jaula
        /// Rechaza el ingreso si ya hay un animal adentro: meter dos a la misma
        /// jaula no puede volver a pasar
        /// </summary>
        public void IngresarPaciente(string nombreMascota)
        {
            if (Ocupado)
            {
                throw new ArgumentException(
                $"La jaula {Codigo} ya esta ocupada por '{NombreMascota}', " +
                "no se puede ingresar otro paciente");
            }

            if (string.IsNullOrWhiteSpace(nombreMascota))
            {
                throw new ArgumentException("Debe indicar el nombre del animal");
            }

            Ocupado = true;
            NombreMascota = nombreMascota;
            DiasEstancia = 0;
        }

        /// <summary>
        /// Suma un dia de estancia en la ronda de la manana
        /// Si la jaula esta libre no hace nada, porque no hay a quien cobrarle
        /// </summary>
        public void SumarDiaEstancia()
        {
            if (!Ocupado)
            {
                return;
            }

            DiasEstancia++;
        }

        public decimal CalcularCuenta()
        {
            return DiasEstancia * TarifaDia;
        }

        /// <summary>
        /// Da de alta al paciente
        /// Devuelve el total a cobrar y deja la jaula libre, sin ocupante y con
        /// los dias en cero
        /// </summary>
        public decimal DarDeAltaMedica()
        {
            if (!Ocupado)
            {
                throw new InvalidOperationException($"La jaula {Codigo} esta libre, no hay paciente que dar de alta");
            }

            decimal totalCobrar = CalcularCuenta();

            Ocupado = false;
            NombreMascota = null;
            DiasEstancia = 0;

            return totalCobrar;
        }

        public string ConsultarEstado()
        {
            if (Ocupado)
            {
                return $"Jaula {Codigo} [{Tamano}] - Tarifa: {TarifaDia:C}/dia | " +
                $"OCUPADA por '{NombreMascota}' | Dias: {DiasEstancia} | " +
                $"Cuenta actual: {CalcularCuenta():C}";
            }

            return $"Jaula {Codigo} [{Tamano}] - Tarifa: {TarifaDia:C}/dia | LIBRE";
        }
    }
}