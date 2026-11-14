using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using PetCare;
using PetCareWeb.Servicios;
using PetCareWeb.Utilidades;

namespace PetCareWeb.Components.Pages
{
    /// <summary>
    /// F5 · Detalle de una factura (HU-12 y HU-13). El administrador también puede anular desde aquí.
    /// </summary>
    public partial class DetalleFactura
    {
        private const string MensajeSinConexion =
            "No pudimos comunicarnos con la base de datos. Intenta de nuevo en unos minutos.";

        [Parameter]
        public int Id { get; set; }

        [Inject]
        private ServicioFacturas ServicioFacturas { get; set; } = default!;

        [Inject]
        private ILogger<DetalleFactura> Logger { get; set; } = default!;

        [CascadingParameter]
        private Task<AuthenticationState> EstadoAutenticacion { get; set; } = default!;

        private Factura? factura;
        private int usuarioId;
        private bool esAdministrador;
        private bool confirmandoAnulacion;
        private bool guardando;

        private string? mensajeCarga;
        private string? mensajeOk;
        private string? mensajeError;
        private string tituloError = "No se pudo anular la factura";

        protected override async Task OnInitializedAsync()
        {
            AuthenticationState estado = await EstadoAutenticacion;
            usuarioId = estado.User.ObtenerUsuarioId();
            esAdministrador = estado.User.EsAdministrador();
        }

        protected override async Task OnParametersSetAsync()
        {
            mensajeCarga = null;

            try
            {
                factura = await ServicioFacturas.ObtenerAsync(Id);

                if (factura == null)
                {
                    mensajeCarga = "No existe una factura con ese identificador. Vuelve a los resultados y elige otra.";
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "No se pudo cargar la factura {Id}", Id);
                mensajeCarga = MensajeSinConexion;
            }
        }

        private void CerrarMensajes()
        {
            mensajeOk = null;
            mensajeError = null;
        }

        private void PedirConfirmacionAnular()
        {
            CerrarMensajes();
            confirmandoAnulacion = true;
        }

        private void CancelarAnulacion()
        {
            confirmandoAnulacion = false;
        }

        private async Task AnularAsync()
        {
            if (guardando || factura == null)
            {
                return;
            }

            guardando = true;

            try
            {
                bool anulada = await ServicioFacturas.AnularAsync(factura.Id, usuarioId);

                if (anulada)
                {
                    mensajeOk = $"La factura {factura.Numero} quedó anulada.";
                }
                else
                {
                    tituloError = "La factura ya estaba anulada";
                    mensajeError = "No se hizo ningún cambio.";
                }

                factura = await ServicioFacturas.ObtenerAsync(factura.Id);
            }
            catch (InvalidOperationException ex)
            {
                tituloError = "No se pudo anular la factura";
                mensajeError = ex.Message;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error inesperado al anular la factura {Id}", factura.Id);
                tituloError = "No se pudo anular la factura";
                mensajeError = MensajeSinConexion;
            }
            finally
            {
                guardando = false;
                confirmandoAnulacion = false;
            }
        }
    }
}
