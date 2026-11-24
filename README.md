# PetCare Web

Aplicación web de la clínica veterinaria PetCare: citas de consulta, hospitalizaciones, lotes de medicamento, facturación y consulta de facturas, con usuarios que inician sesión y datos guardados en SQL Server.

Programación Orientada a Objetos (SD1007) · Grupo 053 · 2026-II · Producto de la Unidad 3.

## Integrantes

| Integrante | Usuario de GitHub |
|---|---|
| Ader Quitana | AderDevP |
| Luis Toledo | Jor3pore |
| Wilson Caraballo | wilson22c, wilsoncaraballo520-afk |
| Adel Ramos | AdelRamos-coder, adelramos424 |

## Tecnología

| Elemento | Versión |
|---|---|
| .NET | 10 (SDK 10.0) |
| Interfaz | Blazor Web App, renderizado interactivo del lado del servidor |
| Datos | Entity Framework Core 10 (Code First) con SQL Server |
| Editor | Visual Studio 2026 |

## Estructura de la solución

```
PetCareWeb.slnx
PetCare.Dominio/        Clases del dominio (las de la Unidad 2, adaptadas)
PetCareWeb/
  Datos/                PetCareDbContext, una configuración por tabla y datos iniciales
  Migrations/           Migración inicial de Entity Framework Core (InicialPetCare)
  Servicios/            Un servicio por formulario; las páginas no tocan el contexto
  Modelos/              Modelos de los formularios (EditForm con DataAnnotations)
  Utilidades/           Formatos de pantalla, datos de la sesión y datos de contacto de la clínica
  Components/
    Compartidos/        Componentes reutilizables (Seccion, Nota, DialogoConfirmacion, FacturaImprimible...)
    Layout/             Marco del personal, marco del inicio de sesión y marco de la página pública
    Pages/              Página pública, panel y F0 a F6, cada una con su archivo .razor.cs
  wwwroot/css/          Estilo "Expediente clínico" (petcare.css) y estilo de la página pública (landing.css)
  wwwroot/img/landing/  Imágenes de la página pública
BaseDeDatos.sql         Script de la base de datos a partir del diagrama (referencia del esquema)
DatosDePrueba.sql       Datos de ejemplo para probar todas las pantallas
img/                    Imágenes originales de la página pública
Evidencia_U3/           Informe de la evidencia de desempeño (.md y .pdf), capturas de los ataques y guía de ataques
```

## Cómo levantarlo en un equipo nuevo

### 1. Clonar y abrir

```
git clone https://github.com/AdelRamos-coder/TallerPetCare.git
```

Abrir `PetCareWeb.slnx` en Visual Studio 2026 y dejar `PetCareWeb` como proyecto de inicio.

### 2. Configurar la cadena de conexión

Está en `PetCareWeb/appsettings.json`, con el nombre `PetCare`:

```json
"ConnectionStrings": {
  "PetCare": "Server=localhost;Database=PetCareWeb;Trusted_Connection=True;TrustServerCertificate=True;Connect Timeout=5"
}
```

Cambien solo el valor de `Server` según su instalación:

| Instalación de SQL Server | Valor de Server |
|---|---|
| SQL Server Developer o Express como instancia por defecto | `localhost` |
| SQL Server Express con instancia con nombre | `localhost\SQLEXPRESS` |
| LocalDB que trae Visual Studio | `(localdb)\MSSQLLocalDB` |

`Connect Timeout=5` hace que, si SQL Server está apagado, la aplicación avise en 5 segundos en vez de quedarse esperando.

### 3. Crear la base de datos

La base de datos se crea con Entity Framework Core a partir de las clases de configuración de `PetCareWeb/Datos`, que reproducen `BaseDeDatos.sql` (tablas, llaves, UNIQUE, CHECK, índices y comportamiento de las llaves foráneas). La migración inicial, `InicialPetCare`, ya está en `PetCareWeb/Migrations`: no hay que generarla.

Basta con ejecutar la aplicación. Al arrancar, `Program.cs` llama a `Database.Migrate()`, que crea la base de datos `PetCareWeb` si no existe y aplica las migraciones que falten.

Si prefieren crearla antes de ejecutar, en Visual Studio: **Herramientas > Administrador de paquetes NuGet > Consola del Administrador de paquetes**, con `PetCareWeb` como proyecto predeterminado:

```
Update-Database
```

O desde una terminal en la carpeta del repositorio:

```
dotnet tool install --global dotnet-ef
dotnet ef database update --project PetCareWeb
```

### 4. Ejecutar

F5 en Visual Studio, o `dotnet run --project PetCareWeb`. Abre en `https://localhost:7180`.

### 5. Cargar datos de prueba (opcional)

`DatosDePrueba.sql` carga citas, hospitalizaciones, lotes y facturas de ejemplo para probar todas las pantallas. Se ejecuta una sola vez, después de que exista la base de datos: se puede abrir en SQL Server Management Studio sobre la base `PetCareWeb`, o correr desde una terminal (con el mismo `Server` de la cadena de conexión):

```
sqlcmd -S localhost -d PetCareWeb -i DatosDePrueba.sql
```

Si los datos ya estaban cargados, el script no hace nada.

## Usuarios de prueba

La migración inicial deja creados estos usuarios y las 14 jaulas de la clínica. Las contraseñas están guardadas como hash (PasswordHasher de ASP.NET Core); las de abajo son solo de desarrollo.

| Usuario | Contraseña | Nombre | Rol |
|---|---|---|---|
| `mospina` | `Admin2026*` | Marcela Ospina | Administrador |
| `dgomez` | `Recep2026*` | Diana Gomez | Recepcionista |

Jaulas: J-01 a J-05 pequeñas ($25.000 por día), J-06 a J-10 medianas ($38.000) y J-11 a J-14 grandes ($52.000).

## Página pública

`/` es la página para los dueños de mascotas: presenta la clínica, sus servicios y precios, y tiene el botón **Agendar cita**, que abre WhatsApp con el mensaje listo. No pide sesión y no consulta la base de datos (los precios salen de `CitaVeterinaria` y `Jaula` en el dominio).

- El panel del personal está en `/panel`; el enlace **Acceso personal** de la página lleva al inicio de sesión (con sesión iniciada dice **Ir al panel**).
- Teléfono, dirección, horario y mapa se configuran en `PetCareWeb/appsettings.json`, sección `Clinica`. `WhatsApp` va con indicativo y solo dígitos (por ejemplo `573001234567`). `MostrarOpiniones` se pone en `true` cuando haya testimonios reales.
- Las imágenes están en `PetCareWeb/wwwroot/img/landing/` (originales en `img/`). Si falta una, la página muestra un recuadro con su descripción.
- Nombres y especialidades del equipo están en `PetCareWeb/Components/Pages/Landing.razor`, marcados con [corchetes].

## Formularios

| Código | Pantalla | Ruta | Historias |
|---|---|---|---|
| — | Página pública | `/` | — |
| — | Panel del personal | `/panel` | — |
| F0 | Inicio de sesión | `/iniciar-sesion` | HU-01, HU-02 |
| F1 | Citas de consulta | `/citas` | HU-03, HU-04 |
| F2 | Hospitalizaciones | `/hospitalizaciones` | HU-05, HU-06 |
| F3 | Lotes de medicamento | `/lotes` | HU-07, HU-08 |
| F4 | Facturación | `/facturacion` y `/facturacion/{id}` | HU-09 a HU-14 |
| F5 | Consulta y detalle de facturas | `/facturas` y `/facturas/{id}` | HU-12, HU-13, HU-15 |
| F6 | Inventario | `/inventario` | HU-08, HU-18 |

## Ramas

| Rama | Qué contiene |
|---|---|
| `main` | Versión final de PetCare Web |
| `unidad-2-final` | El proyecto de consola tal como se entregó en la Unidad 2 |
| `antes-robustez` | Evidencia de desempeño: estado del formulario de citas antes del manejo de excepciones |
| `despues-robustez` | Evidencia de desempeño: el mismo formulario ya blindado |

Para correr el estado `antes-robustez` se necesita la migración que está en `main` (el esquema de la base de datos es el mismo en las dos ramas). Los pasos están en `Evidencia_U3/Guia_de_Ataques.md`.

## Documentos

- `DISENO.md`: decisiones de diseño (las seis preguntas del producto).
- `IA.md`: bitácora de uso de IA.
- `BaseDeDatos.sql`: el esquema del diagrama de base de datos. No hace falta ejecutarlo: la migración crea la misma base.
- `DatosDePrueba.sql`: datos de ejemplo (paso 5 de la instalación).
- `Evidencia_U3/`: informe de la evidencia de desempeño (`Informe_Evidencia.md` y su PDF), capturas de los ocho ataques antes y después, y guía para ejecutarlos.
- `videos.md`: enlaces a los vídeos de la Unidad 2 y de la Unidad 3.

El diagrama de clases y las historias de usuario de la Unidad 2 quedaron en la rama `unidad-2-final`.
