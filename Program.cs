using System;

namespace PetCar
{
    class Program
    {
        static Medicamento medicamento1 = new Medicamento("MED-0142", TipoMedicamento.Antibioticos,
            "Amoxivet 250", "Tableta 250 mg", 1800m, 8, new DateTime(2027, 5, 30), false);
        static Medicamento medicamento2 = new Medicamento("MED-0207", TipoMedicamento.Vacunas,
            "Rabivac", "Frasco 10 mL", 32000m, 25, new DateTime(2026, 11, 15), true);
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
            string opcion = string.Empty;

            while (opcion != "0")
            {
                Console.WriteLine("\n========== CLINICA PETCARE ==========");
                Console.WriteLine("1. Farmacia");
                Console.WriteLine("2. Recepcion");
                Console.WriteLine("3. Hospitalizacion");
                Console.WriteLine("4. Casos que no pueden pasar");
                Console.WriteLine("5. Resumen final");
                Console.WriteLine("0. Salir");
                Console.Write("Opcion: ");

                opcion = Console.ReadLine() ?? string.Empty;
                Console.WriteLine();

                switch (opcion)
                {
                    case "1":
                        Farmacia();
                        break;
                    case "2":
                        Recepcion();
                        break;
                    case "3":
                        Hospitalizacion();
                        break;
                    case "4":
                        CasosProhibidos();
                        break;
                    case "5":
                        ResumenFinal();
                        break;
                    case "0":
                        Console.WriteLine("Hasta luego.");
                        break;
                    default:
                        Console.WriteLine("Opcion no valida.");
                        break;
                }
            }
        }

        static void Farmacia()
        {
            Console.WriteLine("---------- FARMACIA ----------");
            medicamento1.RecibirUnidades(20);
            Console.WriteLine($"MED-0142 recibe 20 unidades. Stock: {medicamento1.StockActual}");

            bool despacho = medicamento2.DespacharUnidades(5);
            Console.WriteLine($"MED-0207 despacha 5 unidades: {despacho}. Stock: {medicamento2.StockActual}");

            Console.WriteLine($"MED-0142 vencido: {medicamento1.EstaVencido()}");
            Console.WriteLine($"MED-0315 vencido: {medicamento3.EstaVencido()}");

            Console.WriteLine($"MED-0142 necesita reabastecimiento: {medicamento1.NecesitaReabastecimiento()}");
            Console.WriteLine($"MED-0207 necesita reabastecimiento: {medicamento2.NecesitaReabastecimiento()}");

            Console.WriteLine($"Valor inventario MED-0315: {medicamento3.CalcularValorInventario():C}");
            Console.WriteLine($"MED-0207 se conserva en nevera: {medicamento2.EnNevera}");
        }

        static void Recepcion()
        {
            Console.WriteLine("---------- RECEPCION ----------");

            cita1.MostrarFicha();
            Console.WriteLine($"Dias faltantes: {cita1.CalcularDiasFaltantes()}");
            Console.WriteLine($"Vigente: {cita1.EstaVigente()}");

            Console.WriteLine($"\nCita 3 dias faltantes: {cita3.CalcularDiasFaltantes()}");
            Console.WriteLine($"Cita 3 vigente: {cita3.EstaVigente()}");

            Console.WriteLine($"\nAtender cita 2: {cita2.MarcarAtendida()}. Estado: {cita2.Estado}");
            Console.WriteLine($"Cancelar cita 3: {cita3.MarcarCancelada()}. Estado: {cita3.Estado}");
        }

        static void Hospitalizacion()
        {
            Console.WriteLine("---------- HOSPITALIZACION ----------");
            if (!jaula1.Ocupado)
            {
                jaula1.IngresarPaciente("Manchas");
            }
            if (!jaula2.Ocupado)
            {
                jaula2.IngresarPaciente("Rocky");
            }
            Console.WriteLine("Manchas en J-07 y Rocky en J-08.");

            jaula1.SumarDiaEstancia();
            jaula1.SumarDiaEstancia();
            jaula1.SumarDiaEstancia();
            jaula2.SumarDiaEstancia();
            jaula3.SumarDiaEstancia();
            Console.WriteLine("Ronda de la manana hecha.");

            Console.WriteLine($"Cuenta de J-07: {jaula1.CalcularCuenta():C}");
            Console.WriteLine(jaula2.ConsultarEstado());
            Console.WriteLine(jaula3.ConsultarEstado());

            if (jaula1.Ocupado)
            {
                decimal totalCobrar = jaula1.DarDeAltaMedica();
                Console.WriteLine($"Alta de Manchas. Total a cobrar: {totalCobrar:C}");
                Console.WriteLine(jaula1.ConsultarEstado());
            }
        }


        static void CasosProhibidos()
        {
            Console.WriteLine("---------- CASOS QUE NO PUEDEN PASAR ----------");

            // Marcela: "despacho veinte unidades cuando solo habia cinco".
            Console.WriteLine("\n[1] Despachar 200 unidades de MED-0142:");
            Console.WriteLine($"    Resultado: {medicamento1.DespacharUnidades(200)}");
            Console.WriteLine($"    Stock intacto: {medicamento1.StockActual}");

            // Diana: "una cita ya atendida no se puede cancelar, punto".
            cita2.MarcarAtendida();
            Console.WriteLine("\n[2] Cancelar la cita 2, que ya fue atendida:");
            Console.WriteLine($"    Resultado: {cita2.MarcarCancelada()}. Estado: {cita2.Estado}");

            // Diana: "cancelar dos veces la misma cita tampoco tiene sentido".
            cita3.MarcarCancelada();
            Console.WriteLine("\n[3] Cancelar por segunda vez la cita 3:");
            Console.WriteLine($"    Resultado: {cita3.MarcarCancelada()}. Estado: {cita3.Estado}");

            // Don Hernan: "metimos dos animales a la misma jaula".
            if (!jaula2.Ocupado)
            {
                jaula2.IngresarPaciente("Rocky");
            }
            Console.WriteLine("\n[4] Ingresar a Nube en J-08, ocupada por Rocky:");
            try
            {
                jaula2.IngresarPaciente("Nube");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"    Bloqueado por la clase: {ex.Message}");
            }

            // La clase tampoco deja nacer un objeto invalido.
            Console.WriteLine("\n[5] Crear una cita con tarifa negativa:");
            try
            {
                Cita citaInvalida = new Cita(4, "Simba", "8001234", "Dr. Andres Zapata",
                    new DateTime(2026, 10, 1, 10, 0, 0), "Consulta", -5000m);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"    Bloqueado por la clase: {ex.Message}");
            }

            Console.WriteLine("\n[6] Crear una jaula con tarifa cero:");
            try
            {
                Jaula jaulaInvalida = new Jaula("J-99", TamanoJaula.Grande, 0m);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"    Bloqueado por la clase: {ex.Message}");
            }
        }

        /// <summary>Muestra el estado actual de las tres areas.</summary>
        static void ResumenFinal()
        {
            Console.WriteLine("---------- RESUMEN FINAL ----------");

            decimal valorFarmacia = medicamento1.CalcularValorInventario()
                + medicamento2.CalcularValorInventario()
                + medicamento3.CalcularValorInventario();

            Console.WriteLine("FARMACIA: 3 referencias");
            Console.WriteLine($"  Valor total del inventario: {valorFarmacia:C}");
            Console.WriteLine($"  MED-0315 vencido: {medicamento3.EstaVencido()}");

            Console.WriteLine("RECEPCION: 3 citas");
            Console.WriteLine($"  Cita 1: {cita1.Estado} | Vigente: {cita1.EstaVigente()}");
            Console.WriteLine($"  Cita 2: {cita2.Estado}");
            Console.WriteLine($"  Cita 3: {cita3.Estado}");

            Console.WriteLine("HOSPITALIZACION: 3 jaulas");
            Console.WriteLine("  " + jaula1.ConsultarEstado());
            Console.WriteLine("  " + jaula2.ConsultarEstado());
            Console.WriteLine("  " + jaula3.ConsultarEstado());
        }
    }
}