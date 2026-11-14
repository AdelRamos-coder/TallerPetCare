using Microsoft.AspNetCore.Components;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PetCare;
using PetCareWeb.Modelos;
using PetCareWeb.Servicios;
using PetCareWeb.Utilidades;

namespace PetCareWeb.Components.Pages
{
    /// <summary>
    /// F1 · Citas de consulta (HU-03, HU-04 y HU-17).
    /// Cada operación atrapa los errores de lo más específico a lo más general,
    /// muestra un mensaje que el usuario entiende y deja los datos como estaban.
    /// </summary>
    public partial class Citas
    {
        private const string MensajeSinConexion =
            "No pudimos comunicarnos con la base de datos. Los datos quedaron como estaban; intenta de nuevo en unos minutos.";

        private const string MensajeInesperado =
            "Ocurrió un problema inesperado y no se guardó nada. Intenta de nuevo; si se repite, avisa a la administración.";

        [Inject]
        private ServicioCitas ServicioCitas { get; set; } = default!;

        [Inject]
        private ILogger<Citas> Logger { get; set; } = default!;

        private List<CitaVeterinaria> citas = new();
        private CitaVeterinaria? seleccionada;
        private ModeloCita modelo = new();

        private int duracionNueva;
        private string? veterinarioNuevo;

        // Evita que un doble clic envíe la misma operación dos veces.
        private bool guardando;

        private string? mensajeCarga;
        private string? mensajeRegistro;
        private string? errorRegistro;
        private string tituloErrorRegistro = "No se pudo registrar la cita";
        private string? mensajeAjuste;
        private string? avisoAjuste;
        private string? errorAjuste;
        private string tituloErrorAjuste = "No se pudo guardar el cambio";

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await CargarAsync(null);
            }
            catch (SqlException ex)
            {
                Logger.LogError(ex, "Sin conexión al cargar las citas");
                mensajeCarga = MensajeSinConexion;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error inesperado al cargar las citas");
                mensajeCarga = MensajeInesperado;
            }
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
            if (guardando)
            {
                return;
            }

            seleccionada = cita;
            duracionNueva = cita.DuracionMinutos;
            veterinarioNuevo = null;
            CerrarMensajes();
        }

        private void CerrarCarga()
        {
            mensajeCarga = null;
        }

        private void CerrarMensajes()
        {
            mensajeRegistro = null;
            errorRegistro = null;
            mensajeAjuste = null;
            avisoAjuste = null;
            errorAjuste = null;
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
                CitaVeterinaria cita = await ServicioCitas.RegistrarAsync(
                    modelo.Codigo, modelo.NombreMascota, modelo.CedulaCliente, modelo.Fecha!.Value, modelo.Motivo);

                mensajeRegistro = $"La cita {cita.Codigo} de {cita.NombreMascota} quedó en la lista.";
                modelo = new ModeloCita();
                await CargarAsync(cita.Id);
            }
            catch (CodigoDuplicadoException ex)
            {
                MostrarErrorRegistro("El código ya existe", ex.Message);
            }
            catch (ReglaNegocioException ex)
            {
                MostrarErrorRegistro("La cita no cumple una regla de la clínica", Formato.Oracion(ex.Message));
            }
            catch (ArgumentException ex)
            {
                MostrarErrorRegistro("Revisa los datos de la cita", Formato.Oracion(ex.Message));
            }
            catch (DbUpdateException ex)
            {
                Logger.LogWarning(ex, "La base de datos rechazó la cita {Codigo}", modelo.Codigo);
                MostrarErrorRegistro("No se pudo registrar la cita",
                    "La base de datos rechazó el registro. Revisa los datos e intenta de nuevo.");
            }
            catch (SqlException ex)
            {
                Logger.LogError(ex, "Sin conexión al registrar la cita {Codigo}", modelo.Codigo);
                MostrarErrorRegistro("No se pudo registrar la cita", MensajeSinConexion);
            }
            catch (InvalidOperationException ex)
            {
                // Las reglas del dominio llegan como ReglaNegocioException (atrapada arriba);
                // otra InvalidOperationException viene de la infraestructura y su texto no es para el usuario.
                Logger.LogError(ex, "Operación inválida al registrar la cita {Codigo}", modelo.Codigo);
                MostrarErrorRegistro("No se pudo registrar la cita", MensajeSinConexion);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error inesperado al registrar la cita {Codigo}", modelo.Codigo);
                MostrarErrorRegistro("No se pudo registrar la cita", MensajeInesperado);
            }
            finally
            {
                guardando = false;
            }
        }

        private async Task CambiarDuracionAsync()
        {
            if (seleccionada == null || duracionNueva == seleccionada.DuracionMinutos)
            {
                return;
            }

            int id = seleccionada.Id;
            string codigo = seleccionada.Codigo;
            int duracionAnterior = seleccionada.DuracionMinutos;
            int duracionPedida = duracionNueva;

            bool guardado = await EjecutarAjusteAsync(() => ServicioCitas.CambiarDuracionAsync(id, duracionPedida));

            if (guardado)
            {
                mensajeAjuste = $"La cita {codigo} ahora dura {duracionPedida} minutos.";
            }
            else
            {
                // HU-04 CA1: si el valor no sirve, la cita conserva la duración que tenía.
                duracionNueva = duracionAnterior;
            }
        }

        private async Task ConfirmarVeterinarioAsync()
        {
            if (seleccionada == null)
            {
                return;
            }

            int id = seleccionada.Id;
            string codigo = seleccionada.Codigo;
            string nombre = veterinarioNuevo ?? string.Empty;
            bool confirmado = false;

            bool guardado = await EjecutarAjusteAsync(async () =>
            {
                confirmado = await ServicioCitas.ConfirmarVeterinarioAsync(id, nombre);
            });

            if (!guardado)
            {
                return;
            }

            if (confirmado)
            {
                mensajeAjuste = $"{nombre.Trim()} atenderá la cita {codigo}.";
                veterinarioNuevo = null;
            }
            else
            {
                avisoAjuste = "La cita ya tiene veterinario o ya no está programada. El veterinario se asigna una sola vez.";
            }
        }

        private async Task MarcarAtendidaAsync()
        {
            if (seleccionada == null)
            {
                return;
            }

            int id = seleccionada.Id;
            string codigo = seleccionada.Codigo;
            bool cambiada = false;
            bool guardado = await EjecutarAjusteAsync(async () =>
            {
                cambiada = await ServicioCitas.MarcarAtendidaAsync(id);
            });

            if (guardado)
            {
                if (cambiada)
                {
                    mensajeAjuste = $"La cita {codigo} quedó como atendida.";
                }
                else
                {
                    avisoAjuste = "Solo una cita programada se puede marcar como atendida.";
                }
            }
        }

        private async Task MarcarCanceladaAsync()
        {
            if (seleccionada == null)
            {
                return;
            }

            int id = seleccionada.Id;
            string codigo = seleccionada.Codigo;
            bool cambiada = false;
            bool guardado = await EjecutarAjusteAsync(async () =>
            {
                cambiada = await ServicioCitas.MarcarCanceladaAsync(id);
            });

            if (guardado)
            {
                if (cambiada)
                {
                    mensajeAjuste = $"La cita {codigo} quedó cancelada y ya no se puede facturar.";
                }
                else
                {
                    avisoAjuste = "Solo una cita programada se puede cancelar.";
                }
            }
        }

        /// <summary>
        /// Ejecuta una operación del panel de ajuste con el mismo manejo de errores
        /// y vuelve a leer la lista para que la pantalla muestre lo que quedó guardado.
        /// </summary>
        /// <returns>true si la operación terminó sin excepciones.</returns>
        private async Task<bool> EjecutarAjusteAsync(Func<Task> operacion)
        {
            if (guardando || seleccionada == null)
            {
                return false;
            }

            CerrarMensajes();
            guardando = true;
            int id = seleccionada.Id;
            bool guardado = false;

            try
            {
                await operacion();
                guardado = true;
            }
            catch (ReglaNegocioException ex)
            {
                MostrarErrorAjuste("La clínica no permite ese cambio", Formato.Oracion(ex.Message) + " La cita no cambió.");
            }
            catch (ArgumentException ex)
            {
                MostrarErrorAjuste("Revisa el dato", Formato.Oracion(ex.Message));
            }
            catch (DbUpdateException ex)
            {
                Logger.LogWarning(ex, "La base de datos rechazó el cambio de la cita {Id}", id);
                MostrarErrorAjuste("No se pudo guardar el cambio", "La base de datos rechazó el cambio. La cita no cambió.");
            }
            catch (SqlException ex)
            {
                Logger.LogError(ex, "Sin conexión al cambiar la cita {Id}", id);
                MostrarErrorAjuste("No se pudo guardar el cambio", MensajeSinConexion);
            }
            catch (InvalidOperationException ex)
            {
                Logger.LogError(ex, "Operación inválida al cambiar la cita {Id}", id);
                MostrarErrorAjuste("No se pudo guardar el cambio", MensajeSinConexion);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error inesperado al cambiar la cita {Id}", id);
                MostrarErrorAjuste("No se pudo guardar el cambio", MensajeInesperado);
            }

            try
            {
                await CargarAsync(id);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "No se pudo recargar la lista de citas");
                mensajeCarga = MensajeSinConexion;
            }
            finally
            {
                guardando = false;
            }

            return guardado;
        }

        private void MostrarErrorRegistro(string titulo, string mensaje)
        {
            tituloErrorRegistro = titulo;
            errorRegistro = mensaje;
        }

        private void MostrarErrorAjuste(string titulo, string mensaje)
        {
            tituloErrorAjuste = titulo;
            errorAjuste = mensaje;
        }
    }
}
