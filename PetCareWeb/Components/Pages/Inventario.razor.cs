using Microsoft.AspNetCore.Components;
using PetCare;
using PetCareWeb.Components.Compartidos;
using PetCareWeb.Servicios;

namespace PetCareWeb.Components.Pages
{
    /// <summary>
    /// F6 · Inventario (HU-08 y HU-18). Todo se muestra como IFacturable:
    /// la pantalla no pregunta de qué clase es cada elemento.
    /// </summary>
    public partial class Inventario
    {
        [Inject]
        private ServicioInventario ServicioInventario { get; set; } = default!;

        [Inject]
        private ILogger<Inventario> Logger { get; set; } = default!;

        private List<FilaInventario> servicios = new();
        private List<FilaInventario> productos = new();
        private string? mensajeError;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                List<IFacturable> listaServicios = await ServicioInventario.ListarServiciosAsync();
                List<IFacturable> listaProductos = await ServicioInventario.ListarProductosAsync();

                servicios = listaServicios.Select(FilaInventario.Crear).ToList();
                productos = listaProductos.Select(FilaInventario.Crear).ToList();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "No se pudo cargar el inventario");
                mensajeError = "No pudimos comunicarnos con la base de datos. Recarga la página en unos minutos.";
            }
        }
    }
}
