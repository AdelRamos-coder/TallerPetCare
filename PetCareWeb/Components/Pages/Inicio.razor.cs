using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using PetCare;
using PetCareWeb.Servicios;
using PetCareWeb.Utilidades;

namespace PetCareWeb.Components.Pages
{
    /// <summary>
    /// Pantalla principal después de iniciar sesión: muestra quién entró (HU-01 CA2) y el resumen del día.
    /// </summary>
    public partial class Inicio
    {
        [Inject]
        private ServicioCitas ServicioCitas { get; set; } = default!;

        [Inject]
        private ServicioHospitalizaciones ServicioHospitalizaciones { get; set; } = default!;

        [Inject]
        private ServicioLotes ServicioLotes { get; set; } = default!;

        [Inject]
        private ServicioFacturas ServicioFacturas { get; set; } = default!;

        [Inject]
        private ILogger<Inicio> Logger { get; set; } = default!;

        [CascadingParameter]
        private Task<AuthenticationState> EstadoAutenticacion { get; set; } = default!;

        private string saludo = string.Empty;
        private bool cargando = true;
        private string? mensajeError;

        private List<CitaVeterinaria> citasHoy = new();
        private List<EstadoJaula> jaulasOcupadas = new();
        private int totalJaulas;
        private List<LoteMedicamento> lotesVencidos = new();
        private List<LoteMedicamento> lotesPorAgotarse = new();
        private List<Hospitalizacion> hospitalizacionesActivas = new();
        private int facturasHoy;
        private decimal facturadoHoy;

        protected override async Task OnInitializedAsync()
        {
            AuthenticationState estado = await EstadoAutenticacion;
            saludo = $"Hola, {estado.User.ObtenerNombreCompleto()} · {Formato.TextoRol(estado.User.ObtenerRol())}";

            try
            {
                DateTime hoy = DateTime.Today;

                List<CitaVeterinaria> citas = await ServicioCitas.ListarAsync();
                citasHoy = citas
                    .Where(c => c.Fecha.Date == hoy)
                    .OrderBy(c => c.Fecha)
                    .ToList();

                List<EstadoJaula> jaulas = await ServicioHospitalizaciones.ListarJaulasAsync();
                totalJaulas = jaulas.Count;
                jaulasOcupadas = jaulas.Where(j => j.Ocupada).ToList();

                List<Hospitalizacion> hospitalizaciones = await ServicioHospitalizaciones.ListarAsync();
                hospitalizacionesActivas = hospitalizaciones.Where(h => h.FechaAlta == null).ToList();

                List<LoteMedicamento> lotes = await ServicioLotes.ListarAsync();
                lotesVencidos = lotes.Where(l => l.EstaVencido()).ToList();
                lotesPorAgotarse = lotes.Where(l => !l.EstaVencido() && l.NecesitaReabastecimiento()).ToList();

                List<Factura> facturas = await ServicioFacturas.ListarDeHoyAsync();
                List<Factura> vigentes = facturas.Where(f => !f.Anulada).ToList();
                facturasHoy = vigentes.Count;
                facturadoHoy = vigentes.Sum(f => f.Total);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "No se pudo cargar el resumen del día");
                mensajeError = "No pudimos comunicarnos con la base de datos. Recarga la página en unos minutos.";
            }
            finally
            {
                cargando = false;
            }
        }

        private string TextoCanceladas()
        {
            int canceladas = citasHoy.Count(c => c.Estado == EstadoCita.Cancelada);
            return canceladas switch
            {
                0 => "ninguna cancelada",
                1 => "una cancelada",
                _ => $"{canceladas} canceladas"
            };
        }

        private string AvisoFarmacia()
        {
            var partes = new List<string>();

            if (lotesVencidos.Count > 0)
            {
                partes.Add($"{string.Join(" y ", lotesVencidos.Select(l => l.Codigo))} vencidos.");
            }

            foreach (LoteMedicamento lote in lotesPorAgotarse)
            {
                partes.Add($"{lote.Codigo} con {lote.StockActual} unidades.");
            }

            return partes.Count == 0 ? "Sin lotes vencidos ni por agotarse." : string.Join(" ", partes);
        }

        private string AvisoHospitalizacion()
        {
            if (hospitalizacionesActivas.Count == 0)
            {
                return "No hay mascotas hospitalizadas.";
            }

            return string.Join(" ", hospitalizacionesActivas.Select(h =>
                $"{h.NombreMascota} ({h.Jaula.Codigo}) lleva {h.CalcularDias()} {(h.CalcularDias() == 1 ? "día" : "días")}."));
        }

        private string AvisoCaja()
        {
            return facturasHoy switch
            {
                0 => "Todavía no se han emitido facturas hoy.",
                1 => "Una factura vigente emitida hoy.",
                _ => $"{facturasHoy} facturas vigentes emitidas hoy."
            };
        }
    }
}
