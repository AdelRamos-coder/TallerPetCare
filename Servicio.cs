namespace PetCare
{
    public abstract class Servicio : IFacturable
    {
        public string Codigo { get; }
        public string NombreMascota { get; }
        public DateTime Fecha { get; }

        protected Servicio(string codigo, string nombreMascota, DateTime fecha)
        {
            if (string.IsNullOrWhiteSpace(codigo))
            {
                throw new ArgumentException("El codigo del servicio es obligatorio");
            }
            if (string.IsNullOrWhiteSpace(nombreMascota))
            {
                throw new ArgumentException("El nombre de la mascota es obligatorio");
            }

            Codigo = codigo.Trim();
            NombreMascota = nombreMascota.Trim();
            Fecha = fecha;
        }

        public abstract decimal CalcularCosto();

        public virtual string ObtenerDescripcion()
        {
            return NombreMascota;
        }

        public decimal ObtenerPrecioUnitario()
        {
            return CalcularCosto();
        }

        public void ValidarFacturacion(int cantidad)
        {
            if (cantidad != 1)
            {
                throw new ArgumentException("Un servicio siempre se cobra como una unidad");
            }
        }
    }
}
