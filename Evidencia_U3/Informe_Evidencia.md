# Estructura del informe · PetCare Web a prueba de fallos

Evidencia de desempeño de la Unidad 3 (10 %). Máximo 10 páginas sin contar las capturas de los anexos. Esta es la estructura; los textos los escribe el equipo. Lo que está entre corchetes se reemplaza.

---

## Portada

- Institución Universitaria Pascual Bravo · Facultad de Ingeniería · Departamento de Sistemas Digitales
- Programación Orientada a Objetos (SD1007) · Grupo 053 · 2026-II
- Docente: Jorge Albeiro Muriel Vélez
- Título: PetCare Web a prueba de fallos
- Integrantes: Ader Quitana · Luis Toledo · Wilson Caraballo · Adel Ramos [agregar documentos]
- Repositorio: https://github.com/AdelRamos-coder/TallerPetCare (ramas `antes-robustez` y `despues-robustez`)
- Fecha: 19 de noviembre de 2026

---

## 1. Formulario elegido

F1 · Citas de consulta (`/citas`).

[Explicar en 3 a 5 líneas por qué lo eligieron. Datos que pueden usar: guarda en la tabla Servicios; tiene registro, cambio de duración, confirmación de veterinario y cambio de estado; la duración de 500 minutos del mensaje de la Dra. Ospina ocurre en este formulario.]

Archivos del formulario:

| Capa | Archivo |
|---|---|
| Interfaz | `PetCareWeb/Components/Pages/Citas.razor` y `Citas.razor.cs` |
| Validación de pantalla | `PetCareWeb/Modelos/ModeloCita.cs` |
| Servicio | `PetCareWeb/Servicios/ServicioCitas.cs` |
| Dominio | `PetCare.Dominio/CitaVeterinaria.cs`, `Servicio.cs`, `ReglaNegocioException.cs`, `CodigoDuplicadoException.cs` |
| Datos | `PetCareWeb/Datos/Configuraciones/ServicioConfiguracion.cs` y `CitaVeterinariaConfiguracion.cs` |

---

## 2. Síntesis de investigación (máximo 3 páginas)

Cada respuesta: palabras del equipo, un ejemplo corto de PetCare y la fuente citada. Mínimo tres fuentes y al menos dos de Microsoft Learn. La IA no cuenta como fuente.

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
Dónde mirar en el código: el set de `DuracionMinutos` en `CitaVeterinaria.cs`.
Fuente: [ ]

### P2. ¿Cómo funcionan try, catch, finally y throw? ¿Qué le pasa a una excepción que nadie atrapa y cómo viaja por la pila?

[Respuesta del equipo]

Ejemplo de PetCare: [ ]
Dónde mirar: `RegistrarAsync` en `Citas.razor.cs` (después) y el mismo método en la rama `antes-robustez`, donde no hay try.
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
Dónde mirar: `EsCodigoRepetido` en `ServicioCitas.cs` y los catch de DbUpdateException y SqlException en `Citas.razor.cs`.
Fuente: [ ]

### P6. ¿Qué es ILogger, qué niveles tiene y dónde se ven los mensajes en Visual Studio?

[Respuesta del equipo]

Ejemplo de PetCare: [ ]
Dónde mirar: `Logger.LogWarning` y `Logger.LogError` en `Citas.razor.cs`.
Fuente: [ ]

### P7. ¿Qué prácticas son incorrectas (atrapar y callar, atrapar todo, excepciones para el flujo normal) y por qué?

[Respuesta del equipo]

Ejemplo de PetCare: [ ]
Dónde mirar: `ConfirmarVeterinario` devuelve false en vez de lanzar cuando la cita ya tiene veterinario.
Fuente: [ ]

### P8. ¿Cómo se documenta con comentarios XML un método que lanza excepciones? ¿Para qué sirven summary y exception?

[Respuesta del equipo]

Ejemplo de PetCare: [ ]
Dónde mirar: los comentarios `///` de `RegistrarAsync` en `ServicioCitas.cs`.
Fuente: [ ]

---

## 3. Anexo A · Registro de pruebas antes y después

Los datos exactos de cada ataque están en `Guia_de_Ataques.md`. Capa: dominio (clases del modelo), datos (EF Core / SQL Server) o interfaz (componente Blazor). Las capturas van numeradas igual que el ataque (A1-antes, A1-después...).

| # | Datos de entrada exactos | ANTES: qué vio el usuario | ANTES: excepción y capa | DESPUÉS: qué vio el usuario | DESPUÉS: registro y BD |
|---|---|---|---|---|---|
| A1 | Todos los campos vacíos; luego tres espacios en Código, Mascota, Cédula y Motivo, sin fecha | | | | |
| A2 | Cita CIT-A01, duración 500; luego 0 y -10 | | | | |
| A3 | Código CIT-A01 (ya existe), Rocky, 1152447890, mañana 11:00, "Control de peso" | | | | |
| A4 | SQL Server detenido; registrar CIT-A04, Luna, 43876512, mañana 14:00, "Revisión de piel"; recargar la página | | | | |
| A5 | CIT-A01: confirmar "Dra. Lina Restrepo", luego "Dr. Andrés Zapata"; cancelar y luego marcar atendida | | | | |
| A6 | CIT-A06 con motivo de 300 caracteres; CIT-A06B con fecha 31/12/9999 10:00; CIT-A06C con fecha 01/01/0001 10:00 | | | | |
| A7 | CIT-A07, Kira, cédula "1.036.ABC", mañana 16:00, "Desparasitación" | | | | |
| A8 | CIT-A08, Manchas, 1001234567, mañana 16:30, "Vacunación anual", doble clic en Registrar cita | | | | |

### Justificación de A7 y A8

- A7: [por qué este ataque y qué categoría prueba]
- A8: [por qué este ataque y en qué se diferencia de la categoría de A7]

---

## 4. Fase 4 · Lo que decidimos no atrapar o no logramos resolver (máximo 120 palabras)

[Párrafo del equipo]

---

## 5. Anexo B · Bitácora de uso de IA

Mínimo tres entradas: creación, depuración y documentación. Al menos una con una respuesta corregida o rechazada y la razón técnica. Lo que esté aquí tiene que coincidir con `IA.md`.

### Entrada 1 · Creación

| Campo | Contenido |
|---|---|
| Herramienta y fecha | Claude · 14/11/2026 |
| Qué necesitábamos resolver | Blindar el formulario de citas: excepción propia, orden de los catch, mensajes distintos para duplicado y base caída, y que el formulario quede usable después de un error |
| Instrucción (prompt) tal como se escribió | [pegar el texto exacto que se le escribió] |
| Qué respondió la IA (resumen) | Propuso ReglaNegocioException y CodigoDuplicadoException, validar en CitaVeterinaria, revisar el código antes de guardar, traducir el rechazo de UQ_Servicios_Codigo y bloquear el doble clic con una bandera que se apaga en finally |
| Qué aceptamos, qué cambiamos y qué rechazamos, y por qué | En la primera versión ReglaNegocioException heredaba de InvalidOperationException. La cambiamos para que herede directamente de Exception: la evidencia lo pide (R2) y así una regla del negocio no se confunde con un error del sistema al atraparla. [agregar lo demás] |
| Cómo comprobamos que funcionaba | [qué ataques del Anexo A lo comprueban] |
| Integrante responsable | [ ] |

### Entrada 2 · Depuración

| Campo | Contenido |
|---|---|
| Herramienta y fecha | [ ] |
| Qué necesitábamos resolver | [ ] |
| Instrucción (prompt) tal como se escribió | [ ] |
| Qué respondió la IA (resumen) | [ ] |
| Qué aceptamos, qué cambiamos y qué rechazamos, y por qué | [ ] |
| Cómo comprobamos que funcionaba | [ ] |
| Integrante responsable | [ ] |

### Entrada 3 · Documentación

| Campo | Contenido |
|---|---|
| Herramienta y fecha | [ ] |
| Qué necesitábamos resolver | [ ] |
| Instrucción (prompt) tal como se escribió | [ ] |
| Qué respondió la IA (resumen) | [ ] |
| Qué aceptamos, qué cambiamos y qué rechazamos, y por qué | [ ] |
| Cómo comprobamos que funcionaba | [ ] |
| Integrante responsable | [ ] |

---

## 6. Anexo C · Reflexión ética individual

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
