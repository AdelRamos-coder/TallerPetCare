namespace PetCare
{
    /// <summary>
    /// Lo común de todo servicio que presta la clínica a una mascota
    /// (consulta, hospitalización y, a futuro, vacunación o peluquería).
    /// Es abstracta: no existe "un servicio" a secas, siempre es uno concreto.
    /// </summary>
    public abstract class Servicio : IFacturable
    {
        /// <summary>Identificador que asigna la base de datos.</summary>
        public int Id { get; private set; }

        /// <summary>Código único del servicio, por ejemplo CIT-009 u HOS-132.</summary>
        public string Codigo { get; }

        /// <summary>Nombre de la mascota atendida.</summary>
        public string NombreMascota { get; }

        /// <summary>Fecha del servicio: la programada en la cita o la de ingreso en la hospitalización.</summary>
        public DateTime Fecha { get; }

        /// <summary>
        /// Constructor que usan las clases hijas. Valida lo común a todo servicio.
        /// </summary>
        /// <exception cref="ArgumentException">Si el código o el nombre de la mascota están vacíos.</exception>
        protected Servicio(string codigo, string nombreMascota, DateTime fecha)
        {
            if (string.IsNullOrWhiteSpace(codigo))
            {
                throw new ArgumentException("El código del servicio es obligatorio");
            }
            if (string.IsNullOrWhiteSpace(nombreMascota))
            {
                throw new ArgumentException("El nombre de la mascota es obligatorio");
            }

            Codigo = codigo.Trim();
            NombreMascota = nombreMascota.Trim();
            Fecha = fecha;
        }

        /// <summary>
        /// Constructor vacío que solo usa Entity Framework Core al leer de la base de datos.
        /// </summary>
        protected Servicio()
        {
            Codigo = string.Empty;
            NombreMascota = string.Empty;
        }

        /// <summary>
        /// Cuánto cuesta el servicio. Cada servicio lo calcula a su manera.
        /// </summary>
        public abstract decimal CalcularCosto();

        /// <summary>
        /// Parte común de la descripción: el nombre de la mascota.
        /// Las clases hijas la completan con lo suyo.
        /// </summary>
        public virtual string ObtenerDescripcion()
        {
            return NombreMascota;
        }

        /// <summary>
        /// Un servicio se cobra como una unidad, así que su precio unitario es su costo.
        /// </summary>
        public decimal ObtenerPrecioUnitario()
        {
            return CalcularCosto();
        }

        /// <summary>
        /// Un servicio siempre se cobra como una sola unidad.
        /// </summary>
        /// <exception cref="ArgumentException">Si la cantidad es distinta de 1.</exception>
        public virtual void ValidarFacturacion(int cantidad)
        {
            if (cantidad != 1)
            {
                throw new ArgumentException("Un servicio siempre se cobra como una unidad");
            }
        }
    }
}
