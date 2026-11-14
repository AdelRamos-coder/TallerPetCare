using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCare;

namespace PetCareWeb.Datos.Configuraciones
{
    /// <summary>
    /// Columnas de la tabla Servicios que usa la cita de consulta.
    /// </summary>
    public class CitaVeterinariaConfiguracion : IEntityTypeConfiguration<CitaVeterinaria>
    {
        public void Configure(EntityTypeBuilder<CitaVeterinaria> builder)
        {
            builder.Property(c => c.CedulaCliente).HasMaxLength(20);
            builder.Property(c => c.Motivo).HasMaxLength(150);

            // Al leer de la base se llena el campo directamente, sin pasar por la validación del set.
            builder.Property(c => c.DuracionMinutos)
                .HasField("_duracionMinutos")
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.Property(c => c.Veterinario)
                .HasColumnName("VeterinarioAsignado")
                .HasMaxLength(100);

            builder.Property(c => c.Estado).HasConversion<string>().HasMaxLength(20);
        }
    }
}
