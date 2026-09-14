using System;

namespace TallerPetCare
{
    public class Jaula
    {
        public string Codigo { get; init; }
        public TamanoJaula Tamano { get; init; }
        public decimal TarifaPorDia { get; init; }
        public bool Ocupada { get; private set; } = false;
        public string Ocupante { get; private set; } = string.Empty;
        public int DiasEstancia { get; private set; } = 0;

    }
}