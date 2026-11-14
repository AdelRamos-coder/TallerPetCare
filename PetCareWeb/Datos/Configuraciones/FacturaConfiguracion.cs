using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCare;

namespace PetCareWeb.Datos.Configuraciones
{
    /// <summary>
    /// Tabla Facturas. Los renglones se leen y escriben por el campo privado _renglones,
    /// así la lista sigue sin poder modificarse desde afuera.
    /// </summary>
    public class FacturaConfiguracion : IEntityTypeConfiguration<Factura>
    {
        public void Configure(EntityTypeBuilder<Factura> builder)
        {
            builder.ToTable("Facturas");

            builder.HasKey(f => f.Id).HasName("PK_Facturas");

            builder.Property(f => f.Numero).HasMaxLength(20).IsRequired();
            builder.HasAlternateKey(f => f.Numero).HasName("UQ_Facturas_Numero");

            builder.Property(f => f.FechaEmision).HasColumnType("datetime2");
            builder.Property(f => f.NombreCliente).HasMaxLength(100).IsRequired();

            builder.Property(f => f.Anulada)
                .HasColumnName("EstaAnulada")
                .HasDefaultValue(false);

            builder.HasOne(f => f.UsuarioEmisor)
                .WithMany()
                .HasForeignKey("UsuarioEmisorId")
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Facturas_Usuarios_UsuarioEmisorId");

            builder.HasIndex("UsuarioEmisorId").HasDatabaseName("IX_Facturas_UsuarioEmisorId");

            builder.Navigation(f => f.Renglones)
                .HasField("_renglones")
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.Ignore(f => f.Total);
        }
    }
}
