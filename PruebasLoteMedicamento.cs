namespace PetCare
{
    public static class PruebasLoteMedicamento
    {
        private static int aprobadas;
        private static int fallidas;

        public static void EjecutarTodas()
        {
            aprobadas = 0;
            fallidas = 0;
            DateOnly hoy = DateOnly.FromDateTime(DateTime.Today);

            Console.WriteLine("===== Pruebas de LoteMedicamento =====");

            var vigente = new LoteMedicamento("L-203", "Amoxicilina 500 mg", hoy.AddMonths(6), 2500m);
            Verificar("Lote vigente no está vencido", !vigente.EstaVencido());
            Verificar("Lote vigente pasa la validación",
                NoLanza(() => vigente.ValidarFacturacion(2)));

            var vencido = new LoteMedicamento("L-118", "Meloxicam 1 mg", hoy.AddDays(-1), 1800m);
            Verificar("Lote vencido ayer sí está vencido", vencido.EstaVencido());
            Verificar("Lote vencido no pasa la validación",
                Lanza<InvalidOperationException>(() => vencido.ValidarFacturacion(1)));

            var venceHoy = new LoteMedicamento("L-300", "Ivermectina 1 %", hoy, 4000m);
            Verificar("Lote que vence hoy todavía se puede vender", !venceHoy.EstaVencido());
            Verificar("Ese mismo lote mañana ya está vencido", venceHoy.EstaVencido(hoy.AddDays(1)));

            Verificar("Cantidad en cero se rechaza",
                Lanza<ArgumentException>(() => vigente.ValidarFacturacion(0)));

            Verificar("Descripción 'Amoxicilina 500 mg – Lote L-203'",
                vigente.ObtenerDescripcion() == "Amoxicilina 500 mg – Lote L-203");

            Verificar("Código vacío se rechaza",
                Lanza<ArgumentException>(() => new LoteMedicamento(" ", "Amoxicilina", hoy, 100m)));
            Verificar("Nombre vacío se rechaza",
                Lanza<ArgumentException>(() => new LoteMedicamento("L-1", "", hoy, 100m)));
            Verificar("Precio en cero se rechaza",
                Lanza<ArgumentException>(() => new LoteMedicamento("L-1", "Amoxicilina", hoy, 0m)));

            IFacturable item = vigente;
            Verificar("Por la interfaz: precio unitario 2.500", item.ObtenerPrecioUnitario() == 2500m);
            Verificar("Por la interfaz: subtotal de 2 unidades = 5.000",
                item.ObtenerPrecioUnitario() * 2 == 5000m);

            Console.WriteLine($"Resultado: {aprobadas} aprobadas, {fallidas} fallidas");
        }

        private static void Verificar(string nombre, bool condicion)
        {
            Console.WriteLine($"[{(condicion ? "OK" : "FALLA")}] {nombre}");
            if (condicion) aprobadas++; else fallidas++;
        }

        private static bool Lanza<TExcepcion>(Action accion) where TExcepcion : Exception
        {
            try
            {
                accion();
                return false;
            }
            catch (TExcepcion)
            {
                return true;
            }
        }

        private static bool NoLanza(Action accion)
        {
            try
            {
                accion();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
