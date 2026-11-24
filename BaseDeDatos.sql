IF DB_ID(N'PetCareWeb') IS NULL
    CREATE DATABASE PetCareWeb;
GO

USE PetCareWeb;
GO

CREATE TABLE dbo.Usuarios (
    Id INT IDENTITY(1,1) NOT NULL,
    NombreUsuario NVARCHAR(50) NOT NULL,
    NombreCompleto NVARCHAR(100) NOT NULL,
    ContrasenaHash NVARCHAR(200) NOT NULL,
    Rol NVARCHAR(20) NOT NULL,
    CONSTRAINT PK_Usuarios PRIMARY KEY (Id),
    CONSTRAINT UQ_Usuarios_NombreUsuario UNIQUE (NombreUsuario),
    CONSTRAINT CK_Usuarios_Rol CHECK (Rol IN (N'Recepcionista', N'Administrador'))
);
GO

CREATE TABLE dbo.Jaulas (
    Id INT IDENTITY(1,1) NOT NULL,
    Codigo NVARCHAR(10) NOT NULL,
    Tamano NVARCHAR(10) NOT NULL,
    TarifaDia DECIMAL(18,2) NOT NULL,
    CONSTRAINT PK_Jaulas PRIMARY KEY (Id),
    CONSTRAINT UQ_Jaulas_Codigo UNIQUE (Codigo),
    CONSTRAINT CK_Jaulas_Tamano CHECK (Tamano IN (N'Pequeno', N'Mediano', N'Grande')),
    CONSTRAINT CK_Jaulas_TarifaDia CHECK (TarifaDia > 0)
);
GO

CREATE TABLE dbo.Facturas (
    Id INT IDENTITY(1,1) NOT NULL,
    Numero NVARCHAR(20) NOT NULL,
    FechaEmision DATETIME2 NOT NULL,
    NombreCliente NVARCHAR(100) NOT NULL,
    EstaAnulada BIT NOT NULL CONSTRAINT DF_Facturas_EstaAnulada DEFAULT (0),
    UsuarioEmisorId INT NOT NULL,
    CONSTRAINT PK_Facturas PRIMARY KEY (Id),
    CONSTRAINT UQ_Facturas_Numero UNIQUE (Numero),
    CONSTRAINT FK_Facturas_Usuarios_UsuarioEmisorId FOREIGN KEY (UsuarioEmisorId)
        REFERENCES dbo.Usuarios (Id) ON DELETE NO ACTION
);
GO

CREATE TABLE dbo.Servicios (
    Id INT IDENTITY(1,1) NOT NULL,
    Codigo NVARCHAR(20) NOT NULL,
    Tipo NVARCHAR(20) NOT NULL,
    NombreMascota NVARCHAR(80) NOT NULL,
    Fecha DATETIME2 NOT NULL,
    DuracionMinutos INT NULL,
    VeterinarioAsignado NVARCHAR(100) NULL,
    CedulaCliente NVARCHAR(20) NULL,
    Motivo NVARCHAR(150) NULL,
    Estado NVARCHAR(20) NULL,
    TarifaDiaria DECIMAL(18,2) NULL,
    FechaAlta DATETIME2 NULL,
    JaulaId INT NULL,
    CONSTRAINT PK_Servicios PRIMARY KEY (Id),
    CONSTRAINT UQ_Servicios_Codigo UNIQUE (Codigo),
    CONSTRAINT FK_Servicios_Jaulas_JaulaId FOREIGN KEY (JaulaId)
        REFERENCES dbo.Jaulas (Id) ON DELETE NO ACTION,
    CONSTRAINT CK_Servicios_Tipo CHECK (Tipo IN (N'Cita', N'Hospitalizacion')),
    CONSTRAINT CK_Servicios_DuracionCita CHECK (Tipo <> N'Cita' OR (DuracionMinutos IS NOT NULL AND DuracionMinutos BETWEEN 15 AND 180)),
    CONSTRAINT CK_Servicios_DatosCita CHECK (Tipo <> N'Cita' OR (CedulaCliente IS NOT NULL AND Motivo IS NOT NULL AND Estado IS NOT NULL)),
    CONSTRAINT CK_Servicios_EstadoCita CHECK (Estado IS NULL OR Estado IN (N'Programada', N'Atendida', N'Cancelada')),
    CONSTRAINT CK_Servicios_TarifaHospitalizacion CHECK (Tipo <> N'Hospitalizacion' OR (TarifaDiaria IS NOT NULL AND TarifaDiaria > 0)),
    CONSTRAINT CK_Servicios_JaulaHospitalizacion CHECK (Tipo <> N'Hospitalizacion' OR JaulaId IS NOT NULL),
    CONSTRAINT CK_Servicios_FechaAlta CHECK (FechaAlta IS NULL OR FechaAlta >= Fecha)
);
GO

CREATE TABLE dbo.Productos (
    Id INT IDENTITY(1,1) NOT NULL,
    Codigo NVARCHAR(20) NOT NULL,
    Tipo NVARCHAR(30) NOT NULL,
    Nombre NVARCHAR(100) NOT NULL,
    PrecioUnidad DECIMAL(18,2) NOT NULL,
    FechaVencimiento DATE NULL,
    TipoMedicamento NVARCHAR(30) NULL,
    StockActual INT NULL,
    EnNevera BIT NULL,
    CONSTRAINT PK_Productos PRIMARY KEY (Id),
    CONSTRAINT UQ_Productos_Codigo UNIQUE (Codigo),
    CONSTRAINT CK_Productos_Tipo CHECK (Tipo IN (N'LoteMedicamento')),
    CONSTRAINT CK_Productos_VencimientoLote CHECK (Tipo <> N'LoteMedicamento' OR FechaVencimiento IS NOT NULL),
    CONSTRAINT CK_Productos_DatosLote CHECK (Tipo <> N'LoteMedicamento' OR (TipoMedicamento IS NOT NULL AND StockActual IS NOT NULL AND EnNevera IS NOT NULL)),
    CONSTRAINT CK_Productos_TipoMedicamento CHECK (TipoMedicamento IS NULL OR TipoMedicamento IN (N'Antiparasitarios', N'Antibioticos', N'Analgesicos', N'Vacunas')),
    CONSTRAINT CK_Productos_StockActual CHECK (StockActual IS NULL OR StockActual >= 0),
    CONSTRAINT CK_Productos_PrecioUnidad CHECK (PrecioUnidad > 0)
);
GO

CREATE TABLE dbo.LineasFactura (
    Id INT IDENTITY(1,1) NOT NULL,
    FacturaId INT NOT NULL,
    ServicioId INT NULL,
    ProductoId INT NULL,
    Cantidad INT NOT NULL,
    PrecioUnitario DECIMAL(18,2) NOT NULL,
    CONSTRAINT PK_LineasFactura PRIMARY KEY (Id),
    CONSTRAINT FK_LineasFactura_Facturas_FacturaId FOREIGN KEY (FacturaId)
        REFERENCES dbo.Facturas (Id) ON DELETE CASCADE,
    CONSTRAINT FK_LineasFactura_Servicios_ServicioId FOREIGN KEY (ServicioId)
        REFERENCES dbo.Servicios (Id) ON DELETE NO ACTION,
    CONSTRAINT FK_LineasFactura_Productos_ProductoId FOREIGN KEY (ProductoId)
        REFERENCES dbo.Productos (Id) ON DELETE NO ACTION,
    CONSTRAINT CK_LineasFactura_Cantidad CHECK (Cantidad > 0),
    CONSTRAINT CK_LineasFactura_PrecioUnitario CHECK (PrecioUnitario >= 0),
    CONSTRAINT CK_LineasFactura_UnSoloOrigen CHECK (
        (ServicioId IS NOT NULL AND ProductoId IS NULL) OR
        (ServicioId IS NULL AND ProductoId IS NOT NULL)
    )
);
GO

CREATE INDEX IX_Facturas_UsuarioEmisorId ON dbo.Facturas (UsuarioEmisorId);
CREATE INDEX IX_Servicios_JaulaId ON dbo.Servicios (JaulaId);
CREATE UNIQUE INDEX UX_Servicios_JaulaOcupada ON dbo.Servicios (JaulaId) WHERE JaulaId IS NOT NULL AND FechaAlta IS NULL;
CREATE INDEX IX_LineasFactura_FacturaId ON dbo.LineasFactura (FacturaId);
CREATE INDEX IX_LineasFactura_ServicioId ON dbo.LineasFactura (ServicioId);
CREATE INDEX IX_LineasFactura_ProductoId ON dbo.LineasFactura (ProductoId);
GO
