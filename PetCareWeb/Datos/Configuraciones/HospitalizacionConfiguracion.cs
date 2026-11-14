using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCare;

namespace PetCareWeb.Datos.Configuraciones
{
    /// <summary>
    /// Columnas de la tabla Servicios que usa la hospitalización.
    /// </summary>
    public class HospitalizacionConfiguracion : IEntityTypeConfiguration<Hospitalizacion>
    {
        public void Configure(EntityTypeBuilder<Hospitalizacion> builder)
        {
            builder.Property(h => h.TarifaDiaria).HasPrecision(18, 2);
            builder.Property(h => h.FechaAlta).HasColumnType("datetime2");
            builder.Ignore(h => h.EstaHospitalizado);

            builder.HasOne(h => h.Jaula)
                .WithMany()
                .HasForeignKey("JaulaId")
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Servicios_Jaulas_JaulaId");

            builder.HasIndex(new[] { "JaulaId" }, "IX_Servicios_JaulaId")
                .HasDatabaseName("IX_Servicios_JaulaId");

            // Una jaula no puede tener dos hospitalizaciones sin alta al mismo tiempo.
            builder.HasIndex(new[] { "JaulaId" }, "UX_Servicios_JaulaOcupada")
                .HasDatabaseName("UX_Servicios_JaulaOcupada")
                .IsUnique()
                .HasFilter("[JaulaId] IS NOT NULL AND [FechaAlta] IS NULL");
        }
    }
}
