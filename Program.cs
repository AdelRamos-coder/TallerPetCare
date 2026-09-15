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
            MostrarBienvenida();
            MenuPrincipal();
            MostrarDespedida();
        }

        static void MostrarBienvenida()
        {
            Console.WriteLine("===============================================");
            Console.WriteLine("       CLINICA VETERINARIA PETCARE");
            Console.WriteLine("===============================================");
        }

        static void MostrarDespedida()
        {
            Console.WriteLine("Sistema cerrado");
        }

        // ---------- NIVEL 1: AREAS ----------

        static void MenuPrincipal()
        {
            string opcion = string.Empty;

            while (opcion != "0")
            {
                Console.WriteLine("\n=========== MENU PRINCIPAL ===========");
                Console.WriteLine("1. Farmacia");
                Console.WriteLine("2. Recepcion");
                Console.WriteLine("3. Hospitalizacion");
                Console.WriteLine("4. Resumen de las tres areas");
                Console.WriteLine("5. Demostracion automatica");
                Console.WriteLine("0. Salir");
                Console.Write("Opcion: ");

                opcion = Console.ReadLine() ?? string.Empty;
                Console.WriteLine();

                switch (opcion)
                {
                    case "1": ElegirMedicamento(); break;
                    case "2": ElegirCita(); break;
                    case "3": ElegirJaula(); break;
                    case "4": ResumenGeneral(); break;
                    case "5": DemostracionAutomatica(); break;
                    case "0": break;
                    default: Console.WriteLine("Opcion no valida"); break;
                }
            }
        }

        // ---------- NIVEL 2: ELEGIR OBJETO ----------

        static void ElegirMedicamento()
        {
            string opcion = string.Empty;

            while (opcion != "0")
            {
                Console.WriteLine("\n----- FARMACIA: elija la referencia -----");
                Console.WriteLine("1. MED-0142  Amoxivet 250");
                Console.WriteLine("2. MED-0207  Rabivac");
                Console.WriteLine("3. MED-0315  Drontal Plus");
                Console.WriteLine("0. Volver");
                Console.Write("Opcion: ");

                opcion = Console.ReadLine() ?? string.Empty;
                Console.WriteLine();

                switch (opcion)
                {
                    case "1": MenuMedicamento(medicamento1); break;
                    case "2": MenuMedicamento(medicamento2); break;
                    case "3": MenuMedicamento(medicamento3); break;
                    case "0": break;
                    default: Console.WriteLine("Opcion no valida"); break;
                }
            }
        }

        static void ElegirCita()
        {
            string opcion = string.Empty;

            while (opcion != "0")
            {
                Console.WriteLine("\n----- RECEPCION: elija la cita -----");
                Console.WriteLine("1. Cita 1  Manchas   20/09/2026");
                Console.WriteLine("2. Cita 2  Rocky     15/09/2026");
                Console.WriteLine("3. Cita 3  Luna      10/09/2026");
                Console.WriteLine("0. Volver");
                Console.Write("Opcion: ");

                opcion = Console.ReadLine() ?? string.Empty;
                Console.WriteLine();

                switch (opcion)
                {
                    case "1": MenuCita(cita1); break;
                    case "2": MenuCita(cita2); break;
                    case "3": MenuCita(cita3); break;
                    case "0": break;
                    default: Console.WriteLine("Opcion no valida"); break;
                }
            }
        }

        static void ElegirJaula()
        {
            string opcion = string.Empty;

            while (opcion != "0")
            {
                Console.WriteLine("\n----- HOSPITALIZACION: elija la jaula -----");
                Console.WriteLine("1. J-07  Pequena");
                Console.WriteLine("2. J-08  Mediana");
                Console.WriteLine("3. J-12  Grande");
                Console.WriteLine("0. Volver");
                Console.Write("Opcion: ");

                opcion = Console.ReadLine() ?? string.Empty;
                Console.WriteLine();

                switch (opcion)
                {
                    case "1": MenuJaula(jaula1); break;
                    case "2": MenuJaula(jaula2); break;
                    case "3": MenuJaula(jaula3); break;
                    case "0": break;
                    default: Console.WriteLine("Opcion no valida"); break;
                }
            }
        }

        // ---------- NIVEL 3: MEDICAMENTO ----------

        static void MenuMedicamento(Medicamento medicamento)
        {
            string opcion = string.Empty;

            while (opcion != "0")
            {
                Console.WriteLine($"\n----- {medicamento.CodigoInterno} - {medicamento.NombreComercial} -----");
                Console.WriteLine("1. Ver ficha");
                Console.WriteLine("2. Recibir unidades del proveedor");
                Console.WriteLine("3. Despachar unidades de una formula");
                Console.WriteLine("4. Saber si esta vencido");
                Console.WriteLine("5. Saber si necesita reabastecimiento");
                Console.WriteLine("6. Calcular valor del inventario");
                Console.WriteLine("0. Volver");
                Console.Write("Opcion: ");

                opcion = Console.ReadLine() ?? string.Empty;
                Console.WriteLine();

                switch (opcion)
                {
                    case "1": MostrarFichaMedicamento(medicamento); break;
                    case "2": RecibirUnidades(medicamento); break;
                    case "3": DespacharUnidades(medicamento); break;
                    case "4": MostrarVencimiento(medicamento); break;
                    case "5": MostrarReabastecimiento(medicamento); break;
                    case "6": MostrarValorInventario(medicamento); break;
                    case "0": break;
                    default: Console.WriteLine("Opcion no valida"); break;
                }
            }
        }

        static void MostrarFichaMedicamento(Medicamento medicamento)
        {
            Console.WriteLine($"Codigo: {medicamento.CodigoInterno}");
            Console.WriteLine($"Tipo: {medicamento.TipoMedicamento}");
            Console.WriteLine($"Nombre comercial: {medicamento.NombreComercial}");
            Console.WriteLine($"Presentacion: {medicamento.Presentacion}");
            Console.WriteLine($"Precio por unidad: {medicamento.PrecioUnidad:C}");
            Console.WriteLine($"Stock actual: {medicamento.StockActual}");
            Console.WriteLine($"Vence: {medicamento.FechaVencimiento:dd/MM/yyyy}");

            if (medicamento.EnNevera)
            {
                Console.WriteLine("*** CONSERVAR EN NEVERA ***");
            }
        }

        static void RecibirUnidades(Medicamento medicamento)
        {
            int cantidad = LeerEntero("Cuantas unidades llegaron: ");

            try
            {
                medicamento.RecibirUnidades(cantidad);
                Console.WriteLine($"Stock actualizado: {medicamento.StockActual}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Rechazado por la clase: {ex.Message}");
                Console.WriteLine($"Stock intacto: {medicamento.StockActual}");
            }
        }

        static void DespacharUnidades(Medicamento medicamento)
        {
            Console.WriteLine($"Stock disponible: {medicamento.StockActual}");
            int cantidad = LeerEntero("Cuantas unidades despacha: ");

            if (medicamento.DespacharUnidades(cantidad))
            {
                Console.WriteLine($"Despachadas | Stock: {medicamento.StockActual}");
            }
            else
            {
                Console.WriteLine("Rechazado por la clase: no hay suficientes unidades");
                Console.WriteLine($"Stock intacto: {medicamento.StockActual}");
            }
        }

        static void MostrarVencimiento(Medicamento medicamento)
        {
            Console.WriteLine($"Vence el {medicamento.FechaVencimiento:dd/MM/yyyy}");
            Console.WriteLine($"Esta vencido: {medicamento.EstaVencido()}");
        }

        static void MostrarReabastecimiento(Medicamento medicamento)
        {
            Console.WriteLine($"Stock actual: {medicamento.StockActual}");
            Console.WriteLine($"Necesita reabastecimiento: {medicamento.NecesitaReabastecimiento()}");
        }

        static void MostrarValorInventario(Medicamento medicamento)
        {
            Console.WriteLine($"{medicamento.StockActual} unidades x {medicamento.PrecioUnidad:C}");
            Console.WriteLine($"Valor del inventario: {medicamento.CalcularValorInventario():C}");
        }

        // ---------- NIVEL 3: CITA ----------

        static void MenuCita(Cita cita)
        {
            string opcion = string.Empty;

            while (opcion != "0")
            {
                Console.WriteLine($"\n----- Cita {cita.Numero} - {cita.NombreMascota} - {cita.Estado} -----");
                Console.WriteLine("1. Ver ficha");
                Console.WriteLine("2. Cuantos dias faltan");
                Console.WriteLine("3. Marcar como atendida");
                Console.WriteLine("4. Cancelar");
                Console.WriteLine("5. Saber si sigue vigente");
                Console.WriteLine("0. Volver");
                Console.Write("Opcion: ");

                opcion = Console.ReadLine() ?? string.Empty;
                Console.WriteLine();

                switch (opcion)
                {
                    case "1": cita.MostrarFicha(); break;
                    case "2": MostrarDiasFaltantes(cita); break;
                    case "3": MarcarAtendida(cita); break;
                    case "4": MarcarCancelada(cita); break;
                    case "5": MostrarVigencia(cita); break;
                    case "0": break;
                    default: Console.WriteLine("Opcion no valida"); break;
                }
            }
        }

        static void MostrarDiasFaltantes(Cita cita)
        {
            Console.WriteLine($"Faltan {cita.CalcularDiasFaltantes()} dias");
            Console.WriteLine("Un numero negativo significa que la cita ya paso");
        }

        static void MarcarAtendida(Cita cita)
        {
            if (cita.MarcarAtendida())
            {
                Console.WriteLine($"Cita marcada como atendida - Estado: {cita.Estado}");
            }
            else
            {
                Console.WriteLine($"Rechazado por la clase: la cita esta {cita.Estado}");
            }
        }

        static void MarcarCancelada(Cita cita)
        {
            if (cita.MarcarCancelada())
            {
                Console.WriteLine($"Cita cancelada - Estado: {cita.Estado}");
            }
            else
            {
                Console.WriteLine($"Rechazado por la clase: la cita esta {cita.Estado}");
            }
        }

        static void MostrarVigencia(Cita cita)
        {
            Console.WriteLine($"Estado: {cita.Estado}");
            Console.WriteLine($"Sigue vigente: {cita.EstaVigente()}");
        }

        // ---------- NIVEL 3: JAULA ----------

        static void MenuJaula(Jaula jaula)
        {
            string opcion = string.Empty;

            while (opcion != "0")
            {
                Console.WriteLine($"\n----- Jaula {jaula.Codigo} - {jaula.Tamano} -----");
                Console.WriteLine("1. Consultar estado");
                Console.WriteLine("2. Ingresar paciente");
                Console.WriteLine("3. Sumar un dia de estancia");
                Console.WriteLine("4. Calcular la cuenta");
                Console.WriteLine("5. Dar de alta medica");
                Console.WriteLine("0. Volver");
                Console.Write("Opcion: ");

                opcion = Console.ReadLine() ?? string.Empty;
                Console.WriteLine();

                switch (opcion)
                {
                    case "1": Console.WriteLine(jaula.ConsultarEstado()); break;
                    case "2": IngresarPaciente(jaula); break;
                    case "3": SumarDia(jaula); break;
                    case "4": MostrarCuenta(jaula); break;
                    case "5": DarDeAlta(jaula); break;
                    case "0": break;
                    default: Console.WriteLine("Opcion no valida"); break;
                }
            }
        }

        static void IngresarPaciente(Jaula jaula)
        {
            Console.Write("Nombre del animal: ");
            string nombreMascota = Console.ReadLine() ?? string.Empty;

            try
            {
                jaula.IngresarPaciente(nombreMascota);
                Console.WriteLine(jaula.ConsultarEstado());
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Rechazado por la clase: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Rechazado por la clase: {ex.Message}");
            }
        }

        static void SumarDia(Jaula jaula)
        {
            jaula.SumarDiaEstancia();
            Console.WriteLine($"Dias de estancia: {jaula.DiasEstancia}");
            Console.WriteLine("Una jaula libre no acumula dias");
        }

        static void MostrarCuenta(Jaula jaula)
        {
            Console.WriteLine($"{jaula.DiasEstancia} dias x {jaula.TarifaDia:C}");
            Console.WriteLine($"Cuenta actual: {jaula.CalcularCuenta():C}");
        }

        static void DarDeAlta(Jaula jaula)
        {
            try
            {
                decimal totalCobrar = jaula.DarDeAltaMedica();
                Console.WriteLine($"Alta dada - Total a cobrar al dueno: {totalCobrar:C}");
                Console.WriteLine(jaula.ConsultarEstado());
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Rechazado por la clase: {ex.Message}");
            }
        }

        // ---------- DEMOSTRACION AUTOMATICA ----------

        static void DemostracionAutomatica()
        {
            DemoFarmacia();
            DemoRecepcion();
            DemoHospitalizacion();
            DemoCasosProhibidos();
            ResumenGeneral();
        }

        static void DemoFarmacia()
        {
            Console.WriteLine("===== DEMO FARMACIA =====");

            MostrarFichaMedicamento(medicamento1);

            medicamento1.RecibirUnidades(20);
            Console.WriteLine($"\nMED-0142 recibe 20 unidades | Stock: {medicamento1.StockActual}");
            Console.WriteLine($"MED-0207 despacha 5 unidades: {medicamento2.DespacharUnidades(5)} | Stock: {medicamento2.StockActual}");

            Console.WriteLine($"MED-0315 vencido: {medicamento3.EstaVencido()}");
            Console.WriteLine($"MED-0142 necesita pedido: {medicamento1.NecesitaReabastecimiento()}");
            Console.WriteLine($"Valor inventario MED-0207: {medicamento2.CalcularValorInventario():C}");
        }

        static void DemoRecepcion()
        {
            Console.WriteLine("\n===== DEMO RECEPCION =====");

            cita1.MostrarFicha();

            Console.WriteLine($"\nCita 1 faltan {cita1.CalcularDiasFaltantes()} dias, vigente: {cita1.EstaVigente()}");
            Console.WriteLine($"Cita 3 faltan {cita3.CalcularDiasFaltantes()} dias, vigente: {cita3.EstaVigente()}");
            Console.WriteLine($"Atender cita 2: {cita2.MarcarAtendida()} - Estado: {cita2.Estado}");
            Console.WriteLine($"Cancelar cita 3: {cita3.MarcarCancelada()} - Estado: {cita3.Estado}");
        }

        static void DemoHospitalizacion()
        {
            Console.WriteLine("\n===== DEMO HOSPITALIZACION =====");

            if (!jaula1.Ocupado)
            {
                jaula1.IngresarPaciente("Manchas");
            }
            if (!jaula2.Ocupado)
            {
                jaula2.IngresarPaciente("Rocky");
            }

            jaula1.SumarDiaEstancia();
            jaula1.SumarDiaEstancia();
            jaula1.SumarDiaEstancia();
            jaula2.SumarDiaEstancia();
            jaula3.SumarDiaEstancia();

            Console.WriteLine($"Cuenta de J-07: {jaula1.CalcularCuenta():C}");
            Console.WriteLine(jaula2.ConsultarEstado());
            Console.WriteLine(jaula3.ConsultarEstado());

            if (jaula1.Ocupado)
            {
                Console.WriteLine($"Alta de Manchas - Total a cobrar: {jaula1.DarDeAltaMedica():C}");
                Console.WriteLine(jaula1.ConsultarEstado());
            }
        }

        static void DemoCasosProhibidos()
        {
            Console.WriteLine("\n===== CASOS QUE NO PUEDEN PASAR =====");

            Console.WriteLine($"\n[1] Despachar 200 de MED-0142 (hay {medicamento1.StockActual})");
            Console.WriteLine($"    Resultado: {medicamento1.DespacharUnidades(200)}");
            Console.WriteLine($"    Stock intacto: {medicamento1.StockActual}");

            cita2.MarcarAtendida();
            Console.WriteLine($"\n[2] Cancelar la cita 2 que esta {cita2.Estado}");
            Console.WriteLine($"    Resultado: {cita2.MarcarCancelada()} | Sigue {cita2.Estado}");

            cita3.MarcarCancelada();
            Console.WriteLine($"\n[3] Cancelar la cita 3 que ya esta {cita3.Estado}");
            Console.WriteLine($"    Resultado: {cita3.MarcarCancelada()}");

            if (!jaula2.Ocupado)
            {
                jaula2.IngresarPaciente("Rocky");
            }
            Console.WriteLine("\n[4] Meter a Nube en J-08, ocupada por Rocky");
            try
            {
                jaula2.IngresarPaciente("Nube");
                Console.WriteLine("    Nube entro (no deberia)");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"    Bloqueado por la clase: {ex.Message}");
            }

            Console.WriteLine("\n[5] Crear una cita con tarifa negativa");
            try
            {
                Cita citaInvalida = new Cita(4, "Simba", "8001234", "Dr. Andres Zapata",
                    new DateTime(2026, 10, 1, 10, 0, 0), "Consulta", -5000m);
                Console.WriteLine("    La cita se creo (no deberia)");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"    Bloqueado por la clase: {ex.Message}");
            }

            Console.WriteLine("\n[6] Crear una jaula con tarifa en cero");
            try
            {
                Jaula jaulaInvalida = new Jaula("J-99", TamanoJaula.Grande, 0m);
                Console.WriteLine("    La jaula se creo (no deberia)");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"    Bloqueado por la clase: {ex.Message}");
            }
        }

        // ---------- RESUMEN ----------

        static void ResumenGeneral()
        {
            Console.WriteLine("\n===== RESUMEN DE LAS TRES AREAS =====");

            decimal valorTotal = medicamento1.CalcularValorInventario()
                + medicamento2.CalcularValorInventario()
                + medicamento3.CalcularValorInventario();

            Console.WriteLine("FARMACIA: 3 referencias");
            Console.WriteLine($"  Valor total del inventario: {valorTotal:C}");
            Console.WriteLine($"  MED-0142: {medicamento1.StockActual} unidades | pedir: {medicamento1.NecesitaReabastecimiento()} | vencido: {medicamento1.EstaVencido()}");
            Console.WriteLine($"  MED-0207: {medicamento2.StockActual} unidades | pedir: {medicamento2.NecesitaReabastecimiento()} | vencido: {medicamento2.EstaVencido()}");
            Console.WriteLine($"  MED-0315: {medicamento3.StockActual} unidades | pedir: {medicamento3.NecesitaReabastecimiento()} | vencido: {medicamento3.EstaVencido()}");

            Console.WriteLine("RECEPCION: 3 citas");
            Console.WriteLine($"  Cita 1: {cita1.Estado} | vigente: {cita1.EstaVigente()} | faltan {cita1.CalcularDiasFaltantes()} dias");
            Console.WriteLine($"  Cita 2: {cita2.Estado} | tarifa {cita2.Tarifa:C}");
            Console.WriteLine($"  Cita 3: {cita3.Estado} | vigente: {cita3.EstaVigente()}");

            Console.WriteLine("HOSPITALIZACION: 3 jaulas");
            Console.WriteLine("  " + jaula1.ConsultarEstado());
            Console.WriteLine("  " + jaula2.ConsultarEstado());
            Console.WriteLine("  " + jaula3.ConsultarEstado());
        }

        static int LeerEntero(string mensaje)
        {
            Console.Write(mensaje);
            string texto = Console.ReadLine() ?? string.Empty;

            int numero;
            if (int.TryParse(texto, out numero))
            {
                return numero;
            }

            Console.WriteLine("Eso no es un numero, se toma 0");
            return 0;
        }
    }
}