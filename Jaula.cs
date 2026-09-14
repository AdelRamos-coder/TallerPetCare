using System;

namespace PetCar
{
    public class Jaula
    {
        public required string Codigo { get; init; }
        public TamanoJaula Tamano {  get; init; }
        public decimal TarifaDia { get; set; }

        public bool Ocupado { get; set; }
        public string? NombreMascota { get; set; }
        public int DiasEstancia { get; set; }

        public Jaula(string codigo, TamanoJaula tamano, decimal tarifaDia)
        {
            if (string.IsNullOrEmpty(codigo))
            {
                throw new ArgumentException("El codigo de la jaula es obligatorio.");
            }

            if (TarifaDia <= 0)
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

      
    }
}
