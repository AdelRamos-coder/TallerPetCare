# Bitácora de uso de IA

Usamos Claude y ChatGPT para revisar el código, buscar errores y pedir ideas. No todo lo que nos dijeron lo dejamos, abajo está lo que usamos y lo que cambiamos.

| Fecha | Quién | Herramienta | Qué le pedimos | Qué usamos | Qué corregimos o no usamos y por qué |
|---|---|---|---|---|---|
| 13/10 | Adel | Claude | Hacer LoteMedicamento y una interfaz para cobrar servicios y medicamentos en la misma factura | La interfaz IFacturable y la regla de no vender lotes vencidos | Le quitamos los comentarios para ponerlos nosotros. Luego le agregamos la cantidad a ValidarFacturacion porque se podía cobrar una consulta como 3 unidades |
| 13/10 | Adel | ChatGPT | Que nos propusiera el diagrama de clases | La idea de la clase abstracta para los servicios, la interfaz y la factura con renglones | El diagrama no tenía agregación ni multiplicidades y el profe las pide. Usaba un bool para saber si se podía poner cantidad y eso obligaba a la factura a usar if. También decía que al anular se borraban los renglones y la directora no dijo eso |
| 13/10 | Adel | Claude | Revisar el código de la unidad 1 que teníamos en el repo | Arreglos: no despachar medicamentos vencidos ni 0 unidades, validar los datos del constructor | El menú daba resultados distintos según el orden en que uno entrara a las opciones, eso lo arreglamos también |
| 13/10 | Adel | Claude | Revisar lo que subió Ader | Que Servicio implementara IFacturable | Lo demás de Ader se dejó igual, solo faltaba conectarlo con la interfaz para que la factura pudiera cobrar consultas |
| 14/10 | Adel | Claude | Revisar todo el repo con la guía del profe | Nos dijo que en la demo los pasos 6 y 7 solo imprimían un mensaje y nunca llamaban a Agregar | Luis lo cambió con try/catch para que sea la factura la que no deje agregar |
| 14/10 | Adel | Claude | Qué hacer con Medicamento y Jaula de la unidad 1 | Juntamos Medicamento con LoteMedicamento (stock, tipo, nevera) | La IA dijo que borráramos Jaula pero no le hicimos caso porque el trabajo tiene que tener relación con la unidad 1. La dejamos como el lugar donde está el animal y Hospitalizacion la usa |
| 14/10 | Adel | Claude | Quitar datos repetidos entre Jaula y Hospitalizacion | Que la jaula guarde quién la está ocupando en vez de copiar el nombre de la mascota | Esto lo notamos nosotros. Pensamos que Jaula podía heredar de Servicio pero no, una jaula no es un servicio y se iba a poder meter en la factura |
| 14/10 | Adel | Claude | Revisar que la hospitalización se cobrara según el tamaño de la jaula | Una tabla con la tarifa de cada tamaño | Antes la tarifa se escribía a mano y una jaula grande podía salir más barata que una pequeña |
| 14/10 | Adel | Claude | Recuperar lo que se perdió de la cita de la unidad 1 | La cédula, el motivo, los estados de la cita y los días que faltan | Agregamos que una cita cancelada no se pueda facturar |
| 14/10 | Adel | Claude | Hacer el diagrama en draw.io desde el código | El archivo de draw.io y el png | Lo revisamos clase por clase contra el código |
| 14/10 | Adel | Claude | Revisar el diseno.md | Nos marcó errores, por ejemplo que en la unidad 1 no teníamos factura | Dejamos el texto con nuestras palabras |
|  | Ader |  |  |  |  |
|  | Wilson |  |  |  |  |
|  | Luis |  |  |  |  |

## Lo que aprendimos

