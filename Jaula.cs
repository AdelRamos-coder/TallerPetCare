namespace PetCare
{
    public class Jaula
    {
        public string Codigo { get; }
        public TamanoJaula Tamano { get; }
        public decimal TarifaDia { get; }

        public bool Ocupado { get; private set; }
        public string? NombreMascota { get; private set; }

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
        }

        public void Liberar()
        {
            if (!Ocupado)
            {
                throw new InvalidOperationException($"La jaula {Codigo} ya esta libre");
            }

            Ocupado = false;
            NombreMascota = null;
        }

        public string ConsultarEstado()
        {
            string estado = Ocupado ? $"OCUPADA por '{NombreMascota}'" : "LIBRE";
            return $"Jaula {Codigo} [{Tamano}] - Tarifa: {TarifaDia:C0}/dia | {estado}";
        }
    }
}
