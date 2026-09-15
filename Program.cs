using System;

namespace TallerPetcar
{
    internal static class Program
    {
        static Medicamento medicamento1 = new Medicamento("MED-0142", TipoMedicamento.Antibioticos,
                "Amoxivet 250", "Tableta 250 mg", 1800m, 8, new DateTime(2027, 5, 30), false);
        static Medicamento medicamento2 = new Medicamento("MED-0207", TipoMedicamento.Vacunas,
            "Rabivac", "Frasco 10 mL", 32000m  , 25, new DateTime(2026, 11, 15), true);
        static Medicamento medicamento3 = new Medicamento("MED-0315", TipoMedicamento.Antiparasitarios,
            "Drontal Plus", "Tableta 700 mg", 9500m, 40, new DateTime(2026, 3, 10), false);

        static Cita cita1 = new Cita(1, "Manchas", "1036458721", "Dra. Lina Restrepo",
            new DateTime(2026, 9, 20, 9, 30, 0), "Vacunacion anual", 45000m);
        static Cita cita2 = new Cita(2, "Rocky", "43125890", "Dr. Andres Zapata",
            new DateTime(2026, 9, 15, 14, 0, 0), "Control posoperatorio", 60000m);
        static Cita cita3 = new Cita(3, "Luna", "71234567", "Dra. Lina Restrepo",
            new DateTime(2026, 9, 10, 8, 0, 0), "Urgencia", 120000m);

        static Jaula jaula1 = new Jaula("J-07", TamanoJaula.Pequeno, 25000m);
        static Jaula jaula2 = new Jaula("J-08", TamanoJaula.Mediano, 38000m);
        static Jaula jaula3 = new Jaula("J-12", TamanoJaula.Grande, 52000m); 

        static void Main()
        {
            MostrarBienvenida();
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
                Console.WriteLine("4. Casos que no pueden pasar");
                Console.WriteLine("5. Resumen de las tres areas");
                Console.WriteLine("0. Salir");
                Console.Write("Opcion: ");

                opcion = Console.ReadLine() ?? string.Empty;
                Console.WriteLine();

                switch (opcion)
                {
                    case "1": Farmacia(); break;
                    case "2": Recepcion(); break;
                    case "3": Hospitalizacion(); break;
                    case "4": CasosProhibidos(); break;
                    case "5": Resumen(); break;
                    case "0": break;
                    default: Console.WriteLine("Opcion no valida"); break;
                }
            }
        }

        static void Farmacia()
        {
            Console.WriteLine("---------- FARMACIA ----------");
            MostrarFicha(medicamento1);
            MostrarFicha(medicamento2);
            MostrarFicha(medicamento3);

            medicamento1.RecibirUnidades(20);
            Console.WriteLine($"\nMED-0142 recibe 20 unidades | Stock: {medicamento1.StockActual}");
            Console.WriteLine($"MED-0207 despacha 5 unidades: {medicamento2.DespacharUnidades(5)} | Stock: {medicamento2.StockActual}");
            Console.WriteLine($"MED-0315 vencido: {medicamento3.EstaVencido()}");
            Console.WriteLine($"MED-0142 necesita pedido: {medicamento1.NecesitaReabastecimiento()}");
            Console.WriteLine($"Valor inventario MED-0207: {medicamento2.CalcularValorInventario():C}");
        }

        static void MostrarFicha(Medicamento medicamento)
        {
            Console.WriteLine($"{medicamento.CodigoInterno} | {medicamento.TipoMedicamento} | {medicamento.NombreComercial} | " +
                $"{medicamento.Presentacion} | {medicamento.PrecioUnidad:C} | Stock: {medicamento.StockActual} | " +
                $"Vence: {medicamento.FechaVencimiento:dd/MM/yyyy}" + (medicamento.EnNevera ? " | EN NEVERA" : ""));
        }

        static void Recepcion()
        {
            Console.WriteLine("---------- RECEPCION ----------");
            cita1.MostrarFicha();

            Console.WriteLine($"\nCita 1 {TextoDias(cita1.CalcularDiasFaltantes())} | vigente: {cita1.EstaVigente()}");
            Console.WriteLine($"Cita 3 {TextoDias(cita3.CalcularDiasFaltantes())} | vigente: {cita3.EstaVigente()}");
            Console.WriteLine($"Atender cita 2: {cita2.MarcarAtendida()} | Estado: {cita2.Estado}");
            Console.WriteLine($"Cancelar cita 3: {cita3.MarcarCancelada()} | Estado: {cita3.Estado}");
        }

        static void Hospitalizacion()
        {
            Console.WriteLine("---------- HOSPITALIZACION ----------");
            jaula1.IngresarPaciente("Manchas");
            jaula2.IngresarPaciente("Rocky");

            jaula1.SumarDiaEstancia();
            jaula1.SumarDiaEstancia();
            jaula1.SumarDiaEstancia();
            jaula2.SumarDiaEstancia();
            jaula3.SumarDiaEstancia();

            Console.WriteLine($"Cuenta de J-07: {jaula1.CalcularCuenta():C}");
            Console.WriteLine(jaula2.ConsultarEstado());
            Console.WriteLine(jaula3.ConsultarEstado());

            Console.WriteLine($"Alta de Manchas | Total a cobrar: {jaula1.DarDeAltaMedica():C}");
            Console.WriteLine(jaula1.ConsultarEstado());
        }

        

        static void CasosProhibidos()
        {
            Console.WriteLine("---------- CASOS QUE NO PUEDEN PASAR ----------");

            Console.WriteLine($"\n[1] Despachar 200 de MED-0142 (hay {medicamento1.StockActual})");
            Console.WriteLine($"    Resultado: {medicamento1.DespacharUnidades(200)} | Stock intacto: {medicamento1.StockActual}");

            cita2.MarcarAtendida();
            Console.WriteLine($"\n[2] Cancelar la cita 2 que esta {cita2.Estado}");
            Console.WriteLine($"    Resultado: {cita2.MarcarCancelada()} | Sigue {cita2.Estado}");

            cita3.MarcarCancelada();
            Console.WriteLine($"\n[3] Cancelar la cita 3 que ya esta {cita3.Estado}");
            Console.WriteLine($"    Resultado: {cita3.MarcarCancelada()}");

            jaula2.IngresarPaciente("Rocky");
            Console.WriteLine("\n[4] Meter a Nube en J-08, ocupada por Rocky");
            jaula2.IngresarPaciente("Nube");

            Console.WriteLine("\n[5] Recibir una cantidad negativa en MED-0315");
            medicamento3.RecibirUnidades(-10);

            Console.WriteLine("\n[6] Crear una cita con tarifa negativa");
            Cita citaInvalida = new Cita(4, "Simba", "8001234", "Dr. Andres Zapata",
                    new DateTime(2026, 10, 1, 10, 0, 0), "Consulta", -5000m);

            Console.WriteLine("\n[7] Crear una jaula con tarifa en cero");
            Jaula jaulaInvalida = new Jaula("J-99", TamanoJaula.Grande, 0m);
        }

        static void Resumen()
        {
            Console.WriteLine("---------- RESUMEN DE LAS TRES AREAS ----------");

            decimal valorTotal = medicamento1.CalcularValorInventario()
                + medicamento2.CalcularValorInventario()
                + medicamento3.CalcularValorInventario();

            Console.WriteLine($"FARMACIA | valor del inventario: {valorTotal:C}");
            Console.WriteLine($"  MED-0142: {medicamento1.StockActual} unidades | pedir: {medicamento1.NecesitaReabastecimiento()} | vencido: {medicamento1.EstaVencido()}");
            Console.WriteLine($"  MED-0207: {medicamento2.StockActual} unidades | pedir: {medicamento2.NecesitaReabastecimiento()} | vencido: {medicamento2.EstaVencido()}");
            Console.WriteLine($"  MED-0315: {medicamento3.StockActual} unidades | pedir: {medicamento3.NecesitaReabastecimiento()} | vencido: {medicamento3.EstaVencido()}");

            Console.WriteLine("RECEPCION");
            Console.WriteLine($"  Cita 1: {cita1.Estado} | vigente: {cita1.EstaVigente()} | {TextoDias(cita1.CalcularDiasFaltantes())}");
            Console.WriteLine($"  Cita 2: {cita2.Estado} | tarifa {cita2.Tarifa:C}");
            Console.WriteLine($"  Cita 3: {cita3.Estado} | vigente: {cita3.EstaVigente()}");

            Console.WriteLine("HOSPITALIZACION");
            Console.WriteLine("  " + jaula1.ConsultarEstado());
            Console.WriteLine("  " + jaula2.ConsultarEstado());
            Console.WriteLine("  " + jaula3.ConsultarEstado());
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