namespace PetCare
{
    public class Jaula
    {
        public string Codigo { get; }
        public TamanoJaula Tamano { get; }
        public decimal TarifaDia { get; }

        public bool Ocupado { get; private set; }
        public string? NombreMascota { get; private set; }
        public int DiasEstancia { get; private set; }

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

        public void IngresarPaciente(string nombreMascota)
        {
            if (string.IsNullOrWhiteSpace(nombreMascota))
            {
                throw new ArgumentException("Debe indicar el nombre del animal");
            }
            if (Ocupado)
            {
                throw new InvalidOperationException(
                    $"La jaula {Codigo} ya esta ocupada por '{NombreMascota}', no se puede ingresar otro paciente");
            }

            Ocupado = true;
            NombreMascota = nombreMascota.Trim();
            DiasEstancia = 0;
        }

        public void SumarDiaEstancia()
        {
            if (Ocupado)
            {
                DiasEstancia++;
            }
        }

        public decimal CalcularCuenta()
        {
            if (!Ocupado)
            {
                return 0m;
            }

            return Math.Max(1, DiasEstancia) * TarifaDia;
        }

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
            if (!Ocupado)
            {
                return $"Jaula {Codigo} [{Tamano}] - Tarifa: {TarifaDia:C0}/dia | LIBRE";
            }

            return $"Jaula {Codigo} [{Tamano}] - Tarifa: {TarifaDia:C0}/dia | " +
                   $"OCUPADA por '{NombreMascota}' | Dias: {DiasEstancia} | " +
                   $"Cuenta actual: {CalcularCuenta():C0}";
        }
    }
}
