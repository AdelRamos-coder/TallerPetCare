using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetCare;

namespace PetCareWeb.Datos.Configuraciones
{
    /// <summary>
    /// Tabla Usuarios.
    /// </summary>
    public class UsuarioConfiguracion : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuarios", tabla =>
                tabla.HasCheckConstraint("CK_Usuarios_Rol", "[Rol] IN (N'Recepcionista', N'Administrador')"));

            builder.HasKey(u => u.Id).HasName("PK_Usuarios");

            builder.Property(u => u.NombreUsuario).HasMaxLength(50).IsRequired();
            builder.HasAlternateKey(u => u.NombreUsuario).HasName("UQ_Usuarios_NombreUsuario");

            builder.Property(u => u.NombreCompleto).HasMaxLength(100).IsRequired();
            builder.Property(u => u.ContrasenaHash).HasMaxLength(200).IsRequired();
            builder.Property(u => u.Rol).HasConversion<string>().HasMaxLength(20).IsRequired();

            builder.Ignore(u => u.EsAdministrador);

            builder.HasData(DatosIniciales.Usuarios);
        }
    }
}
