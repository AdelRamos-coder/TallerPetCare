using PetCare;

namespace PetCareWeb.Datos
{
    /// <summary>
    /// Datos que la migración inicial deja cargados: los dos usuarios de prueba
    /// y las 14 jaulas de la clínica. Las contraseñas están guardadas como hash
    /// generado con PasswordHasher (las contraseñas en texto están en el README).
    /// </summary>
    public static class DatosIniciales
    {
        public static readonly object[] Usuarios =
        {
            new
            {
                Id = 1,
                NombreUsuario = "mospina",
                NombreCompleto = "Marcela Ospina",
                ContrasenaHash = "AQAAAAIAAYagAAAAEL0v4EOzhRcZ6GHqml54BE3BYacJANUfvo8U+Qi5/5dMCUIagbZplGwViALCknUnGQ==",
                Rol = RolUsuario.Administrador
            },
            new
            {
                Id = 2,
                NombreUsuario = "dgomez",
                NombreCompleto = "Diana Gomez",
                ContrasenaHash = "AQAAAAIAAYagAAAAEDy0Usf05qeX9CaRVBT2MU9mgLFMzUVBfr2tIXVdJ7gWNEc6UnZoIuwLB6GtrRD9CA==",
                Rol = RolUsuario.Recepcionista
            }
        };

        public static readonly object[] Jaulas =
        {
            new { Id = 1, Codigo = "J-01", Tamano = TamanoJaula.Pequeno, TarifaDia = 25000m },
            new { Id = 2, Codigo = "J-02", Tamano = TamanoJaula.Pequeno, TarifaDia = 25000m },
            new { Id = 3, Codigo = "J-03", Tamano = TamanoJaula.Pequeno, TarifaDia = 25000m },
            new { Id = 4, Codigo = "J-04", Tamano = TamanoJaula.Pequeno, TarifaDia = 25000m },
            new { Id = 5, Codigo = "J-05", Tamano = TamanoJaula.Pequeno, TarifaDia = 25000m },
            new { Id = 6, Codigo = "J-06", Tamano = TamanoJaula.Mediano, TarifaDia = 38000m },
            new { Id = 7, Codigo = "J-07", Tamano = TamanoJaula.Mediano, TarifaDia = 38000m },
            new { Id = 8, Codigo = "J-08", Tamano = TamanoJaula.Mediano, TarifaDia = 38000m },
            new { Id = 9, Codigo = "J-09", Tamano = TamanoJaula.Mediano, TarifaDia = 38000m },
            new { Id = 10, Codigo = "J-10", Tamano = TamanoJaula.Mediano, TarifaDia = 38000m },
            new { Id = 11, Codigo = "J-11", Tamano = TamanoJaula.Grande, TarifaDia = 52000m },
            new { Id = 12, Codigo = "J-12", Tamano = TamanoJaula.Grande, TarifaDia = 52000m },
            new { Id = 13, Codigo = "J-13", Tamano = TamanoJaula.Grande, TarifaDia = 52000m },
            new { Id = 14, Codigo = "J-14", Tamano = TamanoJaula.Grande, TarifaDia = 52000m }
        };
    }
}
