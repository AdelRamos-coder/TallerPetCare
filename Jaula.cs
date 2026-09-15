using System;

namespace TallerPetcar
{
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
                throw new ArgumentException("El codigo de la jaula es obligatorio.");
            }

            if (tarifaDia <= 0)
            {
                throw new ArgumentException("La tarifa por dia debe ser mayor a cero.");
            }

            Codigo = codigo;
            Tamano = tamano;
            TarifaDia = tarifaDia;

            Ocupado = false;
            NombreMascota = null;
            DiasEstancia = 0;
        }

        public void IngresarPaciente(string nombreMascota)
        {
            if (Ocupado)
            {
                throw new InvalidOperationException(
                $"La jaula {Codigo} ya está ocupada por '{NombreMascota}'. " +
                "No se puede ingresar otro paciente.");
            }

            if (string.IsNullOrWhiteSpace(nombreMascota))
            {
                throw new ArgumentException("Debe indicar el nombre del animal.");

            }

            Ocupado = true;
            NombreMascota = nombreMascota;
            DiasEstancia = 0;
        }

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

        public decimal DarDeAltaMedica()
        {
            if (!Ocupado)
            {
                throw new InvalidOperationException($"La jaula {Codigo} está libre; no hay paciente que dar de alta.");
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
                return $"Jaula {Codigo} [{Tamano}] - Tarifa: {TarifaDia:C}/día | " +
                $"OCUPADA por '{NombreMascota}' | Días: {DiasEstancia} | " +
                $"Cuenta actual: {CalcularCuenta():C}";
            }

            return $"Jaula {Codigo} [{Tamano}] - Tarifa: {TarifaDia:C}/día | LIBRE";
        }
    }
}
