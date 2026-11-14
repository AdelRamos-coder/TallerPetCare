using Microsoft.AspNetCore.Components;
using PetCare;
using PetCareWeb.Modelos;
using PetCareWeb.Servicios;

namespace PetCareWeb.Components.Pages
{
    /// <summary>
    /// F1 · Citas de consulta (HU-03 y HU-04).
    /// </summary>
    public partial class Citas
    {
        [Inject]
        private ServicioCitas ServicioCitas { get; set; } = default!;

        private List<CitaVeterinaria> citas = new();
        private CitaVeterinaria? seleccionada;
        private ModeloCita modelo = new();

        private int duracionNueva;
        private string? veterinarioNuevo;

        private string? mensajeRegistro;
        private string? mensajeAjuste;
        private string? avisoAjuste;

        protected override async Task OnInitializedAsync()
        {
            await CargarAsync(null);
        }

        private string TituloAjuste()
        {
            return seleccionada == null
                ? "Ajuste de la cita"
                : $"Ajuste de {seleccionada.Codigo} · {seleccionada.NombreMascota}";
        }

        private async Task CargarAsync(int? idSeleccionada)
        {
            citas = await ServicioCitas.ListarAsync();
            seleccionada = citas.FirstOrDefault(c => c.Id == idSeleccionada);
            duracionNueva = seleccionada?.DuracionMinutos ?? 30;
        }

        private void Seleccionar(CitaVeterinaria cita)
        {
            seleccionada = cita;
            duracionNueva = cita.DuracionMinutos;
            veterinarioNuevo = null;
            CerrarMensajes();
        }

        private void CerrarMensajes()
        {
            mensajeRegistro = null;
            mensajeAjuste = null;
            avisoAjuste = null;
        }

        private async Task RegistrarAsync()
        {
            CerrarMensajes();

            CitaVeterinaria cita = await ServicioCitas.RegistrarAsync(
                modelo.Codigo, modelo.NombreMascota, modelo.CedulaCliente, modelo.Fecha!.Value, modelo.Motivo);

            mensajeRegistro = $"La cita {cita.Codigo} de {cita.NombreMascota} quedó en la lista.";
            modelo = new ModeloCita();
            await CargarAsync(cita.Id);
        }

        private async Task CambiarDuracionAsync()
        {
            CerrarMensajes();

            await ServicioCitas.CambiarDuracionAsync(seleccionada!.Id, duracionNueva);

            mensajeAjuste = $"La cita {seleccionada.Codigo} ahora dura {duracionNueva} minutos.";
            await CargarAsync(seleccionada.Id);
        }

        private async Task ConfirmarVeterinarioAsync()
        {
            CerrarMensajes();

            bool confirmado = await ServicioCitas.ConfirmarVeterinarioAsync(seleccionada!.Id, veterinarioNuevo ?? string.Empty);

            if (confirmado)
            {
                mensajeAjuste = $"{veterinarioNuevo!.Trim()} atenderá la cita {seleccionada.Codigo}.";
                veterinarioNuevo = null;
            }
            else
            {
                avisoAjuste = "La cita ya tiene veterinario o ya no está programada. El veterinario se asigna una sola vez.";
            }

            await CargarAsync(seleccionada.Id);
        }

        private async Task MarcarAtendidaAsync()
        {
            CerrarMensajes();

            bool cambiada = await ServicioCitas.MarcarAtendidaAsync(seleccionada!.Id);

            if (cambiada)
            {
                mensajeAjuste = $"La cita {seleccionada.Codigo} quedó como atendida.";
            }
            else
            {
                avisoAjuste = "Solo una cita programada se puede marcar como atendida.";
            }

            await CargarAsync(seleccionada.Id);
        }

        private async Task MarcarCanceladaAsync()
        {
            CerrarMensajes();

            bool cambiada = await ServicioCitas.MarcarCanceladaAsync(seleccionada!.Id);

            if (cambiada)
            {
                mensajeAjuste = $"La cita {seleccionada.Codigo} quedó cancelada y ya no se puede facturar.";
            }
            else
            {
                avisoAjuste = "Solo una cita programada se puede cancelar.";
            }

            await CargarAsync(seleccionada.Id);
        }
    }
}
