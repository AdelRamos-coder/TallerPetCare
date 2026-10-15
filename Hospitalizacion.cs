namespace PetCare
{
    public class Hospitalizacion : Servicio
    {
        public Jaula Jaula { get; }
        public decimal TarifaDiaria { get; }
        public DateTime? FechaAlta { get; private set; }

        public Hospitalizacion(string codigo, string nombreMascota, DateTime fechaIngreso, Jaula jaula)
            : base(codigo, nombreMascota, fechaIngreso)
        {
            ArgumentNullException.ThrowIfNull(jaula);

            Jaula = jaula;
            TarifaDiaria = jaula.TarifaDia;

            jaula.IngresarPaciente(this);
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
            Jaula.Liberar();
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
