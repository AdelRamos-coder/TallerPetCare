using System.Globalization;

namespace PetCare
{
    internal static class Program
    {
        static void Main()
        {
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

        static Cita[] CrearCitas()
        {
            return new[]
            {
                new Cita(1, "Manchas", "1036458721", "Dra. Lina Restrepo",
                    DateTime.Today.AddDays(5).AddHours(9.5), "Vacunacion anual", 45000m),
                new Cita(2, "Rocky", "43125890", "Dr. Andres Zapata",
                    DateTime.Today.AddDays(1).AddHours(14), "Control posoperatorio", 60000m),
                new Cita(3, "Luna", "71234567", "Dra. Lina Restrepo",
                    DateTime.Today.AddDays(-3).AddHours(8), "Urgencia", 120000m)
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
            Cita[] citas = CrearCitas();

            Console.WriteLine(citas[0].ObtenerFicha());
            Console.WriteLine($"\nCita 1 {TextoDias(citas[0].CalcularDiasFaltantes())} | vigente: {citas[0].EstaVigente()}");
            Console.WriteLine($"Cita 3 {TextoDias(citas[2].CalcularDiasFaltantes())} | vigente: {citas[2].EstaVigente()}");
            Console.WriteLine($"Atender cita 2: {citas[1].MarcarAtendida()} | Estado: {citas[1].Estado}");
            Console.WriteLine($"Cancelar cita 3: {citas[2].MarcarCancelada()} | Estado: {citas[2].Estado}");
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
            Cita[] citas = CrearCitas();
            Jaula[] jaulas = CrearJaulas();

            Console.WriteLine($"\n[1] Despachar 200 de MED-0142 (hay {medicamentos[0].StockActual})");
            Console.WriteLine($"    Resultado: {medicamentos[0].DespacharUnidades(200)} | Stock intacto: {medicamentos[0].StockActual}");

            Console.WriteLine($"\n[2] Despachar 1 unidad de MED-0315, vencido el {medicamentos[2].FechaVencimiento:dd/MM/yyyy}");
            Console.WriteLine($"    Resultado: {medicamentos[2].DespacharUnidades(1)} | Stock intacto: {medicamentos[2].StockActual}");

            citas[1].MarcarAtendida();
            Console.WriteLine($"\n[3] Cancelar la cita 2 que esta {citas[1].Estado}");
            Console.WriteLine($"    Resultado: {citas[1].MarcarCancelada()} | Sigue {citas[1].Estado}");

            citas[2].MarcarCancelada();
            Console.WriteLine($"\n[4] Cancelar la cita 3 que ya esta {citas[2].Estado}");
            Console.WriteLine($"    Resultado: {citas[2].MarcarCancelada()}");

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

            Console.WriteLine("\n[7] Crear una cita con tarifa negativa");
            try
            {
                new Cita(4, "Simba", "8001234", "Dr. Andres Zapata",
                    DateTime.Today.AddDays(2), "Consulta", -5000m);
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
            Cita[] citas = CrearCitas();
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
            foreach (Cita cita in citas)
            {
                Console.WriteLine($"  Cita {cita.Numero}: {cita.Estado} | vigente: {cita.EstaVigente()} | " +
                    $"{TextoDias(cita.CalcularDiasFaltantes())} | tarifa {cita.Tarifa:C0}");
            }

            Console.WriteLine("HOSPITALIZACION");
            foreach (Jaula jaula in jaulas)
            {
                Console.WriteLine("  " + jaula.ConsultarEstado());
            }
        }

        static string TextoDias(int dias)
        {
            if (dias > 0)
            {
                return $"faltan {dias} dias";
            }
            if (dias == 0)
            {
                return "es hoy";
            }
            return $"ya pasaron {Math.Abs(dias)} dias";
        }
    }
}
