using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCare;

namespace PetCareWeb.Datos.Configuraciones
{
    /// <summary>
    /// Columnas de la tabla Productos que usa el lote de medicamento.
    /// </summary>
    public class LoteMedicamentoConfiguracion : IEntityTypeConfiguration<LoteMedicamento>
    {
        public void Configure(EntityTypeBuilder<LoteMedicamento> builder)
        {
            builder.Property(l => l.Tipo)
                .HasColumnName("TipoMedicamento")
                .HasConversion<string>()
                .HasMaxLength(30);

            builder.Property(l => l.FechaVencimiento).HasColumnType("date");
            builder.Property(l => l.StockActual);
            builder.Property(l => l.EnNevera);
        }
    }
}
