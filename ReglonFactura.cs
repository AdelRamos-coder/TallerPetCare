namespace PetCare
{
    public sealed class RenglonFactura
    {
        public string Descripcion { get; }
 
        public int Cantidad { get; }
 
        public decimal Subtotal { get; }
 
        internal RenglonFactura(string descripcion, int cantidad, decimal subtotal)
        {
            Descripcion = descripcion;
            Cantidad = cantidad;
            Subtotal = subtotal;
        }
    }
}
