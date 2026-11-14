namespace PetCare
{
    public sealed class RenglonFactura
    {
        public IFacturable Item { get; }
        public string Descripcion { get; }
 
        public int Cantidad { get; }
 
        public decimal Subtotal { get; }
 
        internal RenglonFactura(IFacturable item, string descripcion, int cantidad, decimal subtotal)
        {
            Item = item;
            Descripcion = descripcion;
            Cantidad = cantidad;
            Subtotal = subtotal;
        }
    }
}
