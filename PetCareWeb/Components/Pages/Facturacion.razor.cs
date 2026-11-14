using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using PetCare;
using PetCareWeb.Modelos;
using PetCareWeb.Servicios;
using PetCareWeb.Utilidades;

namespace PetCareWeb.Components.Pages
{
    /// <summary>
    /// F4 · Facturación (HU-09 a HU-14).
    /// Sin Id muestra el formulario para crear la factura; con Id permite agregar renglones y anular.
    /// </summary>
    public partial class Facturacion
    {
        private const string MensajeSinConexion =
            "No pudimos comunicarnos con la base de datos. La factura quedó como estaba; intenta de nuevo en unos minutos.";

        [Parameter]
        public int? Id { get; set; }

        [Inject]
        private ServicioFacturas ServicioFacturas { get; set; } = default!;

        [Inject]
        private NavigationManager Navegacion { get; set; } = default!;

        [Inject]
        private ILogger<Facturacion> Logger { get; set; } = default!;

        [CascadingParameter]
        private Task<AuthenticationState> EstadoAutenticacion { get; set; } = default!;

        private Factura? factura;
        private List<Servicio> servicios = new();
        private List<ProductoFarmacia> productos = new();

        private ModeloFactura modeloFactura = new();
        private ModeloRenglonServicio modeloServicio = new();
        private ModeloRenglonProducto modeloProducto = new();

        private int usuarioId;
        private string nombreUsuario = string.Empty;
        private bool esAdministrador;

        private bool guardando;
        private bool confirmandoAnulacion;

        private string? mensajeCarga;
        private string? mensajeOk;
        private string? mensajeError;
        private string tituloError = "No se pudo completar la operación";

        protected override async Task OnInitializedAsync()
        {
            AuthenticationState estado = await EstadoAutenticacion;
            usuarioId = estado.User.ObtenerUsuarioId();
            nombreUsuario = estado.User.ObtenerNombreCompleto();
            esAdministrador = estado.User.EsAdministrador();
        }

        protected override async Task OnParametersSetAsync()
        {
            CerrarMensajes();
            mensajeCarga = null;
            factura = null;
            modeloFactura = new ModeloFactura();

            if (Id == null)
            {
                return;
            }

            try
            {
                factura = await ServicioFacturas.ObtenerAsync(Id.Value);

                if (factura == null)
                {
                    mensajeCarga = $"No existe una factura con el identificador {Id}. Crea una nueva o búscala en la consulta de facturas.";
                    return;
                }

                servicios = await ServicioFacturas.ListarServiciosAsync();
                productos = await ServicioFacturas.ListarProductosAsync();
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

        private async Task CrearAsync()
        {
            if (guardando)
            {
                return;
            }

            CerrarMensajes();
            guardando = true;
            int? idCreada = null;

            try
            {
                idCreada = await ServicioFacturas.CrearAsync(
                    modeloFactura.Numero, modeloFactura.FechaEmision!.Value, modeloFactura.NombreCliente, usuarioId);
            }
            catch (ArgumentException ex)
            {
                MostrarError("Revisa los datos de la factura", ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                MostrarError("No se pudo crear la factura", ex.Message);
            }
            catch (DbUpdateException ex)
            {
                Logger.LogWarning(ex, "La base de datos rechazó la factura {Numero}", modeloFactura.Numero);
                MostrarError("No se pudo crear la factura",
                    "La base de datos rechazó la factura. Revisa que el número no exista.");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error inesperado al crear la factura {Numero}", modeloFactura.Numero);
                MostrarError("No se pudo crear la factura", MensajeSinConexion);
            }
            finally
            {
                guardando = false;
            }

            if (idCreada != null)
            {
                Navegacion.NavigateTo($"facturacion/{idCreada}");
            }
        }

        private async Task AgregarServicioAsync()
        {
            await EjecutarSobreFacturaAsync(
                "No se pudo agregar el servicio",
                () => ServicioFacturas.AgregarServicioAsync(factura!.Id, modeloServicio.ServicioId),
                "El servicio quedó en la factura.");

            if (mensajeError == null)
            {
                modeloServicio = new ModeloRenglonServicio();
            }
        }

        private async Task AgregarProductoAsync()
        {
            await EjecutarSobreFacturaAsync(
                "No se pudo agregar el lote",
                () => ServicioFacturas.AgregarProductoAsync(factura!.Id, modeloProducto.ProductoId, modeloProducto.Cantidad!.Value),
                "El medicamento quedó en la factura.");

            if (mensajeError == null)
            {
                modeloProducto = new ModeloRenglonProducto();
            }
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
                    mensajeOk = $"La factura {factura.Numero} quedó anulada. Sus servicios y lotes se pueden facturar de nuevo.";
                }
                else
                {
                    MostrarError("La factura ya estaba anulada", "No se hizo ningún cambio.");
                }

                factura = await ServicioFacturas.ObtenerAsync(factura.Id);
            }
            catch (InvalidOperationException ex)
            {
                MostrarError("No se pudo anular la factura", ex.Message);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error inesperado al anular la factura {Id}", factura.Id);
                MostrarError("No se pudo anular la factura", MensajeSinConexion);
            }
            finally
            {
                guardando = false;
                confirmandoAnulacion = false;
            }
        }

        // Agregar renglones comparte el mismo manejo: si el dominio rechaza, se muestra el motivo
        // y se vuelve a leer la factura para que la pantalla quede igual que en la base de datos.
        private async Task EjecutarSobreFacturaAsync(string tituloSiFalla, Func<Task> operacion, string mensajeSiFunciona)
        {
            if (guardando || factura == null)
            {
                return;
            }

            CerrarMensajes();
            guardando = true;

            try
            {
                await operacion();
                mensajeOk = mensajeSiFunciona;
            }
            catch (ReglaNegocioException ex)
            {
                MostrarError(tituloSiFalla, Formato.Oracion(ex.Message) + " La factura queda igual que antes.");
            }
            catch (ArgumentException ex)
            {
                MostrarError(tituloSiFalla, ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                MostrarError(tituloSiFalla, Formato.Oracion(ex.Message) + " La factura queda igual que antes.");
            }
            catch (DbUpdateException ex)
            {
                Logger.LogWarning(ex, "La base de datos rechazó un renglón de la factura {Id}", factura.Id);
                MostrarError(tituloSiFalla, "La base de datos rechazó el renglón. La factura queda igual que antes.");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error inesperado al agregar un renglón a la factura {Id}", factura.Id);
                MostrarError(tituloSiFalla, MensajeSinConexion);
            }

            try
            {
                factura = await ServicioFacturas.ObtenerAsync(factura.Id) ?? factura;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "No se pudo recargar la factura {Id}", factura.Id);
            }
            finally
            {
                guardando = false;
            }
        }

        private void MostrarError(string titulo, string mensaje)
        {
            tituloError = titulo;
            mensajeError = mensaje;
        }
    }
}
