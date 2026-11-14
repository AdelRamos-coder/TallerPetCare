namespace PetCare
{
    public interface IFacturable
    {
        string Codigo { get; }

        string ObtenerDescripcion();

        decimal ObtenerPrecioUnitario();

        void ValidarFacturacion(int cantidad);
    }
}
