namespace PetCare
{
    public class Hospitalizacion : Servicio
    {
        public decimal TarifaDiaria { get; }
        public DateTime? FechaAlta { get; private set; }

        public Hospitalizacion(string codigo, string nombreMascota, DateTime fechaIngreso, decimal tarifaDiaria)
            : base(codigo, nombreMascota, fechaIngreso)
        {
            if (tarifaDiaria <= 0)
            {
                throw new ArgumentException("La tarifa diaria debe ser mayor que cero");
            }

            TarifaDiaria = tarifaDiaria;
        }

        public bool DarDeAlta(DateTime fechaAlta)
        {
            if (FechaAlta != null)
            {
                return false;
            }
            if (fechaAlta < Fecha)
            {
                throw new ArgumentException("La fecha de alta no puede ser anterior al ingreso");
            }

            FechaAlta = fechaAlta;
            return true;
        }

        public int CalcularDias()
        {
            DateTime fin = FechaAlta ?? DateTime.Today;
            return Math.Max(1, (fin.Date - Fecha.Date).Days);
        }

        public override decimal CalcularCosto()
        {
            return CalcularDias() * TarifaDiaria;
        }

        public override string ObtenerDescripcion()
        {
            return $"Hospitalizacion – {base.ObtenerDescripcion()} – {CalcularDias()} dias";
        }
    }
}
