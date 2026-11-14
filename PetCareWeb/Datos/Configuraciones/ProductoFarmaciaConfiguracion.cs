using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCare;

namespace PetCareWeb.Datos.Configuraciones
{
    /// <summary>
    /// Tabla Productos: una sola tabla para toda la jerarquía de farmacia (TPH).
    /// Hoy la columna Tipo solo admite 'LoteMedicamento'.
    /// </summary>
    public class ProductoFarmaciaConfiguracion : IEntityTypeConfiguration<ProductoFarmacia>
    {
        /// <summary>
        /// Nombre interno de la columna Tipo. Es distinto de "Tipo" porque
        /// LoteMedicamento ya tiene una propiedad Tipo (columna TipoMedicamento).
        /// </summary>
        public const string Discriminador = "TipoProducto";

        public void Configure(EntityTypeBuilder<ProductoFarmacia> builder)
        {
            builder.ToTable("Productos", tabla =>
            {
                tabla.HasCheckConstraint("CK_Productos_Tipo", "[Tipo] IN (N'LoteMedicamento')");
                tabla.HasCheckConstraint("CK_Productos_VencimientoLote",
                    "[Tipo] <> N'LoteMedicamento' OR [FechaVencimiento] IS NOT NULL");
                tabla.HasCheckConstraint("CK_Productos_DatosLote",
                    "[Tipo] <> N'LoteMedicamento' OR ([TipoMedicamento] IS NOT NULL AND [StockActual] IS NOT NULL AND [EnNevera] IS NOT NULL)");
                tabla.HasCheckConstraint("CK_Productos_TipoMedicamento",
                    "[TipoMedicamento] IS NULL OR [TipoMedicamento] IN (N'Antiparasitarios', N'Antibioticos', N'Analgesicos', N'Vacunas')");
                tabla.HasCheckConstraint("CK_Productos_StockActual", "[StockActual] IS NULL OR [StockActual] >= 0");
                tabla.HasCheckConstraint("CK_Productos_PrecioUnidad", "[PrecioUnidad] > 0");
            });

            builder.HasKey(p => p.Id).HasName("PK_Productos");

            builder.HasDiscriminator<string>(Discriminador)
                .HasValue<LoteMedicamento>("LoteMedicamento");

            builder.Property<string>(Discriminador)
                .HasColumnName("Tipo")
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(p => p.Codigo).HasMaxLength(20).IsRequired();
            builder.HasAlternateKey(p => p.Codigo).HasName("UQ_Productos_Codigo");

            builder.Property(p => p.NombreMedicamento)
                .HasColumnName("Nombre")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.PrecioUnitario)
                .HasColumnName("PrecioUnidad")
                .HasPrecision(18, 2);
        }
    }
}
