-- Datos de prueba para PetCare Web (base de datos PetCareWeb).
-- Se ejecuta una sola vez, después de aplicar la migración InicialPetCare:
--   sqlcmd -S "(localdb)\MSSQLLocalDB" -d PetCareWeb -i DatosDePrueba.sql
-- Si ya existen los datos (cita CIT-001), no hace nada.
SET NOCOUNT ON;
SET XACT_ABORT ON;
SET QUOTED_IDENTIFIER ON;   -- requerido por el índice filtrado de jaulas ocupadas
SET ANSI_NULLS ON;

IF EXISTS (SELECT 1 FROM Servicios WHERE Codigo = N'CIT-001')
BEGIN
    PRINT 'Los datos de prueba ya estaban cargados.';
    RETURN;
END;

BEGIN TRANSACTION;

------------------------------------------------------------
-- Citas de consulta (F1)
-- Costo: $60.000 hasta 30 min + $1.500 por minuto adicional
------------------------------------------------------------
INSERT INTO Servicios (Codigo, NombreMascota, Fecha, Tipo, CedulaCliente, Motivo, DuracionMinutos, VeterinarioAsignado, Estado)
VALUES
 (N'CIT-001', N'Firulais', '2026-11-03T09:00:00', N'Cita', N'1020304050', N'Vacunación anual y control general',     45, N'Dr. Restrepo',  N'Atendida'),
 (N'CIT-002', N'Michi',    '2026-11-05T10:30:00', N'Cita', N'1098765432', N'Revisión de oído',                       30, N'Dra. Salazar',  N'Atendida'),
 (N'CIT-003', N'Rocky',    '2026-11-10T14:00:00', N'Cita', N'79123456',   N'Cojera en pata trasera',                 60, N'Dr. Restrepo',  N'Atendida'),
 (N'CIT-004', N'Luna',     '2026-11-11T08:30:00', N'Cita', N'52987654',   N'Control posoperatorio',                  30, N'Dra. Salazar',  N'Atendida'),
 (N'CIT-005', N'Toby',     '2026-11-12T16:00:00', N'Cita', N'1012345678', N'Desparasitación',                        20, NULL,             N'Cancelada'),
 (N'CIT-006', N'Canela',   '2026-11-17T09:00:00', N'Cita', N'1030405060', N'Primera consulta de cachorro',           45, N'Dr. Restrepo',  N'Programada'),
 (N'CIT-007', N'Max',      '2026-11-18T11:00:00', N'Cita', N'80456789',   N'Problemas de piel y picazón',            30, N'Dra. Salazar',  N'Programada'),
 (N'CIT-008', N'Nala',     '2026-11-20T15:30:00', N'Cita', N'1020304050', N'Limpieza dental',                        90, NULL,             N'Programada'),
 (N'CIT-009', N'Simba',    '2026-11-24T10:00:00', N'Cita', N'1098765432', N'Vómito y pérdida de apetito',            30, N'Dr. Restrepo',  N'Programada');

------------------------------------------------------------
-- Hospitalizaciones (F2)
-- Activas: FechaAlta NULL (ocupan jaula). Costo: días x tarifa de la jaula.
------------------------------------------------------------
INSERT INTO Servicios (Codigo, NombreMascota, Fecha, Tipo, JaulaId, TarifaDiaria, FechaAlta)
SELECT v.Codigo, v.Mascota, v.Ingreso, N'Hospitalizacion', j.Id, j.TarifaDia, v.Alta
FROM (VALUES
 (N'HOS-101', N'Bruno',   CAST('2026-11-02T08:00:00' AS datetime2), N'J-07', CAST('2026-11-06T17:00:00' AS datetime2)),
 (N'HOS-102', N'Pelusa',  CAST('2026-11-09T12:00:00' AS datetime2), N'J-02', CAST('2026-11-11T10:00:00' AS datetime2)),
 (N'HOS-103', N'Thor',    CAST('2026-11-10T09:30:00' AS datetime2), N'J-12', NULL),
 (N'HOS-104', N'Kira',    CAST('2026-11-12T18:00:00' AS datetime2), N'J-06', NULL),
 (N'HOS-105', N'Coco',    CAST('2026-11-13T07:45:00' AS datetime2), N'J-01', NULL),
 (N'HOS-106', N'Zeus',    CAST('2026-11-14T08:15:00' AS datetime2), N'J-11', NULL)
) AS v(Codigo, Mascota, Ingreso, Jaula, Alta)
JOIN Jaulas j ON j.Codigo = v.Jaula;

------------------------------------------------------------
-- Lotes de medicamento (F3, F6)
-- Incluye un lote vencido, uno por vencer y uno con poco stock.
------------------------------------------------------------
INSERT INTO Productos (Codigo, Nombre, PrecioUnidad, Tipo, TipoMedicamento, FechaVencimiento, EnNevera, StockActual)
VALUES
 (N'L-301', N'Amoxicilina 250 mg',        18500, N'LoteMedicamento', N'Antibioticos',     '2027-06-30', 0, 38),
 (N'L-302', N'Vacuna antirrábica',        45000, N'LoteMedicamento', N'Vacunas',          '2027-02-28', 1, 25),
 (N'L-303', N'Meloxicam 1,5 mg/ml',       22000, N'LoteMedicamento', N'Analgesicos',      '2027-09-15', 0, 14),
 (N'L-304', N'Tramadol 50 mg',            12000, N'LoteMedicamento', N'Analgesicos',      '2026-12-05', 0, 30),
 (N'L-305', N'Ivermectina 1%',            16000, N'LoteMedicamento', N'Antiparasitarios', '2027-04-10', 0, 3),
 (N'L-306', N'Vacuna pentavalente canina',58000, N'LoteMedicamento', N'Vacunas',          '2027-01-20', 1, 12),
 (N'L-307', N'Enrofloxacina 50 mg',       21000, N'LoteMedicamento', N'Antibioticos',     '2026-10-31', 0, 9),
 (N'L-308', N'Pipeta antipulgas gatos',   35000, N'LoteMedicamento', N'Antiparasitarios', '2028-03-01', 0, 40);

------------------------------------------------------------
-- Facturas (F4, F5)
-- Los stocks de arriba ya descuentan lo vendido aquí.
------------------------------------------------------------
DECLARE @admin int = (SELECT Id FROM Usuarios WHERE NombreUsuario = N'mospina');
DECLARE @recep int = (SELECT Id FROM Usuarios WHERE NombreUsuario = N'dgomez');

INSERT INTO Facturas (Numero, FechaEmision, NombreCliente, UsuarioEmisorId, EstaAnulada)
VALUES
 (N'FAC-1001', '2026-11-03T10:00:00', N'Carlos Pérez',    @admin, 0),
 (N'FAC-1002', '2026-11-06T17:30:00', N'Laura Gómez',     @recep, 0),
 (N'FAC-1003', '2026-11-10T15:15:00', N'Andrés Ríos',     @recep, 1),
 (N'FAC-1004', '2026-11-11T11:00:00', N'Sofía Martínez',  @admin, 0),
 (N'FAC-1005', '2026-11-13T09:20:00', N'Valentina Cruz',  @recep, 0);

DECLARE @f1 int = (SELECT Id FROM Facturas WHERE Numero = N'FAC-1001');
DECLARE @f2 int = (SELECT Id FROM Facturas WHERE Numero = N'FAC-1002');
DECLARE @f3 int = (SELECT Id FROM Facturas WHERE Numero = N'FAC-1003');
DECLARE @f4 int = (SELECT Id FROM Facturas WHERE Numero = N'FAC-1004');
DECLARE @f5 int = (SELECT Id FROM Facturas WHERE Numero = N'FAC-1005');

-- Servicios facturados (Cantidad 1, precio = costo calculado)
INSERT INTO LineasFactura (FacturaId, ServicioId, ProductoId, Cantidad, PrecioUnitario)
SELECT v.FacturaId, s.Id, NULL, 1, v.Precio
FROM (VALUES
 (@f1, N'CIT-001',  82500),   -- 45 min
 (@f2, N'HOS-101', 152000),   -- 4 días x $38.000
 (@f3, N'CIT-003', 105000),   -- 60 min (factura anulada)
 (@f4, N'HOS-102',  50000),   -- 2 días x $25.000
 (@f4, N'CIT-004',  60000),   -- 30 min
 (@f5, N'CIT-002',  60000)    -- 30 min
) AS v(FacturaId, Codigo, Precio)
JOIN Servicios s ON s.Codigo = v.Codigo;

-- Productos vendidos
INSERT INTO LineasFactura (FacturaId, ServicioId, ProductoId, Cantidad, PrecioUnitario)
SELECT v.FacturaId, NULL, p.Id, v.Cantidad, p.PrecioUnidad
FROM (VALUES
 (@f1, N'L-302', 1),
 (@f1, N'L-301', 2),
 (@f2, N'L-303', 1),
 (@f4, N'L-304', 2),
 (@f5, N'L-308', 1)
) AS v(FacturaId, Codigo, Cantidad)
JOIN Productos p ON p.Codigo = v.Codigo;

COMMIT;

SELECT 'Citas' AS Tabla, COUNT(*) AS Filas FROM Servicios WHERE Tipo = N'Cita'
UNION ALL SELECT 'Hospitalizaciones', COUNT(*) FROM Servicios WHERE Tipo = N'Hospitalizacion'
UNION ALL SELECT 'Lotes', COUNT(*) FROM Productos
UNION ALL SELECT 'Facturas', COUNT(*) FROM Facturas
UNION ALL SELECT 'Lineas de factura', COUNT(*) FROM LineasFactura;
