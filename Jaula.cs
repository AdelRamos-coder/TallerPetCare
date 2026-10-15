namespace PetCare
{
    public class Jaula
    {
        public string Codigo { get; }
        public TamanoJaula Tamano { get; }
        public decimal TarifaDia { get; }

        public Hospitalizacion? Ocupante { get; private set; }

        public bool Ocupado => Ocupante != null;

        public Jaula(string codigo, TamanoJaula tamano, decimal tarifaDia)
        {
            if (string.IsNullOrWhiteSpace(codigo))
            {
                throw new ArgumentException("El codigo de la jaula es obligatorio");
            }
            if (tarifaDia <= 0)
            {
                throw new ArgumentException("La tarifa por dia debe ser mayor que cero");
            }

            Codigo = codigo.Trim();
            Tamano = tamano;
            TarifaDia = tarifaDia;
        }

        internal void IngresarPaciente(Hospitalizacion hospitalizacion)
        {
            ArgumentNullException.ThrowIfNull(hospitalizacion);

            if (Ocupante != null)
            {
                throw new InvalidOperationException(
                    $"La jaula {Codigo} ya esta ocupada por '{Ocupante.NombreMascota}', no se puede ingresar otro paciente");
            }

            Ocupante = hospitalizacion;
        }

        internal void Liberar()
        {
            if (Ocupante == null)
            {
                throw new InvalidOperationException($"La jaula {Codigo} ya esta libre");
            }

            Ocupante = null;
        }

        public string ConsultarEstado()
        {
            if (Ocupante == null)
            {
                return $"Jaula {Codigo} [{Tamano}] - Tarifa: {TarifaDia:C0}/dia | LIBRE";
            }

            return $"Jaula {Codigo} [{Tamano}] - Tarifa: {TarifaDia:C0}/dia | " +
                   $"OCUPADA por '{Ocupante.NombreMascota}' | Dias: {Ocupante.CalcularDias()} | " +
                   $"Cuenta actual: {Ocupante.CalcularCosto():C0}";
        }
    }
}
