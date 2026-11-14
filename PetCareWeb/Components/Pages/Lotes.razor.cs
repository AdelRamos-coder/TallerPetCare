using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using PetCare;
using PetCareWeb.Modelos;
using PetCareWeb.Servicios;

namespace PetCareWeb.Components.Pages
{
    /// <summary>
    /// F3 · Lotes de medicamento (HU-07 y HU-08).
    /// </summary>
    public partial class Lotes
    {
        private const string MensajeSinConexion =
            "No pudimos comunicarnos con la base de datos. Los datos quedaron como estaban; intenta de nuevo en unos minutos.";

        [Inject]
        private ServicioLotes ServicioLotes { get; set; } = default!;

        [Inject]
        private ILogger<Lotes> Logger { get; set; } = default!;

        private List<LoteMedicamento> lotes = new();
        private ModeloLote modelo = new();
        private bool guardando;

        private string? mensajeOk;
        private string? mensajeError;
        private string tituloError = "No se pudo registrar el lote";

        protected override async Task OnInitializedAsync()
        {
            try
            {
                lotes = await ServicioLotes.ListarAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "No se pudieron cargar los lotes");
                MostrarError("No pudimos cargar los lotes", MensajeSinConexion);
            }
        }

        private string DetalleLista()
        {
            int vencidos = lotes.Count(l => l.EstaVencido());
            return $"{lotes.Count} lotes · {vencidos} vencidos";
        }

        private void CerrarMensajes()
        {
            mensajeOk = null;
            mensajeError = null;
        }

        private async Task RegistrarAsync()
        {
            if (guardando)
            {
                return;
            }

            CerrarMensajes();
            guardando = true;

            try
            {
                LoteMedicamento lote = await ServicioLotes.RegistrarAsync(
                    modelo.Codigo,
                    modelo.NombreMedicamento,
                    modelo.Tipo!.Value,
                    modelo.FechaVencimiento!.Value,
                    modelo.PrecioUnitario!.Value,
                    modelo.StockInicial!.Value,
                    modelo.EnNevera);

                mensajeOk = lote.EstaVencido()
                    ? $"El lote {lote.Codigo} quedó registrado, pero ya está vencido: no se podrá vender."
                    : $"El lote {lote.Codigo} de {lote.NombreMedicamento} quedó en la lista.";

                modelo = new ModeloLote();
                lotes = await ServicioLotes.ListarAsync();
            }
            catch (ArgumentException ex)
            {
                MostrarError("Revisa los datos del lote", ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                MostrarError("No se pudo registrar el lote", ex.Message);
            }
            catch (DbUpdateException ex)
            {
                Logger.LogWarning(ex, "La base de datos rechazó el lote {Codigo}", modelo.Codigo);
                MostrarError("No se pudo registrar el lote",
                    "La base de datos rechazó el registro. Revisa que el código no exista y que los datos estén completos.");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error inesperado al registrar el lote {Codigo}", modelo.Codigo);
                MostrarError("No se pudo registrar el lote", MensajeSinConexion);
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
