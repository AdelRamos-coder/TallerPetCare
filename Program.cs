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
            DemostracionPetCare();

            Console.WriteLine("\nPresione Enter para ir al menu...");
            Console.ReadLine();

            MenuPrincipal();
            MostrarDespedida();
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
                Console.WriteLine("4. Facturacion");
                Console.WriteLine("5. Casos que no pueden pasar");
                Console.WriteLine("6. Resumen de las tres areas");
                Console.WriteLine("7. Repetir la demostracion");
                Console.WriteLine("0. Salir");
                Console.Write("Opcion: ");

                opcion = Console.ReadLine() ?? "0";
                Console.WriteLine();

                switch (opcion)
                {
                    case "1": AreaFarmacia(); break;
                    case "2": AreaRecepcion(); break;
                    case "3": AreaHospitalizacion(); break;
                    case "4": AreaFacturacion(); break;
                    case "5": CasosProhibidos(); break;
                    case "6": Resumen(); break;
                    case "7": DemostracionPetCare(); break;
                    case "0": break;
                    default: Console.WriteLine("Opcion no valida"); break;
                }
            }
        }

        static LoteMedicamento[] CrearLotes()
        {
            DateOnly hoy = DateOnly.FromDateTime(DateTime.Today);
            return new[]
            {
                new LoteMedicamento("MED-0142", "Amoxivet 250 mg", TipoMedicamento.Antibioticos, hoy.AddMonths(8), 1800m, 8),
                new LoteMedicamento("MED-0207", "Rabivac 10 mL", TipoMedicamento.Vacunas, hoy.AddMonths(12), 32000m, 25, true),
                new LoteMedicamento("MED-0315", "Drontal Plus 700 mg", TipoMedicamento.Antiparasitarios, hoy.AddDays(-20), 9500m, 40)
            };
        }

        static CitaVeterinaria[] CrearCitas()
        {
            return new[]
            {
                new CitaVeterinaria("CIT-001", "Manchas", "1036458721", DateTime.Today.AddDays(5).AddHours(9.5), "Vacunacion anual"),
                new CitaVeterinaria("CIT-002", "Rocky", "43125890", DateTime.Today.AddDays(1).AddHours(14), "Control posoperatorio", 45),
                new CitaVeterinaria("CIT-003", "Luna", "71234567", DateTime.Today.AddDays(-3).AddHours(8), "Urgencia", 90)
            };
        }

        static Jaula[] CrearJaulas()
        {
            return new[]
            {
                new Jaula("J-03", TamanoJaula.Pequeno),
                new Jaula("J-07", TamanoJaula.Mediano),
                new Jaula("J-12", TamanoJaula.Grande)
            };
        }

        static void AreaFarmacia()
        {
            Console.WriteLine("---------- FARMACIA ----------");
            LoteMedicamento[] lotes = CrearLotes();

            foreach (LoteMedicamento lote in lotes)
            {
                Console.WriteLine($"{lote.Codigo} | {lote.Tipo} | {lote.NombreMedicamento} | {lote.PrecioUnitario:C0} | " +
                    $"Stock: {lote.StockActual} | Vence: {lote.FechaVencimiento:dd/MM/yyyy}" + (lote.EnNevera ? " | EN NEVERA" : ""));
            }

            lotes[0].RecibirUnidades(20);
            Console.WriteLine($"\nMED-0142 recibe 20 unidades | Stock: {lotes[0].StockActual}");
            Console.WriteLine($"MED-0207 despacha 5 unidades: {lotes[1].DespacharUnidades(5)} | Stock: {lotes[1].StockActual}");
            Console.WriteLine($"MED-0315 vencido: {lotes[2].EstaVencido()}");
            Console.WriteLine($"MED-0142 necesita pedido: {lotes[0].NecesitaReabastecimiento()}");
            Console.WriteLine($"Valor inventario MED-0207: {lotes[1].CalcularValorInventario():C0}");
        }

        static void AreaRecepcion()
        {
            Console.WriteLine("---------- RECEPCION ----------");
            CitaVeterinaria[] citas = CrearCitas();

            Console.WriteLine($"Confirmar veterinario en CIT-001: {citas[0].ConfirmarVeterinario("Dra. Lina Restrepo")}");
            Console.WriteLine($"Atender CIT-002: {citas[1].MarcarAtendida()} | Estado: {citas[1].Estado}");
            Console.WriteLine($"Cancelar CIT-003: {citas[2].MarcarCancelada()} | Estado: {citas[2].Estado}");
            Console.WriteLine();

            foreach (CitaVeterinaria cita in citas)
            {
                Console.WriteLine($"{cita.Codigo} | Cliente: {cita.CedulaCliente} | {cita.Motivo} | {cita.DuracionMinutos} min | " +
                    $"{TextoDias(cita.CalcularDiasFaltantes())} | vigente: {cita.EstaVigente()} | {cita.CalcularCosto():C0}");
            }
        }

        static void AreaHospitalizacion()
        {
            Console.WriteLine("---------- HOSPITALIZACION ----------");
            Jaula[] jaulas = CrearJaulas();

            foreach (Jaula jaula in jaulas)
            {
                Console.WriteLine(jaula.ConsultarEstado());
            }

            Hospitalizacion manchas = new Hospitalizacion("HOS-001", "Manchas", DateTime.Today.AddDays(-2), jaulas[0]);
            Hospitalizacion rocky = new Hospitalizacion("HOS-002", "Rocky", DateTime.Today.AddDays(-5), jaulas[2]);

            Console.WriteLine("\nIngresan Manchas a J-03 y Rocky a J-12");
            Console.WriteLine(jaulas[0].ConsultarEstado());
            Console.WriteLine(jaulas[2].ConsultarEstado());

            Console.WriteLine($"\nAlta de Manchas: {manchas.DarDeAlta(DateTime.Today)} | Total a cobrar: {manchas.CalcularCosto():C0}");
            Console.WriteLine(jaulas[0].ConsultarEstado());
            Console.WriteLine($"Rocky sigue hospitalizado: {rocky.ObtenerDescripcion()} | Cuenta actual: {rocky.CalcularCosto():C0}");
        }

        static void AreaFacturacion()
        {
            Console.WriteLine("---------- FACTURACION ----------");
            CitaVeterinaria cita = CrearCitas()[1];
            cita.ConfirmarVeterinario("Dr. Andres Zapata");
            cita.MarcarAtendida();

            Jaula jaula = CrearJaulas()[1];
            Hospitalizacion hospitalizacion = new Hospitalizacion("HOS-003", "Rocky", DateTime.Today.AddDays(-2), jaula);
            hospitalizacion.DarDeAlta(DateTime.Today);

            LoteMedicamento lote = CrearLotes()[0];

            Factura factura = new Factura("FAC-2001", DateTime.Today, "Carlos Gomez");
            factura.Agregar(cita);
            factura.Agregar(hospitalizacion);
            factura.Agregar(lote, 3);
            ImprimirFactura(factura);

            Console.WriteLine($"\nAnular la factura: {factura.Anular()}");
            Console.WriteLine($"Anularla otra vez: {factura.Anular()}");
            Console.WriteLine($"Factura anulada: {factura.Anulada}");
        }

        static void CasosProhibidos()
        {
            Console.WriteLine("---------- CASOS QUE NO PUEDEN PASAR ----------");
            LoteMedicamento[] lotes = CrearLotes();
            CitaVeterinaria[] citas = CrearCitas();
            Jaula[] jaulas = CrearJaulas();

            Console.WriteLine($"\n[1] Despachar 200 de MED-0142 (hay {lotes[0].StockActual})");
            Console.WriteLine($"    Resultado: {lotes[0].DespacharUnidades(200)} | Stock intacto: {lotes[0].StockActual}");

            Console.WriteLine($"\n[2] Despachar 1 unidad de MED-0315, vencido el {lotes[2].FechaVencimiento:dd/MM/yyyy}");
            Console.WriteLine($"    Resultado: {lotes[2].DespacharUnidades(1)} | Stock intacto: {lotes[2].StockActual}");

            citas[0].ConfirmarVeterinario("Dra. Lina Restrepo");
            Console.WriteLine($"\n[3] Confirmar otro veterinario en CIT-001, que ya tiene a {citas[0].Veterinario}");
            Console.WriteLine($"    Resultado: {citas[0].ConfirmarVeterinario("Dr. Andres Zapata")} | Sigue {citas[0].Veterinario}");

            citas[1].MarcarAtendida();
            Console.WriteLine($"\n[4] Cancelar CIT-002, que ya esta {citas[1].Estado}");
            Console.WriteLine($"    Resultado: {citas[1].MarcarCancelada()} | Sigue {citas[1].Estado}");

            Console.WriteLine("\n[5] Crear una cita de 10 minutos");
            try
            {
                new CitaVeterinaria("CIT-004", "Nube", "8001234", DateTime.Today.AddDays(2), "Consulta", 10);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"    Error: {ex.Message}");
            }

            new Hospitalizacion("HOS-004", "Rocky", DateTime.Today, jaulas[1]);
            Console.WriteLine("\n[6] Meter a Nube en J-07, ocupada por Rocky");
            try
            {
                new Hospitalizacion("HOS-005", "Nube", DateTime.Today, jaulas[1]);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"    Error: {ex.Message}");
            }

            Console.WriteLine("\n[7] Dar de alta antes del ingreso");
            try
            {
                new Hospitalizacion("HOS-006", "Simba", DateTime.Today, jaulas[2]).DarDeAlta(DateTime.Today.AddDays(-1));
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"    Error: {ex.Message}");
            }

            Factura factura = new Factura("FAC-3001", DateTime.Today, "Diana Perez");
            citas[2].MarcarCancelada();
            Console.WriteLine("\n[8] Facturar CIT-003, que esta cancelada");
            try
            {
                factura.Agregar(citas[2]);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"    Error: {ex.Message}");
            }

            factura.Anular();
            Console.WriteLine("\n[9] Agregar un renglon a una factura anulada");
            try
            {
                factura.Agregar(lotes[1], 1);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"    Error: {ex.Message}");
            }
        }

        static void Resumen()
        {
            Console.WriteLine("---------- RESUMEN DE LAS TRES AREAS ----------");
            LoteMedicamento[] lotes = CrearLotes();
            CitaVeterinaria[] citas = CrearCitas();
            Jaula[] jaulas = CrearJaulas();

            decimal valorTotal = 0m;
            foreach (LoteMedicamento lote in lotes)
            {
                valorTotal += lote.CalcularValorInventario();
            }

            Console.WriteLine($"FARMACIA | valor del inventario: {valorTotal:C0}");
            foreach (LoteMedicamento lote in lotes)
            {
                Console.WriteLine($"  {lote.Codigo}: {lote.StockActual} unidades | pedir: {lote.NecesitaReabastecimiento()} | vencido: {lote.EstaVencido()}");
            }

            Console.WriteLine("RECEPCION");
            foreach (CitaVeterinaria cita in citas)
            {
                Console.WriteLine($"  {cita.Codigo}: {cita.Estado} | vigente: {cita.EstaVigente()} | {TextoDias(cita.CalcularDiasFaltantes())} | {cita.CalcularCosto():C0}");
            }

            Console.WriteLine("HOSPITALIZACION");
            foreach (Jaula jaula in jaulas)
            {
                Console.WriteLine("  " + jaula.ConsultarEstado());
            }
        }

        static string TextoDias(int dias)
        {
            if (dias == 1)
            {
                return "falta 1 dia";
            }
            if (dias > 1)
            {
                return $"faltan {dias} dias";
            }
            if (dias == 0)
            {
                return "es hoy";
            }
            return $"ya pasaron {Math.Abs(dias)} dias";
        }

        static void DemostracionPetCare()
        {
            Console.WriteLine("\n[1] Consulta de 45 minutos");
            CitaVeterinaria consulta = new CitaVeterinaria("CIT-009", "Rambo", "1036458721",
                DateTime.Today.AddHours(10), "Control posoperatorio", 45);
            Console.WriteLine($"  Estado: {consulta.Estado} | vigente: {consulta.EstaVigente()} | dias faltantes: {consulta.CalcularDiasFaltantes()}");
            bool veterinarioConfirmado = consulta.ConfirmarVeterinario("Dr. Mengueche");
            Console.WriteLine($"  Veterinario confirmado: {veterinarioConfirmado}");
            Console.WriteLine($"  Confirmar otro veterinario: {consulta.ConfirmarVeterinario("Dra. Lina Restrepo")}");
            Console.WriteLine($"  Marcar atendida: {consulta.MarcarAtendida()} | Estado: {consulta.Estado}");
            Console.WriteLine($"  Cancelar una cita ya atendida: {consulta.MarcarCancelada()} | Sigue {consulta.Estado}");
            Console.WriteLine($"  {consulta.ObtenerDescripcion()}");
            Console.WriteLine($"  Costo de la consulta: {consulta.CalcularCosto():C0}");

            Console.WriteLine("\n[2] Hospitalizacion que ingreso hace 3 dias, alta hoy");
            Jaula jaulaPequena = new Jaula("J-03", TamanoJaula.Pequeno);
            Jaula jaula = new Jaula("J-07", TamanoJaula.Mediano);
            Jaula jaulaGrande = new Jaula("J-12", TamanoJaula.Grande);
            Console.WriteLine($"  Tarifas por tamano: {jaulaPequena.Tamano} {jaulaPequena.TarifaDia:C0} | " +
                $"{jaula.Tamano} {jaula.TarifaDia:C0} | {jaulaGrande.Tamano} {jaulaGrande.TarifaDia:C0}");
            Hospitalizacion hospitalizacion = new Hospitalizacion("HOS-132", "Trico", DateTime.Today.AddDays(-3), jaula);
            Console.WriteLine($"  Al ingresar: {jaula.ConsultarEstado()}");
            try
            {
                new Hospitalizacion("HOS-133", "Nube", DateTime.Today, jaula);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"  Otro animal en la misma jaula: {ex.Message}");
            }
            bool altaDada = hospitalizacion.DarDeAlta(DateTime.Today);
            Console.WriteLine($"  Alta registrada: {altaDada}");
            Console.WriteLine($"  Despues del alta: {jaula.ConsultarEstado()}");
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

            Console.WriteLine("\n    Intento de facturar una cita cancelada");
            CitaVeterinaria citaCancelada = new CitaVeterinaria("CIT-010", "Luna", "71234567",
                DateTime.Today.AddDays(2), "Vacunacion anual");
            Console.WriteLine($"  Cancelar: {citaCancelada.MarcarCancelada()} | Cancelar otra vez: {citaCancelada.MarcarCancelada()}");
            try
            {
                factura.Agregar(citaCancelada);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"  El sistema lo impidio: {ex.Message}");
            }

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
