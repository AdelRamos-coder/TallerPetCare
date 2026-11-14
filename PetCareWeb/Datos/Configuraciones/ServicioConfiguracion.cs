using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCare;

namespace PetCareWeb.Datos.Configuraciones
{
    /// <summary>
    /// Tabla Servicios: una sola tabla para toda la jerarquía (TPH).
    /// La columna Tipo dice qué clase es cada fila ('Cita' u 'Hospitalizacion')
    /// y Entity Framework Core la usa para crear el objeto correcto al leer.
    /// </summary>
    public class ServicioConfiguracion : IEntityTypeConfiguration<Servicio>
    {
        /// <summary>
        /// Nombre interno de la columna Tipo. Es distinto de "Tipo" para no chocar
        /// con propiedades de las clases hijas.
        /// </summary>
        public const string Discriminador = "TipoServicio";

        public void Configure(EntityTypeBuilder<Servicio> builder)
        {
            builder.ToTable("Servicios", tabla =>
            {
                tabla.HasCheckConstraint("CK_Servicios_Tipo", "[Tipo] IN (N'Cita', N'Hospitalizacion')");
                tabla.HasCheckConstraint("CK_Servicios_DuracionCita",
                    "[Tipo] <> N'Cita' OR ([DuracionMinutos] IS NOT NULL AND [DuracionMinutos] BETWEEN 15 AND 180)");
                tabla.HasCheckConstraint("CK_Servicios_DatosCita",
                    "[Tipo] <> N'Cita' OR ([CedulaCliente] IS NOT NULL AND [Motivo] IS NOT NULL AND [Estado] IS NOT NULL)");
                tabla.HasCheckConstraint("CK_Servicios_EstadoCita",
                    "[Estado] IS NULL OR [Estado] IN (N'Programada', N'Atendida', N'Cancelada')");
                tabla.HasCheckConstraint("CK_Servicios_TarifaHospitalizacion",
                    "[Tipo] <> N'Hospitalizacion' OR ([TarifaDiaria] IS NOT NULL AND [TarifaDiaria] > 0)");
                tabla.HasCheckConstraint("CK_Servicios_JaulaHospitalizacion",
                    "[Tipo] <> N'Hospitalizacion' OR [JaulaId] IS NOT NULL");
                tabla.HasCheckConstraint("CK_Servicios_FechaAlta",
                    "[FechaAlta] IS NULL OR [FechaAlta] >= [Fecha]");
            });

            builder.HasKey(s => s.Id).HasName("PK_Servicios");

            builder.HasDiscriminator<string>(Discriminador)
                .HasValue<CitaVeterinaria>("Cita")
                .HasValue<Hospitalizacion>("Hospitalizacion");

            builder.Property<string>(Discriminador)
                .HasColumnName("Tipo")
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(s => s.Codigo).HasMaxLength(20).IsRequired();
            builder.HasAlternateKey(s => s.Codigo).HasName("UQ_Servicios_Codigo");

            builder.Property(s => s.NombreMascota).HasMaxLength(80).IsRequired();
            builder.Property(s => s.Fecha).HasColumnType("datetime2");
        }
    }
}
