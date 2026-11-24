# PetCare Web a prueba de fallos

Evidencia de desempeño de la Unidad 3 (10 %). Máximo 10 páginas sin contar las capturas de los anexos.

Las secciones marcadas **[Lo escribe el equipo]** son las que la evidencia exige escribir sin ayuda de IA: la síntesis de investigación (P1 a P8) y las reflexiones del Anexo C.

---

## Portada

- Institución Universitaria Pascual Bravo · Facultad de Ingeniería · Departamento de Sistemas Digitales
- Programación Orientada a Objetos (SD1007) · Grupo 053 · 2026-II
- Docente: Jorge Albeiro Muriel Vélez
- Título: PetCare Web a prueba de fallos
- Integrantes: Ader Quitana · Luis Toledo · Wilson Caraballo · Adel Ramos
- Documentos: [documento de cada integrante]
- Repositorio: https://github.com/AdelRamos-coder/TallerPetCare (ramas `antes-robustez` y `despues-robustez`)
- Fecha: 23 de noviembre de 2026

---

## 1. Formulario elegido

F1 · Citas de consulta (`/citas`).

Elegimos Citas porque es el formulario con más operaciones sobre la misma tabla: registrar, cambiar la duración, confirmar el veterinario y cambiar el estado, todo en la tabla Servicios. Ahí ocurrió el primer fallo que contó la Dra. Ospina, la duración de 500 minutos. Además tiene los tres tipos de dato que se pueden atacar: texto con largo máximo, un número con rango y una fecha.

Archivos del formulario:

| Capa | Archivo |
|---|---|
| Interfaz | `PetCareWeb/Components/Pages/Citas.razor` y `Citas.razor.cs` |
| Validación de pantalla | `PetCareWeb/Modelos/ModeloCita.cs` |
| Servicio | `PetCareWeb/Servicios/ServicioCitas.cs` |
| Dominio | `PetCare.Dominio/CitaVeterinaria.cs`, `Servicio.cs`, `ReglaNegocioException.cs`, `CodigoDuplicadoException.cs` |
| Datos | `PetCareWeb/Datos/Configuraciones/ServicioConfiguracion.cs` y `CitaVeterinariaConfiguracion.cs` |

---

## 2. Síntesis de investigación (máximo 3 páginas) · [Lo escribe el equipo]

Cada respuesta: palabras del equipo, un ejemplo corto de PetCare y la fuente citada. Mínimo tres fuentes y al menos dos de Microsoft Learn. La IA no cuenta como fuente y no se puede usar para redactar esta sección.

Fuentes de Microsoft Learn que pueden consultar (verifiquen y citen las que usen):

- Excepciones y control de excepciones en C#: https://learn.microsoft.com/dotnet/csharp/fundamentals/exceptions/
- Instrucciones try, catch, finally y throw: https://learn.microsoft.com/dotnet/csharp/language-reference/statements/exception-handling-statements
- Procedimientos recomendados para excepciones: https://learn.microsoft.com/dotnet/standard/exceptions/best-practices-for-exceptions
- Cómo crear excepciones definidas por el usuario: https://learn.microsoft.com/dotnet/standard/exceptions/how-to-create-user-defined-exceptions
- Control de errores en aplicaciones Blazor: https://learn.microsoft.com/aspnet/core/blazor/fundamentals/handle-errors
- Registro (logging) en ASP.NET Core: https://learn.microsoft.com/aspnet/core/fundamentals/logging/
- DbUpdateException (Entity Framework Core): https://learn.microsoft.com/dotnet/api/microsoft.entityframeworkcore.dbupdateexception
- Resistencia de conexión en EF Core: https://learn.microsoft.com/ef/core/miscellaneous/connection-resiliency
- Etiquetas XML recomendadas: https://learn.microsoft.com/dotnet/csharp/language-reference/xmldoc/recommended-tags
- Eventos y errores del motor de SQL Server (números 2601 y 2627): https://learn.microsoft.com/sql/relational-databases/errors-events/database-engine-events-and-errors

### P1. ¿Qué es una excepción en C#? ¿En qué se diferencia de un error de compilación y de un error de lógica?

[Respuesta del equipo]

Ejemplo de PetCare: [ ]
Dónde mirar en el código: el set de `DuracionMinutos` en `CitaVeterinaria.cs`. En el ataque A6 del estado antes, la fecha del año 9999 se guardó sin ninguna excepción: eso es un error de lógica.
Fuente: [ ]

### P2. ¿Cómo funcionan try, catch, finally y throw? ¿Qué le pasa a una excepción que nadie atrapa y cómo viaja por la pila?

[Respuesta del equipo]

Ejemplo de PetCare: [ ]
Dónde mirar: `RegistrarAsync` en `Citas.razor.cs` (después) y el mismo método en la rama `antes-robustez`, donde no hay try. En el ataque A2 del estado antes, la excepción subió hasta Blazor y la pantalla dejó de responder.
Fuente: [ ]

### P3. Jerarquía de excepciones de .NET. ¿Cuándo ArgumentException, ArgumentOutOfRangeException, InvalidOperationException o FormatException? ¿Por qué importa el orden de los catch?

[Respuesta del equipo]

Ejemplo de PetCare: [ ]
Dónde mirar: el orden de los catch en `RegistrarAsync` (CodigoDuplicadoException antes que ReglaNegocioException).
Fuente: [ ]

### P4. ¿Cómo y cuándo se crea una excepción propia? ¿Qué relación tiene con la herencia de la Unidad 2?

[Respuesta del equipo]

Ejemplo de PetCare: [ ]
Dónde mirar: `ReglaNegocioException.cs` y `CodigoDuplicadoException.cs`.
Fuente: [ ]

### P5. ¿Qué excepciones lanza EF Core con SQL Server al cargar o guardar? ¿Cómo se distingue un duplicado de un servidor que no responde?

[Respuesta del equipo]

Ejemplo de PetCare: [ ]
Dónde mirar: `EsCodigoRepetido` en `ServicioCitas.cs` y los catch de DbUpdateException y SqlException en `Citas.razor.cs`. En el estado antes, A3 dio SqlException número 2627 y A4 número 4060.
Fuente: [ ]

### P6. ¿Qué es ILogger, qué niveles tiene y dónde se ven los mensajes en Visual Studio?

[Respuesta del equipo]

Ejemplo de PetCare: [ ]
Dónde mirar: `Logger.LogWarning` y `Logger.LogError` en `Citas.razor.cs`. En A4 del estado después quedó `fail: PetCareWeb.Components.Pages.Citas[0] Sin conexión al registrar la cita CIT-A04`.
Fuente: [ ]

### P7. ¿Qué prácticas son incorrectas (atrapar y callar, atrapar todo, excepciones para el flujo normal) y por qué?

[Respuesta del equipo]

Ejemplo de PetCare: [ ]
Dónde mirar: `ConfirmarVeterinario` devuelve false en vez de lanzar cuando la cita ya tiene veterinario (ataque A5).
Fuente: [ ]

### P8. ¿Cómo se documenta con comentarios XML un método que lanza excepciones? ¿Para qué sirven summary y exception?

[Respuesta del equipo]

Ejemplo de PetCare: [ ]
Dónde mirar: los comentarios `///` de `RegistrarAsync` en `ServicioCitas.cs`.
Fuente: [ ]

---

## 3. Anexo A · Registro de pruebas antes y después

Los ocho ataques se ejecutaron el 23/11/2026 con los datos exactos de `Guia_de_Ataques.md`, iniciando sesión como `dgomez` (recepcionista). Cada estado se probó sobre una base nueva y vacía, para que lo que guardó un estado no cambiara el resultado del otro. "Mañana" es el 24/11/2026. Antes de los ataques se registró CIT-A01 (Firulais, 1036458721, mañana 10:00, "Control general").

Sobre A4: con LocalDB, la instancia de SQL Server se vuelve a encender sola en cuanto la aplicación se conecta, así que detenerla no deja la base caída. Por eso la base se puso fuera de línea con `ALTER DATABASE ... SET OFFLINE`, que produce la misma SqlException que un servidor apagado, y al terminar se volvió a poner en línea.

Capa: dominio (clases del modelo), datos (EF Core / SQL Server) o interfaz (componente Blazor). Las capturas están en `Evidencia_U3/capturas/antes` y `Evidencia_U3/capturas/despues`, con el nombre del ataque.

| # | Datos de entrada exactos | ANTES: qué vio el usuario | ANTES: excepción y capa | DESPUÉS: qué vio el usuario | DESPUÉS: registro y BD |
|---|---|---|---|---|---|
| A1 | Todos los campos vacíos; luego tres espacios en Código, Mascota, Cédula y Motivo, sin fecha | Debajo de cada campo: "El código es obligatorio.", "El nombre de la mascota es obligatorio.", "La cédula del cliente es obligatoria.", "La fecha de la cita es obligatoria." y "El motivo es obligatorio.". Con espacios, los mismos cinco mensajes. No se guardó nada | Ninguna. El ataque no rompió el formulario: lo frenó `[Required]` de `ModeloCita` (interfaz), que ya estaba en el estado antes y trata los espacios como campo vacío | Los mismos cinco mensajes; el formulario no se envía | Registro: nada, la petición no llega al servicio. BD: ninguna cita nueva |
| A2 | Cita CIT-A01, duración 500; luego 0 y -10 | Barra amarilla: "Ocurrió un error inesperado y la pantalla dejó de responder. Recargar". Lo mismo con 0 y con -10 | `ArgumentException` "La duración debe estar entre 15 y 180 minutos", lanzada por el set de `DuracionMinutos` en `CitaVeterinaria` (dominio). Nadie la atrapó y Blazor cerró el circuito (`CircuitHost[111] Unhandled exception in circuit`). BD: la duración siguió en 30 | Aviso "La clínica no permite ese cambio. La duración debe estar entre 15 y 180 minutos. La cita no cambió." para 500, 0 y -10. La pantalla sigue funcionando | `ReglaNegocioException` del dominio, atrapada en la página. Registro: nada, porque es una regla del negocio y no una falla técnica. BD: CIT-A01 sigue con 30 minutos |
| A3 | CIT-A01 (ya existe), Rocky, 1152447890, mañana 11:00, "Control de peso"; luego solo el código a CIT-A03 | Barra amarilla de error. La pantalla dejó de responder, así que no se pudo cambiar solo el código: había que recargar y volver a escribir todo | `DbUpdateException` con `SqlException` 2627: "Violation of UNIQUE KEY constraint 'UQ_Servicios_Codigo'" (datos). BD: el duplicado no entró; solo está la CIT-A01 original | Aviso "El código ya existe. Ya existe un registro con el código CIT-A01. Usa otro código." Los datos digitados se conservaron; al cambiar solo el código a CIT-A03 salió "La cita CIT-A03 de Rocky quedó en la lista." | `ServicioCitas` encontró el código antes de guardar y lanzó `CodigoDuplicadoException`. Registro: nada. BD: CIT-A03 guardada, sin duplicados |
| A4 | Base de datos fuera de línea; registrar CIT-A04, Luna, 43876512, mañana 14:00, "Revisión de piel"; recargar la página | Al registrar, barra amarilla de error. Al recargar, página técnica en inglés: "An unhandled exception occurred while processing the request. SqlException: Cannot open database ... The login failed." con todo el rastro de la pila | `SqlException` 4060 dentro de `InvalidOperationException` ("likely due to a transient failure") (datos). BD: nada guardado | "No se pudo registrar la cita. No pudimos comunicarnos con la base de datos. Los datos quedaron como estaban; intenta de nuevo en unos minutos." Los datos siguieron en el formulario. Al recargar: "No pudimos cargar las citas" con el mismo texto. Es distinto del mensaje de A3 | Registro: `fail: PetCareWeb.Components.Pages.Citas[0] Sin conexión al registrar la cita CIT-A04` con la SqlException (Error Number 4060) y, al recargar, `Sin conexión al cargar las citas`. BD: CIT-A04 no se guardó |
| A5 | CIT-A01: confirmar "Dra. Lina Restrepo", luego "Dr. Andrés Zapata"; cancelar y luego marcar atendida | "Cambio guardado. Dra. Lina Restrepo atenderá la cita CIT-A01." Luego "No se hizo ningún cambio. La cita ya tiene veterinario o ya no está programada. El veterinario se asigna una sola vez." Al cancelar, "La cita CIT-A01 quedó cancelada y ya no se puede facturar." Al marcar atendida, "No se hizo ningún cambio. Solo una cita programada se puede marcar como atendida." | Ninguna. El ataque no rompió el formulario: `ConfirmarVeterinario`, `MarcarCancelada` y `MarcarAtendida` devuelven false desde la Unidad 2 (dominio) y la página muestra el aviso. BD: veterinario Dra. Lina Restrepo, estado Cancelada | Los mismos cuatro mensajes | Registro: nada. BD: veterinario Dra. Lina Restrepo, estado Cancelada |
| A6 | CIT-A06 con motivo de 300 caracteres; CIT-A06B con fecha 31/12/9999 10:00; CIT-A06C con fecha 01/01/0001 10:00 | Con el motivo largo, barra amarilla de error. Con las dos fechas, "Cita registrada. La cita CIT-A06B de Toby quedó en la lista." y lo mismo con CIT-A06C | Motivo: `DbUpdateException` con `SqlException` "String or binary data would be truncated ... column 'Motivo'" (datos). Fechas: ninguna excepción; es un error de lógica porque nadie revisaba el rango. BD: CIT-A06B quedó con fecha 9999-12-31 y CIT-A06C con 0001-01-01 | Motivo: "El motivo admite máximo 150 caracteres." junto al campo. Fechas: "La cita no cumple una regla de la clínica. La fecha de la cita debe estar entre hoy y el 23/11/2027." Los datos se conservaron | El motivo lo frenó `StringLength` de `ModeloCita` (interfaz); `CitaVeterinaria` también revisa el largo. Las fechas las rechazó `CitaVeterinaria` con `ReglaNegocioException` (dominio). Registro: nada. BD: ninguna de las tres se guardó |
| A7 | CIT-A07, Kira, cédula "1.036.ABC", mañana 16:00, "Desparasitación" | "Cita registrada. La cita CIT-A07 de Kira quedó en la lista." | Ninguna excepción: la columna es texto y nadie revisaba el formato (error de lógica). BD: CIT-A07 guardada con la cédula 1.036.ABC | "La cédula solo puede tener números, sin puntos, espacios ni letras." junto al campo. Los datos se conservaron | Lo frenó `RegularExpression` de `ModeloCita` (interfaz); `CitaVeterinaria` también exige solo dígitos (dominio). Registro: nada. BD: CIT-A07 no se guardó |
| A8 | CIT-A08, Manchas, 1001234567, mañana 16:30, "Vacunación anual", doble clic en Registrar cita | "Cita registrada. La cita CIT-A08 de Manchas quedó en la lista." y al mismo tiempo la barra amarilla de error: la pantalla dejó de responder | El segundo clic intentó guardar la misma cita: `DbUpdateException` con `SqlException` 2627 por `UQ_Servicios_Codigo` (datos). BD: una sola CIT-A08, porque la restricción UNIQUE frenó la segunda | "Cita registrada. La cita CIT-A08 de Manchas quedó en la lista." sin ningún error | El botón se deshabilita mientras guarda (bandera `guardando`, que se apaga en `finally`), así que el segundo clic no hizo nada. Registro: nada. BD: una sola CIT-A08 |

Resumen: en el estado antes, cinco ataques tumbaron la pantalla (A2, A3, A4, el motivo largo de A6 y A8) y dos dejaron datos incorrectos guardados (las fechas de A6 y la cédula de A7). En el estado después, ninguno tumbó la pantalla, ninguno dejó datos incorrectos y el duplicado (A3) y la base caída (A4) muestran mensajes distintos.

### Justificación de A7 y A8

- **A7 (cédula con letras y puntos):** prueba el formato de un dato que la base de datos sí acepta, porque la columna CedulaCliente es texto. Si la aplicación no lo revisa, nadie lo detiene: no hay excepción, la cita se guarda con un dato inútil y el error aparece después, al buscar o facturar por cédula. Es un error de lógica, no de infraestructura.
- **A8 (doble clic en Registrar cita):** prueba el estado de la interfaz mientras se guarda, no un dato. Los datos son válidos; lo que falla es que el usuario hace dos veces la misma acción antes de que termine la primera. Por eso se resuelve con el botón deshabilitado y `finally`, y no con validaciones del dominio.

---

## 4. Fase 4 · Lo que decidimos no atrapar o no logramos resolver (máximo 120 palabras)

No atrapamos la excepción de `Database.Migrate()` en `Program.cs`. Si SQL Server no está disponible cuando la aplicación arranca, Migrate lanza una SqlException y la aplicación no inicia. Atraparla no arreglaría nada: sin base de datos ningún formulario puede leer ni guardar, y la aplicación abriría pantallas que solo mostrarían avisos de error. Es mejor que falle al arrancar, con el detalle en la consola, donde lo ve quien la ejecuta. En cambio, si la base se cae con la aplicación abierta (A4), sí se atrapa en el formulario, porque ahí el usuario puede esperar e intentar de nuevo sin perder lo que escribió.

---

## 5. Anexo B · Bitácora de uso de IA

Mínimo tres entradas: creación, depuración y documentación. Lo que está aquí coincide con `IA.md`.

### Entrada 1 · Creación

| Campo | Contenido |
|---|---|
| Herramienta y fecha | Claude Code (Claude Opus 5.5) · 14/11/2026 |
| Qué necesitábamos resolver | Construir la página pública de la clínica (el diseño 11 que elegimos) dentro de la misma aplicación, sin tocar los formularios del personal |
| Instrucción (prompt) tal como se escribió | "you can start, and act as a architecture engineer senior, with the architecture without forcing nothing, and with good animations, smooothly" |
| Qué respondió la IA (resumen) | Escribió un plan y lo implementó: la página en /, el panel del personal movido a /panel, los datos de contacto en appsettings.json (sección Clinica), los precios tomados del dominio, las imágenes optimizadas y animaciones solo con CSS |
| Qué aceptamos, qué cambiamos y qué rechazamos, y por qué | El plan decía leer las tarifas de las jaulas desde la base de datos. Lo cambiamos para leerlas del dominio (Jaula.TarifaDiaPara), porque ya estaban ahí y así la página pública no depende de la base. Al revisarla en el navegador aparecieron dos errores de la IA: la regla de color de los enlaces dejaba los botones sin texto visible y la barra rápida tapaba el botón de agendar. Los dos se corrigieron |
| Cómo comprobamos que funcionaba | Se revisó en el navegador a ancho de escritorio y de celular, se probó el paso al inicio de sesión del personal y se abrió la página con la base de datos apagada para confirmar que no depende de ella |
| Integrante responsable | Adel Ramos |

### Entrada 2 · Depuración

| Campo | Contenido |
|---|---|
| Herramienta y fecha | Claude Code (Claude Opus 5.5) · 14/11/2026 |
| Qué necesitábamos resolver | Cargar datos de ejemplo en la base para probar todas las pantallas |
| Instrucción (prompt) tal como se escribió | "lets put some dataset examples to test it with information" |
| Qué respondió la IA (resumen) | Leyó el esquema, los CHECK y las reglas de precio del dominio y escribió DatosDePrueba.sql con 9 citas, 6 hospitalizaciones, 8 lotes y 5 facturas |
| Qué aceptamos, qué cambiamos y qué rechazamos, y por qué | El script falló dos veces al cargarlo y hubo que depurarlo. Faltaba SET QUOTED_IDENTIFIER ON, que SQL Server exige para escribir en una tabla con un índice filtrado (UX_Servicios_JaulaOcupada). Las fechas sin segundos no se leían con el idioma del servidor, así que se pasaron a formato ISO completo. Además chocaba con el lote L-205 que ya habíamos creado, y se renumeró a L-301 a L-308 |
| Cómo comprobamos que funcionaba | El script terminó de cargar sin errores en la base PetCareWeb con sqlcmd. Quedó en la raíz del repositorio y el README explica cómo cargarlo |
| Integrante responsable | Adel Ramos |

### Entrada 3 · Documentación

| Campo | Contenido |
|---|---|
| Herramienta y fecha | Claude Code (Claude Opus 5.5) · 23/11/2026 |
| Qué necesitábamos resolver | Que las clases y los miembros públicos tuvieran comentarios XML (///), como pide el producto, y revisar que los métodos que lanzan excepciones tuvieran su etiqueta exception (R8) |
| Instrucción (prompt) tal como se escribió | "puedes clonar el repositorio, y actualizar todo, que todo quede completo?" |
| Qué respondió la IA (resumen) | Compiló con la advertencia CS1591 activada, encontró 78 miembros públicos sin ///, los documentó en 40 archivos sin cambiar el código y revisó que 47 de los 48 lanzamientos de excepciones tuvieran su etiqueta exception |
| Qué aceptamos, qué cambiamos y qué rechazamos, y por qué | Aceptamos los comentarios. La etiqueta que falta (InvalidOperationException en DarDeAltaAsync, del formulario de hospitalizaciones) no se agregó porque decidimos no tocar más código antes de la sustentación; no es del formulario de esta evidencia |
| Cómo comprobamos que funcionaba | Volvimos a compilar con CS1591 activada: 0 errores y 0 advertencias de comentarios faltantes |
| Integrante responsable | Adel Ramos |

### Entrada 4 · Creación (manejo de excepciones de esta evidencia)

| Campo | Contenido |
|---|---|
| Herramienta y fecha | Claude · 14/11/2026 |
| Qué necesitábamos resolver | Blindar el formulario de citas: excepción propia, orden de los catch, mensajes distintos para duplicado y base caída, y que el formulario quede usable después de un error |
| Instrucción (prompt) tal como se escribió | No guardamos el texto exacto de esta petición: fue en un chat de Claude del 14/11 y no lo copiamos en su momento. Por eso las tres entradas anteriores son las que tienen la instrucción literal |
| Qué respondió la IA (resumen) | Propuso ReglaNegocioException y CodigoDuplicadoException, validar en CitaVeterinaria, revisar el código antes de guardar, traducir el rechazo de UQ_Servicios_Codigo y bloquear el doble clic con una bandera que se apaga en finally |
| Qué aceptamos, qué cambiamos y qué rechazamos, y por qué | Aceptamos las dos excepciones, la revisión del código antes de guardar y la bandera con finally. En la primera versión ReglaNegocioException heredaba de InvalidOperationException. La cambiamos para que herede directamente de Exception: la evidencia lo pide (R2) y así una regla del negocio no se confunde con un error del sistema al atraparla. También ajustamos los catch de Facturación e Inventario |
| Cómo comprobamos que funcionaba | Con los ocho ataques del Anexo A en las ramas antes-robustez y despues-robustez. En el estado después ningún ataque tumbó la pantalla, A3 y A4 dan mensajes distintos y el registro muestra "Sin conexión al registrar la cita CIT-A04" |
| Integrante responsable | Adel Ramos |

---

## 6. Anexo C · Reflexión ética individual · [Lo escribe cada integrante]

Entre 150 y 200 palabras por integrante, escritas sin ayuda de IA, en un solo texto que responda:

1. Si la IA propuso un bloque que atrapaba el error y lo ocultaba, y por eso se factura un medicamento vencido, ¿de quién es la responsabilidad?
2. ¿Qué parte del código de esta evidencia entiende menos y qué haría para poder responder por ella?
3. ¿Qué información de un proyecto real no se debería pegar en un asistente de IA y por qué?

### Ader Quitana

[Texto]

Documento: ______ · Firma: ______

### Luis Toledo

[Texto]

Documento: ______ · Firma: ______

### Wilson Caraballo

[Texto]

Documento: ______ · Firma: ______

### Adel Ramos

[Texto]

Documento: ______ · Firma: ______

---

## Anexo A · Capturas

Cada captura lleva el número del ataque y el estado. Las demás capturas de cada paso están en `Evidencia_U3/capturas`.

### A1

![A1 antes](capturas/antes/A1-2-espacios.png)
*A1 antes: campos con espacios.*

![A1 después](capturas/despues/A1-2-espacios.png)
*A1 después: campos con espacios.*

### A2

![A2 antes](capturas/antes/A2-duracion-500.png)
*A2 antes: duración 500.*

![A2 después](capturas/despues/A2-duracion-500.png)
*A2 después: duración 500.*

### A3

![A3 antes](capturas/antes/A3-1-duplicado.png)
*A3 antes: código CIT-A01 repetido.*

![A3 después](capturas/despues/A3-1-duplicado.png)
*A3 después: código CIT-A01 repetido.*

![A3 después corregido](capturas/despues/A3-2-corrige-codigo.png)
*A3 después: solo se cambió el código a CIT-A03.*

### A4

![A4 antes registrar](capturas/antes/A4-1-registrar-sin-bd.png)
*A4 antes: registrar con la base fuera de línea.*

![A4 antes recargar](capturas/antes/A4-2-recargar-sin-bd.png)
*A4 antes: recargar la página con la base fuera de línea.*

![A4 después registrar](capturas/despues/A4-1-registrar-sin-bd.png)
*A4 después: registrar con la base fuera de línea.*

![A4 después recargar](capturas/despues/A4-2-recargar-sin-bd.png)
*A4 después: recargar la página con la base fuera de línea.*

### A5

![A5 antes](capturas/antes/A5-2-confirma-zapata.png)
*A5 antes: segundo veterinario.*

![A5 antes atendida](capturas/antes/A5-4-marcar-atendida.png)
*A5 antes: marcar atendida una cita cancelada.*

![A5 después](capturas/despues/A5-2-confirma-zapata.png)
*A5 después: segundo veterinario.*

![A5 después atendida](capturas/despues/A5-4-marcar-atendida.png)
*A5 después: marcar atendida una cita cancelada.*

### A6

![A6 antes motivo](capturas/antes/A6-1-motivo-300.png)
*A6 antes: motivo de 300 caracteres.*

![A6 antes fecha](capturas/antes/A6-2-fecha-9999.png)
*A6 antes: fecha 31/12/9999.*

![A6 después motivo](capturas/despues/A6-1-motivo-300.png)
*A6 después: motivo de 300 caracteres.*

![A6 después fecha](capturas/despues/A6-2-fecha-9999.png)
*A6 después: fecha 31/12/9999.*

### A7

![A7 antes](capturas/antes/A7-cedula-letras.png)
*A7 antes: cédula 1.036.ABC.*

![A7 después](capturas/despues/A7-cedula-letras.png)
*A7 después: cédula 1.036.ABC.*

### A8

![A8 antes](capturas/antes/A8-doble-clic.png)
*A8 antes: doble clic en Registrar cita.*

![A8 después](capturas/despues/A8-doble-clic.png)
*A8 después: doble clic en Registrar cita.*
