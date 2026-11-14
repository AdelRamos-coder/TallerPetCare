using PetCare;

namespace PetCareWeb.Components.Compartidos
{
    /// <summary>
    /// Un elemento facturable con el motivo por el que hoy no se puede cobrar (si lo hay).
    /// El motivo lo da el propio objeto al validar una unidad: cada clase aplica su regla.
    /// </summary>
    public record FilaInventario(IFacturable Item, string? MotivoBloqueo)
    {
        /// <summary>Indica si hoy se puede agregar a una factura.</summary>
        public bool Facturable => MotivoBloqueo == null;

        /// <summary>Pregunta al elemento si se puede facturar una unidad.</summary>
        public static FilaInventario Crear(IFacturable item)
        {
            try
            {
                item.ValidarFacturacion(1);
                return new FilaInventario(item, null);
            }
            catch (ArgumentException ex)
            {
                return new FilaInventario(item, ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return new FilaInventario(item, ex.Message);
            }
        }
    }
}
