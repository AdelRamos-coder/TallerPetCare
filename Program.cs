using System.Globalization;
using System.Text;

namespace PetCare
{
    internal static class Program
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            CultureInfo.CurrentCulture = new CultureInfo("es-CO");
           
            MostrarBienvenida();
            MenuPrincipal();
            MostrarDespedida();
        }

        static Medicamento[] CrearMedicamentos()
        {
            return new[]
            {
                new Medicamento("MED-0142", TipoMedicamento.Antibioticos, "Amoxivet 250", "Tableta 250 mg",
                    1800m, 8, new DateTime(2027, 5, 30), false),
                new Medicamento("MED-0207", TipoMedicamento.Vacunas, "Rabivac", "Frasco 10 mL",
                    32000m, 25, new DateTime(2027, 11, 15), true),
                new Medicamento("MED-0315", TipoMedicamento.Antiparasitarios, "Drontal Plus", "Tableta 700 mg",
                    9500m, 40, new DateTime(2026, 3, 10), false)
            };
        }

        static CitaVeterinaria[] CrearCitas()
        {
            return new[]
            {
                new CitaVeterinaria("CIT-001", "Manchas", DateTime.Today.AddDays(5).AddHours(9.5)),
                new CitaVeterinaria("CIT-002", "Rocky", DateTime.Today.AddDays(1).AddHours(14), 45),
                new CitaVeterinaria("CIT-003", "Luna", DateTime.Today.AddDays(-3).AddHours(8), 90)
            };
        }

        static Jaula[] CrearJaulas()
        {
            return new[]
            {
                new Jaula("J-07", TamanoJaula.Pequeno, 25000m),
                new Jaula("J-08", TamanoJaula.Mediano, 38000m),
                new Jaula("J-12", TamanoJaula.Grande, 52000m)
            };
        }

        static void MostrarBienvenida()
        {
            Console.WriteLine("=========================================");
            Console.WriteLine("      CLINICA VETERINARIA PETCARE");
            Console.WriteLine("=========================================");
        }

        static void MostrarDespedida()
        {
            Console.WriteLine("Sistema cerrado");
        }

        static void MenuPrincipal()
        {
            string opcion = string.Empty;

            while (opcion != "0")
            {
                Console.WriteLine("\n============ MENU PRINCIPAL ============");
                Console.WriteLine("1. Farmacia");
                Console.WriteLine("2. Recepcion");
                Console.WriteLine("3. Hospitalizacion");
                Console.WriteLine("4. Casos que no pueden pasar");
                Console.WriteLine("5. Resumen de las tres areas");
                Console.WriteLine("6. Pruebas de LoteMedicamento");
                Console.WriteLine("7. Demostracion PetCare");
                Console.WriteLine("0. Salir");
                Console.Write("Opcion: ");

                opcion = Console.ReadLine() ?? "0";
                Console.WriteLine();

                switch (opcion)
                {
                    case "1": AreaFarmacia(); break;
                    case "2": AreaRecepcion(); break;
                    case "3": AreaHospitalizacion(); break;
                    case "4": CasosProhibidos(); break;
                    case "5": Resumen(); break;
                    case "6": PruebasLoteMedicamento.EjecutarTodas(); break;
                    case "7": DemostracionPetCare(); break;
                    case "0": break;
                    default: Console.WriteLine("Opcion no valida"); break;
                }
            }
        }

        static void AreaFarmacia()
        {
            Console.WriteLine("---------- FARMACIA ----------");
            Medicamento[] medicamentos = CrearMedicamentos();

            foreach (Medicamento medicamento in medicamentos)
            {
                MostrarFicha(medicamento);
            }

            medicamentos[0].RecibirUnidades(20);
            Console.WriteLine($"\nMED-0142 recibe 20 unidades | Stock: {medicamentos[0].StockActual}");
            Console.WriteLine($"MED-0207 despacha 5 unidades: {medicamentos[1].DespacharUnidades(5)} | Stock: {medicamentos[1].StockActual}");
            Console.WriteLine($"MED-0315 vencido: {medicamentos[2].EstaVencido()}");
            Console.WriteLine($"MED-0142 necesita pedido: {medicamentos[0].NecesitaReabastecimiento()}");
            Console.WriteLine($"Valor inventario MED-0207: {medicamentos[1].CalcularValorInventario():C0}");
        }

        static void MostrarFicha(Medicamento medicamento)
        {
            Console.WriteLine($"{medicamento.CodigoInterno} | {medicamento.TipoMedicamento} | {medicamento.NombreComercial} | " +
                $"{medicamento.Presentacion} | {medicamento.PrecioUnidad:C0} | Stock: {medicamento.StockActual} | " +
                $"Vence: {medicamento.FechaVencimiento:dd/MM/yyyy}" + (medicamento.EnNevera ? " | EN NEVERA" : ""));
        }

        static void AreaRecepcion()
        {
            Console.WriteLine("---------- RECEPCION ----------");
            CitaVeterinaria[] citas = CrearCitas();

            Console.WriteLine($"Confirmar veterinario cita 1: {citas[0].ConfirmarVeterinario("Dra. Lina Restrepo")}");
            Console.WriteLine($"Confirmar de nuevo cita 1: {citas[0].ConfirmarVeterinario("Dr. Andres Zapata")}");

            foreach (CitaVeterinaria cita in citas)
            {
                Console.WriteLine($"{cita.Codigo} | {cita.ObtenerDescripcion()} | {cita.DuracionMinutos} min | {cita.CalcularCosto():C0}");
            }
        }

        static void AreaHospitalizacion()
        {
            Console.WriteLine("---------- HOSPITALIZACION ----------");
            Jaula[] jaulas = CrearJaulas();

            jaulas[0].IngresarPaciente("Manchas");
            jaulas[1].IngresarPaciente("Rocky");

            jaulas[0].SumarDiaEstancia();
            jaulas[0].SumarDiaEstancia();
            jaulas[0].SumarDiaEstancia();
            jaulas[1].SumarDiaEstancia();
            jaulas[2].SumarDiaEstancia();

            Console.WriteLine($"Cuenta de J-07: {jaulas[0].CalcularCuenta():C0}");
            Console.WriteLine(jaulas[1].ConsultarEstado());
            Console.WriteLine(jaulas[2].ConsultarEstado());

            Console.WriteLine($"Alta de Manchas | Total a cobrar: {jaulas[0].DarDeAltaMedica():C0}");
            Console.WriteLine(jaulas[0].ConsultarEstado());
        }

        static void CasosProhibidos()
        {
            Console.WriteLine("---------- CASOS QUE NO PUEDEN PASAR ----------");
            Medicamento[] medicamentos = CrearMedicamentos();
            CitaVeterinaria[] citas = CrearCitas();
            Jaula[] jaulas = CrearJaulas();

            Console.WriteLine($"\n[1] Despachar 200 de MED-0142 (hay {medicamentos[0].StockActual})");
            Console.WriteLine($"    Resultado: {medicamentos[0].DespacharUnidades(200)} | Stock intacto: {medicamentos[0].StockActual}");

            Console.WriteLine($"\n[2] Despachar 1 unidad de MED-0315, vencido el {medicamentos[2].FechaVencimiento:dd/MM/yyyy}");
            Console.WriteLine($"    Resultado: {medicamentos[2].DespacharUnidades(1)} | Stock intacto: {medicamentos[2].StockActual}");

            citas[1].ConfirmarVeterinario("Dr. Andres Zapata");
            Console.WriteLine($"\n[3] Reasignar veterinario en la cita 2 que ya tiene {citas[1].Veterinario}");
            Console.WriteLine($"    Resultado: {citas[1].ConfirmarVeterinario("Dra. Lina Restrepo")} | Sigue {citas[1].Veterinario}");

            Console.WriteLine("\n[4] Crear una cita de 10 minutos");
            try
            {
                new CitaVeterinaria("CIT-004", "Nube", DateTime.Today, 10);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"    Error: {ex.Message}");
            }

            jaulas[1].IngresarPaciente("Rocky");
            Console.WriteLine("\n[5] Meter a Nube en J-08, ocupada por Rocky");
            try
            {
                jaulas[1].IngresarPaciente("Nube");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"    Error: {ex.Message}");
            }

            Console.WriteLine("\n[6] Recibir una cantidad negativa en MED-0315");
            try
            {
                medicamentos[2].RecibirUnidades(-10);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"    Error: {ex.Message}");
            }

            Console.WriteLine("\n[7] Dar de alta antes del ingreso");
            try
            {
                new Hospitalizacion("HOS-001", "Simba", DateTime.Today, 50000m).DarDeAlta(DateTime.Today.AddDays(-1));
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"    Error: {ex.Message}");
            }

            Console.WriteLine("\n[8] Crear una jaula con tarifa en cero");
            try
            {
                new Jaula("J-99", TamanoJaula.Grande, 0m);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"    Error: {ex.Message}");
            }
        }

        static void Resumen()
        {
            Console.WriteLine("---------- RESUMEN DE LAS TRES AREAS ----------");
            Medicamento[] medicamentos = CrearMedicamentos();
            CitaVeterinaria[] citas = CrearCitas();
            Jaula[] jaulas = CrearJaulas();

            decimal valorTotal = 0m;
            foreach (Medicamento medicamento in medicamentos)
            {
                valorTotal += medicamento.CalcularValorInventario();
            }

            Console.WriteLine($"FARMACIA | valor del inventario: {valorTotal:C0}");
            foreach (Medicamento medicamento in medicamentos)
            {
                Console.WriteLine($"  {medicamento.CodigoInterno}: {medicamento.StockActual} unidades | " +
                    $"pedir: {medicamento.NecesitaReabastecimiento()} | vencido: {medicamento.EstaVencido()}");
            }

            Console.WriteLine("RECEPCION");
            foreach (CitaVeterinaria cita in citas)
            {
                Console.WriteLine($"  {cita.Codigo}: {cita.ObtenerDescripcion()} | costo {cita.CalcularCosto():C0}");
            }

            Console.WriteLine("HOSPITALIZACION");
            foreach (Jaula jaula in jaulas)
            {
                Console.WriteLine("  " + jaula.ConsultarEstado());
            }
        }
        static void DemostracionPetCare()
        {
            Console.WriteLine("*****Demostracion*****");
            //Cita 45min
            Console.WriteLine("**Consulta de 45 minutos**");
            CitaVeterinaria consulta = new CitaVeterinaria("CIT-009", "Rambo", DateTime.Today.AddHours(10), 45);
            bool veterinarioConfirmado = consulta.ConfirmarVeterinario("Dr. Mengueche");
            Console.WriteLine($"Veterinario confirmado: {veterinarioConfirmado}");
            Console.WriteLine(consulta.ObtenerDescripcion());
            Console.WriteLine($"Costo de la consulta: {consulta.CalcularCosto():C}");
            Console.WriteLine();
            //Hospitalizacion hace 3 dias
            Console.WriteLine("**Hospitalizacion de 3 dias **");
            Hospitalizacion hospitalizacion = new Hospitalizacion("HOS-132", "Trico", DateTime.Today.AddDays(-3), 80000m);
            bool altaDada = hospitalizacion.DarDeAlta(DateTime.Today);
            Console.WriteLine($"Alta registrada: :{altaDada}");
            Console.WriteLine(hospitalizacion.ObtenerDescripcion());
            Console.WriteLine($"Costo de la hospitalizacion: {hospitalizacion.CalcularCosto():C}");
            Console.WriteLine();

            //Lotes vencidos y no vencidos
            Console.WriteLine("**Lotes de medicamento**");
            DateOnly hoy = new DateOnly(DateTime.Today.Year,DateTime.Today.Month, DateTime.Today.Day);

            LoteMedicamento loteActivo = new LoteMedicamento("L-203", "Amoxicilina 500 mg", hoy.AddMonths(6), 2500m);
            LoteMedicamento loteVencido = new LoteMedicamento("L-118", "Meloxicam 1 mg", hoy.AddDays(-1), 1800m);
            Console.WriteLine($"{loteActivo.ObtenerDescripcion()} | vencido: {loteActivo.EstaVencido()}");
            Console.WriteLine($"{loteVencido.ObtenerDescripcion()} | vencido: {loteVencido.EstaVencido()}");
            Console.WriteLine();

           
            Console.WriteLine("**Factura alargada**");
            // Crear factura y agregar consulta, hospitalizacion y 2 unidades del lote vigente
            Factura factura = new Factura("FAC-1001", DateTime.Today, "Ivan");
            factura.Agregar(consulta);
            factura.Agregar(hospitalizacion);
            // Agregar lote vigente sólo si no está vencido
            if (loteActivo.EstaVencido())
            {
                Console.WriteLine($"No se agregó {loteActivo.ObtenerDescripcion()}: el lote está vencido.");
            }
            else
            {
                factura.Agregar(loteActivo, 2);
            }
            Console.WriteLine();

            // Imprimir renglones y total
            Console.WriteLine("**Imprimir renglones y total**");
            foreach (var renglon in factura.Renglones)
            {
                Console.WriteLine($"- {renglon.Descripcion} | Cantidad: {renglon.Cantidad} | Subtotal: {renglon.Subtotal:C}");
            }
            Console.WriteLine($"Total factura: {factura.Total:C}");
            Console.WriteLine();

            // Intento de facturar lote vencido 
            Console.WriteLine("**Intento de facturar lote vencido**");
            Factura facturaIntento = new Factura("FAC-1001-V", DateTime.Today, "Ivan");
            if (loteVencido.EstaVencido())
            {
                Console.WriteLine($"  No se pudo agregar el lote: {loteVencido.ObtenerDescripcion()} venció el {loteVencido.FechaVencimiento:dd/MM/yyyy}.");
            }
            Console.WriteLine();
            
            // Anular la factura y demostrar que ya no se pueden agregar renglones
            Console.WriteLine("**Anular factura**");
            bool anulada = factura.Anular();
            Console.WriteLine($"Factura anulada: {anulada}");
            Console.WriteLine("Intento de agregar otro renglón tras anulación:");
            if (factura.Anulada)
            {
                Console.WriteLine("  No se pudo agregar renglón: la factura está anulada.");
            }
            else
            {
                factura.Agregar(loteActivo, 1);
            }
            Console.WriteLine();

            //Factura nueva con la misma consulta y mostrar que se puede facturar de nuevo
            Console.WriteLine("**Factura con la misma consulta**");
            Factura nuevaFactura = new Factura("FAC-1002", DateTime.Today, "Ivan");
            nuevaFactura.Agregar(consulta);
            Console.WriteLine("  Consulta agregada a la nueva factura:");
            foreach (var renglon in nuevaFactura.Renglones)
            {
                Console.WriteLine($"  - {renglon.Descripcion} | Cantidad: {renglon.Cantidad} | Subtotal: {renglon.Subtotal:C}");
            }
            Console.WriteLine($"  Total nueva factura: {nuevaFactura.Total:C}");

        }
    }
}
