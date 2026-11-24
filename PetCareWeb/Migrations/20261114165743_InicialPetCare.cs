using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PetCareWeb.Migrations
{
    /// <inheritdoc />
    public partial class InicialPetCare : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Jaulas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Tamano = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TarifaDia = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jaulas", x => x.Id);
                    table.UniqueConstraint("UQ_Jaulas_Codigo", x => x.Codigo);
                    table.CheckConstraint("CK_Jaulas_Tamano", "[Tamano] IN (N'Pequeno', N'Mediano', N'Grande')");
                    table.CheckConstraint("CK_Jaulas_TarifaDia", "[TarifaDia] > 0");
                });

            migrationBuilder.CreateTable(
                name: "Productos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PrecioUnidad = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    TipoMedicamento = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    FechaVencimiento = table.Column<DateOnly>(type: "date", nullable: true),
                    EnNevera = table.Column<bool>(type: "bit", nullable: true),
                    StockActual = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productos", x => x.Id);
                    table.UniqueConstraint("UQ_Productos_Codigo", x => x.Codigo);
                    table.CheckConstraint("CK_Productos_DatosLote", "[Tipo] <> N'LoteMedicamento' OR ([TipoMedicamento] IS NOT NULL AND [StockActual] IS NOT NULL AND [EnNevera] IS NOT NULL)");
                    table.CheckConstraint("CK_Productos_PrecioUnidad", "[PrecioUnidad] > 0");
                    table.CheckConstraint("CK_Productos_StockActual", "[StockActual] IS NULL OR [StockActual] >= 0");
                    table.CheckConstraint("CK_Productos_Tipo", "[Tipo] IN (N'LoteMedicamento')");
                    table.CheckConstraint("CK_Productos_TipoMedicamento", "[TipoMedicamento] IS NULL OR [TipoMedicamento] IN (N'Antiparasitarios', N'Antibioticos', N'Analgesicos', N'Vacunas')");
                    table.CheckConstraint("CK_Productos_VencimientoLote", "[Tipo] <> N'LoteMedicamento' OR [FechaVencimiento] IS NOT NULL");
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreUsuario = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NombreCompleto = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ContrasenaHash = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Rol = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                    table.UniqueConstraint("UQ_Usuarios_NombreUsuario", x => x.NombreUsuario);
                    table.CheckConstraint("CK_Usuarios_Rol", "[Rol] IN (N'Recepcionista', N'Administrador')");
                });

            migrationBuilder.CreateTable(
                name: "Servicios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NombreMascota = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CedulaCliente = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Motivo = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    DuracionMinutos = table.Column<int>(type: "int", nullable: true),
                    VeterinarioAsignado = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    JaulaId = table.Column<int>(type: "int", nullable: true),
                    TarifaDiaria = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servicios", x => x.Id);
                    table.UniqueConstraint("UQ_Servicios_Codigo", x => x.Codigo);
                    table.CheckConstraint("CK_Servicios_DatosCita", "[Tipo] <> N'Cita' OR ([CedulaCliente] IS NOT NULL AND [Motivo] IS NOT NULL AND [Estado] IS NOT NULL)");
                    table.CheckConstraint("CK_Servicios_DuracionCita", "[Tipo] <> N'Cita' OR ([DuracionMinutos] IS NOT NULL AND [DuracionMinutos] BETWEEN 15 AND 180)");
                    table.CheckConstraint("CK_Servicios_EstadoCita", "[Estado] IS NULL OR [Estado] IN (N'Programada', N'Atendida', N'Cancelada')");
                    table.CheckConstraint("CK_Servicios_FechaAlta", "[FechaAlta] IS NULL OR [FechaAlta] >= [Fecha]");
                    table.CheckConstraint("CK_Servicios_JaulaHospitalizacion", "[Tipo] <> N'Hospitalizacion' OR [JaulaId] IS NOT NULL");
                    table.CheckConstraint("CK_Servicios_TarifaHospitalizacion", "[Tipo] <> N'Hospitalizacion' OR ([TarifaDiaria] IS NOT NULL AND [TarifaDiaria] > 0)");
                    table.CheckConstraint("CK_Servicios_Tipo", "[Tipo] IN (N'Cita', N'Hospitalizacion')");
                    table.ForeignKey(
                        name: "FK_Servicios_Jaulas_JaulaId",
                        column: x => x.JaulaId,
                        principalTable: "Jaulas",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Facturas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Numero = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FechaEmision = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NombreCliente = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UsuarioEmisorId = table.Column<int>(type: "int", nullable: false),
                    EstaAnulada = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facturas", x => x.Id);
                    table.UniqueConstraint("UQ_Facturas_Numero", x => x.Numero);
                    table.ForeignKey(
                        name: "FK_Facturas_Usuarios_UsuarioEmisorId",
                        column: x => x.UsuarioEmisorId,
                        principalTable: "Usuarios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LineasFactura",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServicioId = table.Column<int>(type: "int", nullable: true),
                    ProductoId = table.Column<int>(type: "int", nullable: true),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    FacturaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LineasFactura", x => x.Id);
                    table.CheckConstraint("CK_LineasFactura_Cantidad", "[Cantidad] > 0");
                    table.CheckConstraint("CK_LineasFactura_PrecioUnitario", "[PrecioUnitario] >= 0");
                    table.CheckConstraint("CK_LineasFactura_UnSoloOrigen", "([ServicioId] IS NOT NULL AND [ProductoId] IS NULL) OR ([ServicioId] IS NULL AND [ProductoId] IS NOT NULL)");
                    table.ForeignKey(
                        name: "FK_LineasFactura_Facturas_FacturaId",
                        column: x => x.FacturaId,
                        principalTable: "Facturas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LineasFactura_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LineasFactura_Servicios_ServicioId",
                        column: x => x.ServicioId,
                        principalTable: "Servicios",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Jaulas",
                columns: new[] { "Id", "Codigo", "Tamano", "TarifaDia" },
                values: new object[,]
                {
                    { 1, "J-01", "Pequeno", 25000m },
                    { 2, "J-02", "Pequeno", 25000m },
                    { 3, "J-03", "Pequeno", 25000m },
                    { 4, "J-04", "Pequeno", 25000m },
                    { 5, "J-05", "Pequeno", 25000m },
                    { 6, "J-06", "Mediano", 38000m },
                    { 7, "J-07", "Mediano", 38000m },
                    { 8, "J-08", "Mediano", 38000m },
                    { 9, "J-09", "Mediano", 38000m },
                    { 10, "J-10", "Mediano", 38000m },
                    { 11, "J-11", "Grande", 52000m },
                    { 12, "J-12", "Grande", 52000m },
                    { 13, "J-13", "Grande", 52000m },
                    { 14, "J-14", "Grande", 52000m }
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "ContrasenaHash", "NombreCompleto", "NombreUsuario", "Rol" },
                values: new object[,]
                {
                    { 1, "AQAAAAIAAYagAAAAEL0v4EOzhRcZ6GHqml54BE3BYacJANUfvo8U+Qi5/5dMCUIagbZplGwViALCknUnGQ==", "Marcela Ospina", "mospina", "Administrador" },
                    { 2, "AQAAAAIAAYagAAAAEDy0Usf05qeX9CaRVBT2MU9mgLFMzUVBfr2tIXVdJ7gWNEc6UnZoIuwLB6GtrRD9CA==", "Diana Gomez", "dgomez", "Recepcionista" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Facturas_UsuarioEmisorId",
                table: "Facturas",
                column: "UsuarioEmisorId");

            migrationBuilder.CreateIndex(
                name: "IX_LineasFactura_FacturaId",
                table: "LineasFactura",
                column: "FacturaId");

            migrationBuilder.CreateIndex(
                name: "IX_LineasFactura_ProductoId",
                table: "LineasFactura",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_LineasFactura_ServicioId",
                table: "LineasFactura",
                column: "ServicioId");

            migrationBuilder.CreateIndex(
                name: "IX_Servicios_JaulaId",
                table: "Servicios",
                column: "JaulaId");

            migrationBuilder.CreateIndex(
                name: "UX_Servicios_JaulaOcupada",
                table: "Servicios",
                column: "JaulaId",
                unique: true,
                filter: "[JaulaId] IS NOT NULL AND [FechaAlta] IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LineasFactura");

            migrationBuilder.DropTable(
                name: "Facturas");

            migrationBuilder.DropTable(
                name: "Productos");

            migrationBuilder.DropTable(
                name: "Servicios");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Jaulas");
        }
    }
}
