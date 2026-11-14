using Microsoft.EntityFrameworkCore;
using PetCare;
using PetCareWeb.Datos;

namespace PetCareWeb.Servicios
{
    /// <summary>
    /// Operaciones del formulario F1 (citas de consulta).
    /// Las reglas las aplica CitaVeterinaria; este servicio solo lee y guarda.
    /// </summary>
    public class ServicioCitas
    {
        private readonly IDbContextFactory<PetCareDbContext> _fabrica;

        public ServicioCitas(IDbContextFactory<PetCareDbContext> fabrica)
        {
            _fabrica = fabrica;
        }

        /// <summary>Todas las citas, de la más reciente a la más antigua.</summary>
        public async Task<List<CitaVeterinaria>> ListarAsync()
        {
            await using PetCareDbContext db = await _fabrica.CreateDbContextAsync();

            return await db.Citas
                .AsNoTracking()
                .OrderByDescending(c => c.Fecha)
                .ToListAsync();
        }

        /// <summary>Registra una cita nueva con 30 minutos, sin veterinario y Programada.</summary>
        public async Task<CitaVeterinaria> RegistrarAsync(string codigo, string nombreMascota, string cedulaCliente,
                                                          DateTime fecha, string motivo)
        {
            await using PetCareDbContext db = await _fabrica.CreateDbContextAsync();

            var cita = new CitaVeterinaria(codigo, nombreMascota, cedulaCliente, fecha, motivo);

            db.Citas.Add(cita);
            await db.SaveChangesAsync();

            return cita;
        }

        /// <summary>Cambia la duración de la cita.</summary>
        public async Task CambiarDuracionAsync(int citaId, int duracionMinutos)
        {
            await using PetCareDbContext db = await _fabrica.CreateDbContextAsync();

            CitaVeterinaria cita = await ObtenerAsync(db, citaId);
            cita.DuracionMinutos = duracionMinutos;

            await db.SaveChangesAsync();
        }

        /// <summary>Confirma el veterinario. Devuelve false si la cita ya tenía uno o ya no está programada.</summary>
        public async Task<bool> ConfirmarVeterinarioAsync(int citaId, string veterinario)
        {
            await using PetCareDbContext db = await _fabrica.CreateDbContextAsync();

            CitaVeterinaria cita = await ObtenerAsync(db, citaId);
            bool confirmado = cita.ConfirmarVeterinario(veterinario);

            if (confirmado)
            {
                await db.SaveChangesAsync();
            }

            return confirmado;
        }

        /// <summary>Marca la cita como atendida. Devuelve false si ya no estaba programada.</summary>
        public async Task<bool> MarcarAtendidaAsync(int citaId)
        {
            await using PetCareDbContext db = await _fabrica.CreateDbContextAsync();

            CitaVeterinaria cita = await ObtenerAsync(db, citaId);
            bool cambiada = cita.MarcarAtendida();

            if (cambiada)
            {
                await db.SaveChangesAsync();
            }

            return cambiada;
        }

        /// <summary>Cancela la cita. Devuelve false si ya no estaba programada.</summary>
        public async Task<bool> MarcarCanceladaAsync(int citaId)
        {
            await using PetCareDbContext db = await _fabrica.CreateDbContextAsync();

            CitaVeterinaria cita = await ObtenerAsync(db, citaId);
            bool cambiada = cita.MarcarCancelada();

            if (cambiada)
            {
                await db.SaveChangesAsync();
            }

            return cambiada;
        }

        private static async Task<CitaVeterinaria> ObtenerAsync(PetCareDbContext db, int citaId)
        {
            return await db.Citas.FirstOrDefaultAsync(c => c.Id == citaId)
                ?? throw new InvalidOperationException("La cita ya no existe. Actualiza la lista.");
        }
    }
}
