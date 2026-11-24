# Guía para ejecutar los ataques · Formulario F1 Citas de consulta

Esta guía dice cómo poner la aplicación en cada estado y qué datos exactos usar en cada ataque, para que el antes y el después se hagan con las mismas entradas. Los resultados (qué vio el usuario, qué excepción salió, qué quedó en la base) los anota el equipo en el Anexo A del informe con su captura.

## 1. Los dos estados del código

| Estado | Rama | Qué tiene el formulario de citas |
|---|---|---|
| Antes | `antes-robustez` | Ningún try/catch en la página. El dominio solo revisa campos vacíos y la duración (con ArgumentException). El servicio no revisa el código repetido antes de guardar. |
| Después | `despues-robustez` (y `main`) | ReglaNegocioException y CodigoDuplicadoException; validación de largos, cédula numérica y rango de fecha en CitaVeterinaria; catch ordenados con ILogger; bloqueo del doble clic con `finally`. |

Los dos estados usan el mismo esquema de base de datos, así que se prueban sobre la misma base.

### Pasar al estado antes

La migración está en `main`, así que se trae junto con la rama:

```
git fetch
git checkout antes-robustez
git checkout main -- PetCareWeb/Migrations
```

Ejecutar con F5. Para volver:

```
git reset --hard
git checkout main
```

### Pasar al estado después

```
git checkout main
```

## 2. Dónde se ve el registro (ILogger)

- Ejecutando con el perfil `https` o `http`: en la ventana de consola que abre Visual Studio.
- En Visual Studio: ventana **Salida**, opción **Mostrar salida de: ASP.NET Core Web Server**.
- Las entradas del formulario salen con la categoría `PetCareWeb.Components.Pages.Citas` (`warn` para rechazos de la base de datos, `fail` para errores sin conexión o inesperados).

## 3. Preparación (una sola vez, antes del primer ataque)

Iniciar sesión como `dgomez` / `Recep2026*`, entrar a **Citas** y registrar esta cita, que se usa en varios ataques:

| Campo | Valor |
|---|---|
| Código | `CIT-A01` |
| Mascota | `Firulais` |
| Cédula | `1036458721` |
| Fecha y hora | mañana a las 10:00 |
| Motivo | `Control general` |

## 4. Ataques

Para cada ataque: hacer la acción, tomar la captura de la pantalla, copiar lo que salió en el registro y revisar la tabla Servicios en SQL Server Management Studio:

```sql
SELECT Id, Codigo, NombreMascota, CedulaCliente, Fecha, DuracionMinutos, VeterinarioAsignado, Estado, LEN(Motivo) AS LargoMotivo
FROM dbo.Servicios
WHERE Tipo = N'Cita'
ORDER BY Id;
```

### A1 · Campos vacíos

1. Clic en **Registrar cita** con todos los campos vacíos.
2. Escribir tres espacios en Código, Mascota, Cédula y Motivo, dejar la fecha vacía y clic en **Registrar cita**.

### A2 · Valor fuera de la regla de negocio

1. Clic en la fila de `CIT-A01` en la lista.
2. En **Duración (minutos)** escribir `500` y presionar Tab.
3. Repetir con `0` y con `-10`.

Revisar que la duración de `CIT-A01` en la base siga en 30.

### A3 · Código duplicado

Registrar una cita nueva con el código que ya existe:

| Campo | Valor |
|---|---|
| Código | `CIT-A01` |
| Mascota | `Rocky` |
| Cédula | `1152447890` |
| Fecha y hora | mañana a las 11:00 |
| Motivo | `Control de peso` |

Después del mensaje, revisar si los datos digitados siguen en el formulario y cambiar solo el código a `CIT-A03` para registrarla.

### A4 · Base de datos caída

1. Con la aplicación abierta en **Citas**, detener SQL Server: `services.msc` > **SQL Server (MSSQLSERVER)** (o **SQL Server (SQLEXPRESS)**) > **Detener**.

   Si usan LocalDB, detener la instancia no sirve: se vuelve a encender sola en cuanto la aplicación se conecta. En ese caso pongan la base fuera de línea desde una terminal:

   ```
   sqlcmd -S "(localdb)\MSSQLLocalDB" -E -Q "ALTER DATABASE [PetCareWeb] SET OFFLINE WITH ROLLBACK IMMEDIATE"
   ```

   La aplicación recibe la misma SqlException que con el servidor apagado.
2. Intentar registrar:

| Campo | Valor |
|---|---|
| Código | `CIT-A04` |
| Mascota | `Luna` |
| Cédula | `43876512` |
| Fecha y hora | mañana a las 14:00 |
| Motivo | `Revisión de piel` |

3. Recargar la página (F5 del navegador) para probar la carga.
4. Volver a iniciar el servicio de SQL Server al terminar. Con LocalDB, volver a poner la base en línea:

   ```
   sqlcmd -S "(localdb)\MSSQLLocalDB" -E -Q "ALTER DATABASE [PetCareWeb] SET ONLINE"
   ```

Comparar el mensaje con el de A3: deben ser distintos.

### A5 · Operación inválida para el estado actual

1. Seleccionar `CIT-A01`, escribir `Dra. Lina Restrepo` en **Veterinario** y clic en **Confirmar veterinario**.
2. Escribir `Dr. Andrés Zapata` y clic otra vez en **Confirmar veterinario**.
3. Clic en **Cancelar** y luego en **Marcar atendida**.

Revisar en la base que el veterinario siga siendo `Dra. Lina Restrepo` y el estado `Cancelada`.

### A6 · Dato imposible o desbordado

1. Registrar con un motivo de 300 caracteres (la columna admite 150):

| Campo | Valor |
|---|---|
| Código | `CIT-A06` |
| Mascota | `Toby` |
| Cédula | `1017223344` |
| Fecha y hora | mañana a las 15:00 |
| Motivo | el texto de abajo, completo |

```
Control general de la mascota con revision de piel, oidos, dientes y peso; Control general de la mascota con revision de piel, oidos, dientes y peso; Control general de la mascota con revision de piel, oidos, dientes y peso; Control general de la mascota con revision de piel, oidos, dientes y peso; 
```

2. Registrar con fecha absurda: los mismos datos, motivo `Vacunación anual`, código `CIT-A06B` y fecha `31/12/9999 10:00`.
3. Repetir el paso 2 con código `CIT-A06C` y fecha `01/01/0001 10:00`.

### A7 · Diseñado por el equipo: cédula con letras y puntos

Categoría: formato de un dato que la base de datos sí acepta (la columna es texto), así que el error no lo detiene nadie si el dominio no lo revisa.

| Campo | Valor |
|---|---|
| Código | `CIT-A07` |
| Mascota | `Kira` |
| Cédula | `1.036.ABC` |
| Fecha y hora | mañana a las 16:00 |
| Motivo | `Desparasitación` |

Revisar en la base si quedó guardada una cita con esa cédula.

### A8 · Diseñado por el equipo: doble clic en Registrar cita

Categoría: interacción del usuario y concurrencia (dos envíos de la misma operación).

| Campo | Valor |
|---|---|
| Código | `CIT-A08` |
| Mascota | `Manchas` |
| Cédula | `1001234567` |
| Fecha y hora | mañana a las 16:30 |
| Motivo | `Vacunación anual` |

Hacer doble clic rápido sobre **Registrar cita** (o presionar Enter dos veces seguidas). Revisar en la base cuántas filas con `CIT-A08` quedaron y qué mostró la pantalla.

## 5. Dónde está cada defensa en el estado después

| Defensa | Archivo |
|---|---|
| Validación en pantalla (vacíos, largos, cédula) | `PetCareWeb/Modelos/ModeloCita.cs` |
| Reglas del dominio (largos, cédula, fecha, duración, cancelada) | `PetCare.Dominio/CitaVeterinaria.cs`, `PetCare.Dominio/Servicio.cs` |
| Excepciones propias | `PetCare.Dominio/ReglaNegocioException.cs`, `PetCare.Dominio/CodigoDuplicadoException.cs` |
| Código repetido antes de guardar y traducción de `UQ_Servicios_Codigo` | `PetCareWeb/Servicios/ServicioCitas.cs` |
| Catch ordenados, ILogger, `finally` y bloqueo del doble clic | `PetCareWeb/Components/Pages/Citas.razor.cs` |
| Sin reintentos automáticos (llega la SqlException original) | `PetCareWeb/Program.cs` |
