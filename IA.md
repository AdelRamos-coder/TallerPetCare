# Bitácora de uso de IA 

Programación Orientada a Objetos · Unidad 2 · Producto PetCare 2.0

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

## 4. Lo que aprendimos

Escriban aquí, con sus palabras, 3 o 4 cosas concretas que aprendió el equipo al usar la IA. Por ejemplo: qué sugerencia de la IA resultó incorrecta o incompleta y cómo se dieron cuenta, o en qué parte fue más útil revisar el código por su cuenta que confiar en la respuesta.

## 5. Lista de chequeo antes de entregar

- Cada integrante registró sus propios usos de IA, no solo uno.
- Ninguna celda de la última columna dice solo "todo bien".
- Se registraron también las cosas que la IA dijo mal y el equipo corrigió.
- Todo lo que está en la tabla es verdad: no se agregan usos que no ocurrieron ni se omiten los que sí.
- Cada integrante puede explicar cualquier fragmento del código que esté en la tabla.
