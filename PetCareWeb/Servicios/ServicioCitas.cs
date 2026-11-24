using Microsoft.EntityFrameworkCore;
using PetCare;
using PetCareWeb.Datos;

namespace PetCareWeb.Servicios
{
    /// <summary>
    /// Operaciones del formulario F1 (citas de consulta).
    /// Las reglas las aplica CitaVeterinaria; este servicio lee, guarda y traduce
    /// el rechazo de la base de datos por código repetido a un error del negocio.
    /// </summary>
    public class ServicioCitas
    {
        // Nombre de la restricción UNIQUE de la tabla Servicios en BaseDeDatos.sql.
        private const string RestriccionCodigo = "UQ_Servicios_Codigo";

        private readonly IDbContextFactory<PetCareDbContext> _fabrica;

        /// <summary>Recibe por inyección de dependencias la fábrica que crea un contexto corto por operación.</summary>
        public ServicioCitas(IDbContextFactory<PetCareDbContext> fabrica)
        {
            _fabrica = fabrica;
        }

        /// <summary>Todas las citas, de la más reciente a la más antigua.</summary>
        /// <exception cref="Microsoft.Data.SqlClient.SqlException">Si no hay conexión con la base de datos.</exception>
        public async Task<List<CitaVeterinaria>> ListarAsync()
        {
            await using PetCareDbContext db = await _fabrica.CreateDbContextAsync();

            return await db.Citas
                .AsNoTracking()
                .OrderByDescending(c => c.Fecha)
                .ToListAsync();
        }

        /// <summary>Registra una cita nueva con 30 minutos, sin veterinario y Programada.</summary>
        /// <exception cref="ArgumentException">Si faltan datos, superan el largo de su columna o la cédula no es numérica.</exception>
        /// <exception cref="ReglaNegocioException">Si la fecha no está entre hoy y un año hacia adelante.</exception>
        /// <exception cref="CodigoDuplicadoException">Si ya existe un servicio con ese código.</exception>
        /// <exception cref="DbUpdateException">Si la base de datos rechaza el registro por otro motivo.</exception>
        /// <exception cref="Microsoft.Data.SqlClient.SqlException">Si no hay conexión con la base de datos.</exception>
        public async Task<CitaVeterinaria> RegistrarAsync(string codigo, string nombreMascota, string cedulaCliente,
                                                          DateTime fecha, string motivo)
        {
            await using PetCareDbContext db = await _fabrica.CreateDbContextAsync();

            // Primero el dominio: si los datos no sirven, ni siquiera se consulta la base de datos.
            var cita = new CitaVeterinaria(codigo, nombreMascota, cedulaCliente, fecha, motivo);

            if (await db.Servicios.AnyAsync(s => s.Codigo == cita.Codigo))
            {
                throw new CodigoDuplicadoException(cita.Codigo);
            }

            db.Citas.Add(cita);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (EsCodigoRepetido(ex))
            {
                // Dos registros con el mismo código al mismo tiempo (por ejemplo, doble clic):
                // la revisión de arriba no lo alcanza a ver, pero la restricción UNIQUE sí.
                throw new CodigoDuplicadoException(cita.Codigo, ex);
            }

            return cita;
        }

        /// <summary>Cambia la duración de la cita. Si el valor no sirve, la cita conserva la que tenía.</summary>
        /// <exception cref="ReglaNegocioException">Si la duración está fuera de 15 a 180 minutos o la cita ya no existe.</exception>
        public async Task CambiarDuracionAsync(int citaId, int duracionMinutos)
        {
            await using PetCareDbContext db = await _fabrica.CreateDbContextAsync();

            CitaVeterinaria cita = await ObtenerAsync(db, citaId);
            cita.DuracionMinutos = duracionMinutos;

            await db.SaveChangesAsync();
        }

        /// <summary>Confirma el veterinario. Devuelve false si la cita ya tenía uno o ya no está programada.</summary>
        /// <exception cref="ArgumentException">Si el nombre está vacío o es demasiado largo.</exception>
        /// <exception cref="ReglaNegocioException">Si la cita ya no existe.</exception>
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
        /// <exception cref="ReglaNegocioException">Si la cita ya no existe.</exception>
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
        /// <exception cref="ReglaNegocioException">Si la cita ya no existe.</exception>
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
                ?? throw new ReglaNegocioException("La cita ya no existe. Actualiza la lista.");
        }

        // SQL Server nombra la restricción violada en el mensaje (errores 2601 y 2627).
        private static bool EsCodigoRepetido(DbUpdateException ex)
        {
            string mensaje = ex.InnerException?.Message ?? string.Empty;
            return mensaje.Contains(RestriccionCodigo, StringComparison.OrdinalIgnoreCase);
        }
    }
}
