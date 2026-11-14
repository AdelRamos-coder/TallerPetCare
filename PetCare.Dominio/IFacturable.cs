namespace PetCare
{
    /// <summary>
    /// Contrato para todo lo que se puede cobrar en un renglón de la factura.
    /// Lo cumplen los servicios (cita y hospitalización) y los productos de farmacia
    /// (lote de medicamento), que no son de la misma familia: un medicamento
    /// no es un servicio, no se le "presta" a nadie, pero también se cobra.
    /// La factura solo conoce este contrato, así que cuando lleguen peluquería,
    /// vacunación, alimentos o accesorios no hay que tocar la factura.
    /// </summary>
    public interface IFacturable
    {
        /// <summary>
        /// Código que identifica lo que se cobra (código del servicio o del lote).
        /// </summary>
        string Codigo { get; }

        /// <summary>
        /// Texto que sale en el renglón de la factura.
        /// Cada clase lo arma a su manera, por ejemplo
        /// "Amoxicilina 500 mg – Lote L-203".
        /// </summary>
        string ObtenerDescripcion();

        /// <summary>
        /// Precio de una unidad. La factura lo multiplica por la cantidad
        /// para sacar el subtotal del renglón.
        /// </summary>
        decimal ObtenerPrecioUnitario();

        /// <summary>
        /// Revisa si en este momento se puede cobrar la cantidad indicada.
        /// Si no se puede, lanza una excepción con un mensaje claro y no cambia nada.
        /// </summary>
        /// <param name="cantidad">Unidades que se quieren cobrar.</param>
        void ValidarFacturacion(int cantidad);
    }
}
