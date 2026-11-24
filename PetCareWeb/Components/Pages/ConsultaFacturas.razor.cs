using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using PetCare;
using PetCareWeb.Servicios;

namespace PetCareWeb.Components.Pages
{
    /// <summary>
    /// F5 · Consulta de facturas (HU-15). La búsqueda se ejecuta con Enter o con el botón.
    /// </summary>
    public partial class ConsultaFacturas
    {
        [Inject]
        private ServicioFacturas ServicioFacturas { get; set; } = default!;

        [Inject]
        private ILogger<ConsultaFacturas> Logger { get; set; } = default!;

        private List<Factura> facturas = new();
        private string? numero;
        private string? cliente;
        private bool buscando;
        private string? mensajeError;

        /// <summary>Al abrir la página muestra todas las facturas, sin filtros.</summary>
        protected override async Task OnInitializedAsync()
        {
            await BuscarAsync();
        }

        private async Task AlPresionarTeclaAsync(KeyboardEventArgs e)
        {
            if (e.Key == "Enter" || e.Key == "NumpadEnter")
            {
                await BuscarAsync();
            }
        }

        private async Task BuscarAsync()
        {
            if (buscando)
            {
                return;
            }

            buscando = true;
            mensajeError = null;

            try
            {
                facturas = await ServicioFacturas.BuscarAsync(numero, cliente);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Falló la búsqueda de facturas");
                facturas = new List<Factura>();
                mensajeError = "No pudimos comunicarnos con la base de datos. Intenta de nuevo en unos minutos.";
            }
            finally
            {
                buscando = false;
            }
        }

        private void CerrarError()
        {
            mensajeError = null;
        }

        private string DetalleResultados()
        {
            string cantidad = facturas.Count == 1 ? "1 factura" : $"{facturas.Count} facturas";
            var criterios = new List<string>();

            if (!string.IsNullOrWhiteSpace(numero))
            {
                criterios.Add($"número \"{numero.Trim()}\"");
            }

            if (!string.IsNullOrWhiteSpace(cliente))
            {
                criterios.Add($"cliente \"{cliente.Trim()}\"");
            }

            return criterios.Count == 0 ? cantidad : $"{cantidad} para {string.Join(" y ", criterios)}";
        }
    }
}
