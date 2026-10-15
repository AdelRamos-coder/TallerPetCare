using System.Globalization;
using System.Text;

namespace PetCare
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            CultureInfo.CurrentCulture = new CultureInfo("es-CO");

            Console.WriteLine("=========================================");
            Console.WriteLine("      CLINICA VETERINARIA PETCARE 2.0");
            Console.WriteLine("=========================================");

            if (args.Length > 0 && args[0] == "pruebas")
            {
                PruebasLoteMedicamento.EjecutarTodas();
                return;
            }

            DemostracionPetCare();
        }

        static void DemostracionPetCare()
        {
            Console.WriteLine("\n[1] Consulta de 45 minutos");
            CitaVeterinaria consulta = new CitaVeterinaria("CIT-009", "Rambo", DateTime.Today.AddHours(10), 45);
            bool veterinarioConfirmado = consulta.ConfirmarVeterinario("Dr. Mengueche");
            Console.WriteLine($"  Veterinario confirmado: {veterinarioConfirmado}");
            Console.WriteLine($"  {consulta.ObtenerDescripcion()}");
            Console.WriteLine($"  Costo de la consulta: {consulta.CalcularCosto():C0}");

            Console.WriteLine("\n[2] Hospitalizacion que ingreso hace 3 dias, alta hoy");
            Hospitalizacion hospitalizacion = new Hospitalizacion("HOS-132", "Trico", DateTime.Today.AddDays(-3), 80000m);
            bool altaDada = hospitalizacion.DarDeAlta(DateTime.Today);
            Console.WriteLine($"  Alta registrada: {altaDada}");
            Console.WriteLine($"  {hospitalizacion.ObtenerDescripcion()}");
            Console.WriteLine($"  Costo de la hospitalizacion: {hospitalizacion.CalcularCosto():C0}");

            Console.WriteLine("\n[3] Lotes de medicamento: uno vigente y uno vencido");
            DateOnly hoy = DateOnly.FromDateTime(DateTime.Today);
            LoteMedicamento loteActivo = new LoteMedicamento("L-203", "Amoxicilina 500 mg", TipoMedicamento.Antibioticos, hoy.AddMonths(6), 2500m, 50);
            LoteMedicamento loteVencido = new LoteMedicamento("L-118", "Meloxicam 1 mg", TipoMedicamento.Analgesicos, hoy.AddDays(-1), 1800m, 30);
            Console.WriteLine($"  {loteActivo.ObtenerDescripcion()} | vencido: {loteActivo.EstaVencido()}");
            Console.WriteLine($"  {loteVencido.ObtenerDescripcion()} | vencido: {loteVencido.EstaVencido()}");

            Console.WriteLine("\n    Recorrido polimorfico: la misma linea responde distinto segun el objeto");
            List<IFacturable> facturables = new List<IFacturable> { consulta, hospitalizacion, loteActivo };
            foreach (IFacturable item in facturables)
            {
                Console.WriteLine($"  - {item.ObtenerDescripcion()} | precio unitario: {item.ObtenerPrecioUnitario():C0}");
            }

            Console.WriteLine("\n[4] Factura con la consulta, la hospitalizacion y 2 unidades del lote vigente");
            Factura factura = new Factura("FAC-1001", DateTime.Today, "Ivan");
            factura.Agregar(consulta);
            factura.Agregar(hospitalizacion);
            factura.Agregar(loteActivo, 2);
            Console.WriteLine("  Renglones agregados: 3");

            Console.WriteLine("\n[5] Factura impresa");
            ImprimirFactura(factura);

            Console.WriteLine("\n[6] Intento de facturar el lote vencido");
            try
            {
                factura.Agregar(loteVencido, 1);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"  El sistema lo impidio: {ex.Message}");
            }
            Console.WriteLine($"  La factura sigue igual, total: {factura.Total:C0}");

            Console.WriteLine("\n[7] Anular la factura e intentar agregar otro renglon");
            bool anulada = factura.Anular();
            Console.WriteLine($"  Factura anulada: {anulada}");
            try
            {
                factura.Agregar(loteActivo, 1);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"  El sistema lo impidio: {ex.Message}");
            }

            Console.WriteLine("\n[8] Factura nueva con la misma consulta");
            Factura nuevaFactura = new Factura("FAC-1002", DateTime.Today, "Ivan");
            nuevaFactura.Agregar(consulta);
            ImprimirFactura(nuevaFactura);
        }

        static void ImprimirFactura(Factura factura)
        {
            Console.WriteLine($"  Factura {factura.Numero} | {factura.FechaEmision:dd/MM/yyyy} | Cliente: {factura.NombreCliente}");
            foreach (RenglonFactura renglon in factura.Renglones)
            {
                Console.WriteLine($"  - {renglon.Descripcion} | Cantidad: {renglon.Cantidad} | Subtotal: {renglon.Subtotal:C0}");
            }
            Console.WriteLine($"  Total: {factura.Total:C0}");
        }
    }
}
