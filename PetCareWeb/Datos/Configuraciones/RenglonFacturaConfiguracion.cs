using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCare;

namespace PetCareWeb.Datos.Configuraciones
{
    /// <summary>
    /// Tabla LineasFactura. Cada renglón apunta a un servicio o a un producto, nunca a los dos
    /// (CK_LineasFactura_UnSoloOrigen). Al borrar una factura se borran sus renglones;
    /// los servicios y productos no se tocan.
    /// </summary>
    public class RenglonFacturaConfiguracion : IEntityTypeConfiguration<RenglonFactura>
    {
        /// <summary>Configura la tabla LineasFactura: columnas, llaves y restricciones.</summary>
        public void Configure(EntityTypeBuilder<RenglonFactura> builder)
        {
            builder.ToTable("LineasFactura", tabla =>
            {
                tabla.HasCheckConstraint("CK_LineasFactura_Cantidad", "[Cantidad] > 0");
                tabla.HasCheckConstraint("CK_LineasFactura_PrecioUnitario", "[PrecioUnitario] >= 0");
                tabla.HasCheckConstraint("CK_LineasFactura_UnSoloOrigen",
                    "([ServicioId] IS NOT NULL AND [ProductoId] IS NULL) OR ([ServicioId] IS NULL AND [ProductoId] IS NOT NULL)");
            });

            builder.HasKey(r => r.Id).HasName("PK_LineasFactura");

            builder.Property(r => r.Cantidad).IsRequired();
            builder.Property(r => r.PrecioUnitario).HasPrecision(18, 2);

            builder.HasOne<Factura>()
                .WithMany(f => f.Renglones)
                .HasForeignKey("FacturaId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_LineasFactura_Facturas_FacturaId");

            builder.HasOne(r => r.Servicio)
                .WithMany()
                .HasForeignKey("ServicioId")
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_LineasFactura_Servicios_ServicioId");

            builder.HasOne(r => r.Producto)
                .WithMany()
                .HasForeignKey("ProductoId")
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_LineasFactura_Productos_ProductoId");

            builder.HasIndex("FacturaId").HasDatabaseName("IX_LineasFactura_FacturaId");
            builder.HasIndex("ServicioId").HasDatabaseName("IX_LineasFactura_ServicioId");
            builder.HasIndex("ProductoId").HasDatabaseName("IX_LineasFactura_ProductoId");

            builder.Ignore(r => r.Item);
            builder.Ignore(r => r.Descripcion);
            builder.Ignore(r => r.Subtotal);
        }
    }
}
