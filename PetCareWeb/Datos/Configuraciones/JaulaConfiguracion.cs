using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCare;

namespace PetCareWeb.Datos.Configuraciones
{
    /// <summary>
    /// Tabla Jaulas. El ocupante no se guarda: una jaula está ocupada cuando tiene
    /// una hospitalización sin fecha de alta (lo asegura el índice UX_Servicios_JaulaOcupada).
    /// </summary>
    public class JaulaConfiguracion : IEntityTypeConfiguration<Jaula>
    {
        public void Configure(EntityTypeBuilder<Jaula> builder)
        {
            builder.ToTable("Jaulas", tabla =>
            {
                tabla.HasCheckConstraint("CK_Jaulas_Tamano", "[Tamano] IN (N'Pequeno', N'Mediano', N'Grande')");
                tabla.HasCheckConstraint("CK_Jaulas_TarifaDia", "[TarifaDia] > 0");
            });

            builder.HasKey(j => j.Id).HasName("PK_Jaulas");

            builder.Property(j => j.Codigo).HasMaxLength(10).IsRequired();
            builder.HasAlternateKey(j => j.Codigo).HasName("UQ_Jaulas_Codigo");

            builder.Property(j => j.Tamano).HasConversion<string>().HasMaxLength(10).IsRequired();
            builder.Property(j => j.TarifaDia).HasPrecision(18, 2);

            builder.Ignore(j => j.Ocupante);
            builder.Ignore(j => j.Ocupado);

            builder.HasData(DatosIniciales.Jaulas);
        }
    }
}
