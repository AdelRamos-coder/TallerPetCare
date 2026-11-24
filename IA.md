# Bitácora de uso de IA 

Programación Orientada a Objetos · Unidad 2 (PetCare 2.0) y Unidad 3 (PetCare Web)

Integrantes: Ader, Adel, Wilson y Luis

## 1. Para qué sirve este documento

El enunciado permite usar asistentes de IA como apoyo para el modelado, la depuración y la documentación, pero pide registrar cada uso. La IA es una herramienta de apoyo, no la autora del trabajo: el equipo responde por cada línea de código y cada relación del diagrama.

Una bitácora vacía o genérica se califica como insuficiente. Por eso la última columna (qué se corrigió o descartó y por qué) tiene que decir cosas concretas.

## 2. Cómo se llena la tabla

Cada vez que alguien usó una IA, agrega una fila con estas columnas:

- **Fecha:** el día en que se hizo la consulta.
- **Quién:** el integrante que la hizo.
- **Herramienta:** nombre de la IA (por ejemplo, Claude).
- **Qué le pedimos:** la petición concreta, no "ayuda con el taller".
- **Qué usamos:** la parte de la respuesta que sí entró al proyecto.
- **Qué corregimos o no usamos y por qué:** lo que se cambió, se descartó o se encontró incorrecto, con el motivo.

**Ejemplo malo (genérico):** "Se usó para mejorar el código" / "Todo estuvo bien".

**Ejemplo bueno (concreto):** "Sugirió borrar las clases Jaula y Medicamento. Jaula la mantuvimos porque viene de la Unidad 1 y la conectamos con Hospitalizacion. Medicamento no lo borramos: lo unimos con LoteMedicamento para no perder el inventario de la Unidad 1."

## 3. Tabla de registro

| Fecha | Quién | Herramienta | Qué le pedimos | Qué usamos | Qué corregimos o no usamos y por qué |
|---|---|---|---|---|---|
| 14/10/2026 | Luis | Claude | Revisar el código que entregaron los compañeros contra el PDF del enunciado | El hallazgo de que RenglonFactura solo guardaba texto y no el objeto facturado; se le agregó la propiedad Item de tipo IFacturable para que exista la agregación | [COMPLETAR: qué otros hallazgos se descartaron o cambiaron] |
| 14/10/2026 | Wilson | Claude | Lista de correcciones, tomando como error lo que no estaba en el PDF (Jaula, Medicamento) | La idea de que las clases que se mantienen deben justificarse en DISENO.md y dibujarse en el diagrama | La sugerencia de eliminar Jaula y Medicamento no se aplicó tal cual: Jaula la mantuvimos a propósito y la conectamos con Hospitalizacion, y Medicamento lo unimos con LoteMedicamento en vez de borrarlo. Claude se corrigió al reconocer que el PDF no prohíbe clases adicionales |
| 14/10/2026 | Luis | Claude | Explicación de cómo cambiar el constructor de RenglonFactura, paso a paso | El cambio completo en RenglonFactura.cs y en Factura.cs | [COMPLETAR: si hubo algún ajuste propio al aplicarlo] |
| 14/10/2026 | Ader | Claude | Estructura del método de demostración en Main (pasos 1 a 3) y corrección de un error de DateOnly | La estructura del método DemostracionPetCare | [COMPLETAR: qué valores y nombres se cambiaron] |
| 14/10/2026 | Luis | Claude | Revisar el Main ya terminado contra los 8 pasos del PDF | Los arreglos del polimorfismo (foreach sobre List&lt;IFacturable&gt;) y de los pasos 6 y 7 con try/catch, porque el Main escribía los mensajes en lugar de que el sistema lanzara los errores | No se aplicó el cambio de tildes en las descripciones (decisión del equipo) |
| 14/10/2026 | Ader | Claude | Borrador del diagrama de clases en Mermaid | [COMPLETAR: si se usó como base o solo como guía] | [COMPLETAR: qué se corrigió al pasarlo a draw.io] |
| 14/10/2026 | Luis | Claude | Borrador de historias de usuario, guion del vídeo y guía de estudio | [COMPLETAR: qué parte se usó] | Los datos del borrador se actualizaron porque el código cambió (Jaula conectada a Hospitalizacion, Medicamento fusionado con LoteMedicamento) |
| 13/10/2026 | Adel | Claude | Si LoteMedicamento debía heredar de Servicio o cumplir una interfaz para poder facturarse | Una interfaz (IFacturable) que cumplen Servicio y LoteMedicamento | No usamos la herencia: un medicamento no es un servicio, no tiene mascota ni fecha de atención, y la regla de los servicios de cobrarse siempre como 1 unidad no le sirve a algo que se vende por cantidad |
| 13/10/2026 | Adel | ChatGPT y Claude | Cómo controlar la cantidad que se puede facturar de cada cosa | Que cada clase valide su cantidad con ValidarFacturacion(cantidad) | ChatGPT proponía un bool PermiteCantidad. No lo usamos porque la factura tendría que preguntar con un if qué está cobrando; con el método, cada clase responde por sí misma y la factura no cambia cuando llegue algo nuevo |
| 14/10/2026 | Adel | Claude | Si la relación entre Hospitalizacion y Jaula era composición o agregación | Agregación: la hospitalización recibe la jaula ya creada, la ocupa al ingresar y la libera al dar de alta | No usamos composición porque la jaula existe antes de que llegue el animal y sigue ahí cuando se va. Si la hospitalización la creara, cada animal tendría una jaula nueva, y la clínica tiene jaulas fijas |
| 14/10/2026 | Adel | Claude | Cómo quitar el nombre de la mascota, que estaba repetido en Jaula y en Servicio | Que la jaula guarde a su ocupante (la hospitalización) y le pregunte el nombre, los días y la cuenta | Nosotros propusimos que Jaula heredara de Servicio, pero lo descartamos: una jaula no es un servicio, y si heredara se podría meter en la factura como si se cobrara la jaula |

### Unidad 3 · PetCare Web

| Fecha | Quién | Herramienta | Qué le pedimos | Qué usamos | Qué corregimos o no usamos y por qué |
|---|---|---|---|---|---|
| 29/10/2026 | Adel | Claude | Ayuda con el script SQL que desarrollamos a partir del diagrama de base de datos | El script BaseDeDatos.sql con tablas, llaves, UNIQUE, CHECK e índices | Le pedimos quitar los INSERT y el DROP TABLE: el script tenía que servir para crear la base por primera vez, y los datos iniciales los carga la migración |
| 29/10/2026 | Adel | Claude y Figma | Cinco propuestas de pantallas para los formularios F0 a F6 | El diseño 4, "Expediente clínico", como base de las pantallas. La maqueta en Figma la hicimos nosotros y se la pasamos a Claude | La primera entrega no eran cinco diseños distintos sino variaciones del mismo; se pidió rehacerlos. De la maqueta quitamos el buscador Ctrl K y el botón Imprimir porque ninguna historia de usuario los pide |
| 14/11/2026 | Adel | Claude | Proponer la arquitectura de la Unidad 3 (proyectos, carpetas, clases nuevas) antes de escribir código | La separación en PetCare.Dominio y PetCareWeb, un servicio por formulario y una configuración por tabla | Propuso renombrar las clases a los nombres del diagrama (ServicioVeterinario, LineaFactura, AgregarLinea). No lo aceptamos: mantuvimos los nombres de la Unidad 2 y la equivalencia quedó explicada en DISENO.md. También descartamos una propiedad alias para Nombre porque no hacía falta |
| 14/11/2026 | Adel | Claude | Decidir si la tarifa diaria de la hospitalización se escribe en el formulario o sale de la jaula | Que la tarifa salga del tamaño de la jaula y quede fija al ingresar, como en la Unidad 2 | La historia HU-05 habla de escribir la tarifa, pero la jaula ya la define; dejarla editable permitía tarifas distintas para la misma jaula. En la pantalla se muestra la tarifa sin poder cambiarla |
| 14/11/2026 | Adel | Claude | Si al facturar un lote se debía descontar el stock | Nada: el stock no se descuenta al facturar | La IA preguntó si descontar; decidimos que no porque ninguna historia lo pide y al anular habría que devolver las unidades. La factura solo revisa que la cantidad no supere el stock |
| 14/11/2026 | Adel | Claude | Código de PetCare Web: contexto de EF Core, configuraciones, servicios, páginas F0 a F6 y estilos | La base del proyecto que está en la rama main | La IA no pudo compilar ni generar la migración en su entorno. Al compilarlo nosotros no salieron errores; la migración InicialPetCare se generó y se aplicó el 14/11 (ver la fila "Desplegar el proyecto") |
| 14/11/2026 | Adel | Claude | Manejo de excepciones del formulario de citas para la evidencia de desempeño (estado antes y después) | ReglaNegocioException, CodigoDuplicadoException, el orden de los catch y el bloqueo del doble clic | La primera versión hacía que ReglaNegocioException heredara de InvalidOperationException; la cambiamos para que herede directamente de Exception, como pide la evidencia, y ajustamos los catch de Facturación e Inventario. [COMPLETAR: resultados de los ataques] |
| 14/11/2026 | Adel | Claude | Revisar el dominio de la Unidad 2 contra las historias de la Unidad 3 | Las descripciones con tildes ("Hospitalización – Luna – 3 días") | En la Unidad 2 decidimos no usar tildes; en la Unidad 3 las agregamos porque HU-12 CA2 muestra el texto exacto con tildes |
| 14/11/2026 | Adel | Claude | Desplegar el proyecto | Despliegue local con LocalDB, la migración InicialPetCare y la base creada | Ofreció Azure y Docker; elegimos local porque es gratis y es lo que describe el README. La primera actualización de la base no aplicó la migración (usó --no-build); la IA lo detectó y la repitió compilando |
| 14/11/2026 | Adel | Claude | Datos de ejemplo para probar todas las pantallas | DatosDePrueba.sql: citas, hospitalizaciones, lotes y facturas con precios calculados con las reglas del dominio | El script falló dos veces: faltaba QUOTED_IDENTIFIER y las fechas no se leían con el idioma del servidor. También chocaba con el lote L-205 que ya habíamos creado, así que se renumeró a L-301…L-308 |
| 14/11/2026 | Adel | Claude | Explicar la facturación y el campo "Número" | La explicación del flujo y una tabla de reglas para probar | Solo fue una explicación; no cambió el código |
| 14/11/2026 | Adel | Claude | Si el sistema está pensado para clientes | La conclusión, sacada de los documentos, de que es solo para el personal; el cliente solo recibe la factura | Solo fue una consulta; por esa conclusión la página para los clientes se hizo aparte, sin tocar los formularios del personal |
| 14/11/2026 | Adel | Claude | 10 diseños de página pública usando Tezka y Vital como referencia | La idea de agendar por WhatsApp y de poner la página pública en / con el panel en /panel | Descartamos un formulario de solicitudes y la reserva en línea porque obligaban a crear tablas, pantallas y cuentas de cliente. De la primera ronda solo nos gustaron el 03 y el 10 |
| 14/11/2026 | Adel | Claude | 7 diseños más, tranquilos y cómodos de usar | El diseño 11 «Sala de espera» | Los diseños 12 a 17 no se eligieron |
| 14/11/2026 | Adel | Claude y ChatGPT | Prompts detallados para generar las imágenes | Las 10 imágenes generadas en ChatGPT (carpeta img) | Claude las redujo de ~20 MB a ~950 KB para que la página cargue rápido en el celular. La imagen IMG-02 no se generó porque el diseño 11 no la usa |
| 14/11/2026 | Adel | Claude | Implementar el diseño 11 con buena arquitectura y animaciones suaves | La página Landing.razor, landing.css, OpcionesClinica, ImagenLanding, el panel movido a /panel y animaciones solo con CSS | El plan decía leer los precios de las jaulas desde la base; se cambió para leerlos del dominio (Jaula.TarifaDiaPara) porque ya estaban ahí y así la página no depende de la base. Al revisar en el navegador aparecieron dos errores de la IA: el color de los enlaces dejaba los botones sin texto visible y la barra rápida tapaba el botón de agendar. Ambos se corrigieron |
| 23/11/2026 | Adel | Claude | Revisar el proyecto contra el enunciado y contra GitHub, y subir lo que faltaba | La lista de lo que no estaba en GitHub (migración, página pública, datos de prueba), el ajuste del README, los comentarios /// que faltaban en 78 miembros públicos, las filas nuevas de la tabla de equivalencias de DISENO.md y un borrador de la sección 4 de esta bitácora hecho con los casos de la tabla | La cuenta de GitHub activa en el equipo no era la del repositorio; le pedimos no subir nada con esa cuenta y cambiamos a AdelRamos-coder antes del push. No subimos las notas de la sesión ni los documentos del profesor porque no hacen parte de la entrega |
| 23/11/2026 | Adel | Claude | Completar los datos de la página pública que estaban entre corchetes | Datos de ejemplo: el equipo con personas del caso de estudio (Dra. Marcela Ospina, Dr. Andrés Zapata, Lina Restrepo), barrio Robledo en Medellín, horario de lunes a sábado y el número de WhatsApp de ejemplo | PetCare es una clínica ficticia, así que no hay datos reales. Dejamos los testimonios ocultos (MostrarOpiniones en false) porque inventar opiniones de clientes no es correcto, y agregamos en el pie que es un proyecto académico con datos de contacto de ejemplo |

## 4. Lo que aprendimos

- **La IA propone cambios grandes con mucha seguridad.** Quiso renombrar todas las clases a los nombres del diagrama. Si lo aceptábamos, el proyecto dejaba de ser la continuación de la Unidad 2 y teníamos que volver a explicar todo; nos quedamos con nuestros nombres y dejamos la equivalencia en DISENO.md.
- **Hay que comparar sus respuestas con el enunciado.** ReglaNegocioException heredaba de InvalidOperationException y funcionaba, pero la evidencia pide que herede de Exception (R2). Lo vimos al revisar los requisitos uno por uno, no al probar.
- **Que el código compile no quiere decir que funcione.** La migración no se aplicó la primera vez, el script de datos falló dos veces y los botones de la página pública quedaron sin texto visible. Todo eso apareció al ejecutar y al abrir la página, no leyendo la respuesta.
- **Hay decisiones que no se le dejan a la IA.** Antes de subir a GitHub revisamos con qué cuenta se iba a hacer el push, porque de eso depende a nombre de quién quedan los commits.

## 5. Lista de chequeo antes de entregar

- Cada integrante registró sus propios usos de IA, no solo uno.
- Ninguna celda de la última columna dice solo "todo bien".
- Se registraron también las cosas que la IA dijo mal y el equipo corrigió.
- Todo lo que está en la tabla es verdad: no se agregan usos que no ocurrieron ni se omiten los que sí.
- Cada integrante puede explicar cualquier fragmento del código que esté en la tabla.
