using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using PetCare;
using PetCareWeb.Modelos;
using PetCareWeb.Servicios;
using PetCareWeb.Utilidades;

namespace PetCareWeb.Components.Pages
{
    /// <summary>
    /// F2 · Hospitalizaciones (HU-05 y HU-06).
    /// </summary>
    public partial class Hospitalizaciones
    {
        [Inject]
        private ServicioHospitalizaciones ServicioHospitalizaciones { get; set; } = default!;

        [Inject]
        private ILogger<Hospitalizaciones> Logger { get; set; } = default!;

        private List<Hospitalizacion> hospitalizaciones = new();
        private List<EstadoJaula> jaulas = new();
        private Hospitalizacion? seleccionada;
        private ModeloHospitalizacion modelo = NuevoModelo();

        private DateTime fechaAlta = AhoraSinSegundos();
        private bool confirmandoAlta;
        private bool guardando;

        private string? mensajeOk;
        private string? mensajeError;
        private string tituloError = "No se pudo completar la operación";

        /// <summary>Carga las hospitalizaciones y el estado de las jaulas al abrir la página.</summary>
        protected override async Task OnInitializedAsync()
        {
            await CargarConManejoAsync(null);
        }

        private string TituloAlta()
        {
            return seleccionada == null
                ? "Alta"
                : $"Alta de {seleccionada.Codigo} · {seleccionada.NombreMascota}";
        }

        private string TextoTarifa()
        {
            EstadoJaula? jaula = jaulas.FirstOrDefault(j => j.Id == modelo.JaulaId);

            return jaula == null
                ? "La fija la jaula al elegirla"
                : $"{Formato.Moneda(jaula.TarifaDia)} · la fija la jaula {jaula.Codigo}";
        }

        private static string TextoOcupadas(IGrouping<TamanoJaula, EstadoJaula> grupo)
        {
            List<EstadoJaula> ocupadas = grupo.Where(j => j.Ocupada).ToList();
            string texto = $"{ocupadas.Count} de {grupo.Count()}";

            if (ocupadas.Count > 0)
            {
                texto += " · " + string.Join(", ", ocupadas.Select(j => $"{j.Codigo} {j.NombreOcupante}"));
            }

            return texto;
        }

        private void Seleccionar(Hospitalizacion hospitalizacion)
        {
            seleccionada = hospitalizacion;
            fechaAlta = AhoraSinSegundos();
            CerrarMensajes();
        }

        private void CerrarMensajes()
        {
            mensajeOk = null;
            mensajeError = null;
        }

        private async Task RegistrarIngresoAsync()
        {
            if (guardando)
            {
                return;
            }

            CerrarMensajes();
            guardando = true;

            try
            {
                Hospitalizacion nueva = await ServicioHospitalizaciones.RegistrarIngresoAsync(
                    modelo.Codigo, modelo.NombreMascota, modelo.FechaIngreso!.Value, modelo.JaulaId);

                mensajeOk = $"{nueva.NombreMascota} ingresó con el código {nueva.Codigo}. Aparece como hospitalizada.";
                modelo = NuevoModelo();
                await CargarAsync(nueva.Id);
            }
            catch (ArgumentException ex)
            {
                MostrarError("Revisa los datos del ingreso", ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                MostrarError("No se pudo registrar el ingreso", ex.Message);
            }
            catch (DbUpdateException ex)
            {
                Logger.LogWarning(ex, "La base de datos rechazó el ingreso {Codigo}", modelo.Codigo);
                MostrarError("No se pudo registrar el ingreso",
                    "La base de datos rechazó el registro: el código ya existe o la jaula se ocupó hace un momento. Actualiza la página y revisa.");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error inesperado al registrar el ingreso {Codigo}", modelo.Codigo);
                MostrarError("No se pudo registrar el ingreso", MensajeSinConexion);
            }
            finally
            {
                guardando = false;
            }
        }

        private void PedirConfirmacionAlta()
        {
            CerrarMensajes();
            confirmandoAlta = true;
        }

        private void CancelarAlta()
        {
            confirmandoAlta = false;
        }

        private async Task DarDeAltaAsync()
        {
            if (guardando || seleccionada == null)
            {
                return;
            }

            guardando = true;
            int id = seleccionada.Id;

            try
            {
                bool registrada = await ServicioHospitalizaciones.DarDeAltaAsync(id, fechaAlta);

                if (registrada)
                {
                    mensajeOk = $"{seleccionada.NombreMascota} recibió el alta y la jaula {seleccionada.Jaula.Codigo} quedó libre.";
                }
                else
                {
                    MostrarError("El alta ya estaba registrada",
                        "Esta hospitalización ya tenía alta. Se conserva la primera fecha de alta.");
                }

                await CargarAsync(id);
            }
            catch (ArgumentException ex)
            {
                MostrarError("Revisa la fecha de alta", Formato.Oracion(ex.Message) + " Elige una fecha igual o posterior al ingreso.");
            }
            catch (InvalidOperationException ex)
            {
                MostrarError("No se pudo registrar el alta", ex.Message);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error inesperado al dar de alta la hospitalización {Id}", id);
                MostrarError("No se pudo registrar el alta", MensajeSinConexion);
            }
            finally
            {
                guardando = false;
                confirmandoAlta = false;
            }
        }

        private async Task CargarConManejoAsync(int? idSeleccionada)
        {
            try
            {
                await CargarAsync(idSeleccionada);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "No se pudieron cargar las hospitalizaciones");
                MostrarError("No pudimos cargar las hospitalizaciones", MensajeSinConexion);
            }
        }

        private async Task CargarAsync(int? idSeleccionada)
        {
            hospitalizaciones = await ServicioHospitalizaciones.ListarAsync();
            jaulas = await ServicioHospitalizaciones.ListarJaulasAsync();
            seleccionada = hospitalizaciones.FirstOrDefault(h => h.Id == idSeleccionada);
        }

        private void MostrarError(string titulo, string mensaje)
        {
            tituloError = titulo;
            mensajeError = mensaje;
        }

        private const string MensajeSinConexion =
            "No pudimos comunicarnos con la base de datos. Los datos quedaron como estaban; intenta de nuevo en unos minutos.";

        private static ModeloHospitalizacion NuevoModelo()
        {
            return new ModeloHospitalizacion { FechaIngreso = AhoraSinSegundos() };
        }

        private static DateTime AhoraSinSegundos()
        {
            DateTime ahora = DateTime.Now;
            return new DateTime(ahora.Year, ahora.Month, ahora.Day, ahora.Hour, ahora.Minute, 0);
        }
    }
}
