namespace PetCare
{
    public class Jaula
    {
        private static readonly Dictionary<TamanoJaula, decimal> TarifasPorTamano = new()
        {
            { TamanoJaula.Pequeno, 25000m },
            { TamanoJaula.Mediano, 38000m },
            { TamanoJaula.Grande, 52000m }
        };

        public string Codigo { get; }
        public TamanoJaula Tamano { get; }
        public decimal TarifaDia { get; }

        public Hospitalizacion? Ocupante { get; private set; }

        public bool Ocupado => Ocupante != null;

        public Jaula(string codigo, TamanoJaula tamano)
        {
            if (string.IsNullOrWhiteSpace(codigo))
            {
                throw new ArgumentException("El codigo de la jaula es obligatorio");
            }
            if (!TarifasPorTamano.ContainsKey(tamano))
            {
                throw new ArgumentException("El tamano de la jaula no es valido");
            }

            Codigo = codigo.Trim();
            Tamano = tamano;
            TarifaDia = TarifasPorTamano[tamano];
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
