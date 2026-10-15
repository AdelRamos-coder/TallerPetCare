1. Clase abstracta
Creamos la clase abstracta Servicio. A ella subimos lo que la cita y la hospitalización tienen en común: el código, el nombre de la mascota y la fecha. Estos tres datos no cambian después de registrados. También subimos las validaciones del constructor (que es protected) y el método virtual ObtenerDescripcion(), que por defecto devuelve el nombre de la mascota. Lo único que dejamos abstracto es CalcularCosto(), porque es justo lo que cambia: la consulta se cobra por minutos y la hospitalización por días  con la tarifa de la jaula según su tamano
Elegimos esto y no otra cosa porque la directora dijo que los dos servicios "se parecen mucho" pero "se cobran distinto". Lo común sube y lo distinto queda abstracto, así no repetimos código en CitaVeterinaria ni en Hospitalizacion.
Cambios frente a la Unidad 1: FacturaServicio se convirtió en Factura y RenglonFactura, porque la factura ahora lleva varios renglones. Aparecieron Servicio e IFacturable.
Se conservaron del entregable anterior Jaula y TipoMedicamento para mantener relación con las nuevas clases como hospitalización y Tipomedicamiento

2. Interfaz
Usamos la interfaz IFacturable porque LoteMedicamento no es un servicio, es un producto. La directora lo dijo: "no se le presta a nadie". No tiene mascota ni una fecha de servicio, así que no tiene sentido que herede de Servicio. Además, en C# una clase solo puede heredar de una.
La interfaz define lo que necesita la factura de cualquier cosa que cobre: código, descripción, precio unitario y validación. La cumplen Servicio y LoteMedicamento, que están en jerarquías distintas.

3. Composición y agregación
La composición está entre Factura y RenglonFactura. La factura crea los renglones dentro de Agregar, los guarda en una lista privada y por fuera solo se ven como IReadOnlyList. El constructor del renglón es internal para que no se creen renglones sueltos.
La agregación está entre RenglonFactura e IFacturable. El renglón recibe la consulta, la hospitalización o el lote ya creados y solo los guarda, no los crea.
Cuando se anula una factura, queda marcada como anulada y no admite más renglones. Los renglones ya no sirven. Pero la consulta, la hospitalización y el lote no se tocan, porque el renglón solo los apuntaba. Por eso se pueden facturar de nuevo en otra factura.

4. Polimorfismo dinámico
El polimorfismo dinámico ocurre en la línea [67] de Program.cs dentro del método DemostracionPetCare:
Console.WriteLine($"- {item.ObtenerDescripcion()} | precio unitario: {item.ObtenerPrecioUnitario():C}");
Esa línea está dentro de un foreach que recorre una List<IFacturable> con la consulta, la hospitalización y el lote. La variable item está declarada con el tipo de la interfaz, así que el programa no sabe de qué clase es cada objeto. Quien decide qué método se ejecuta es el objeto real, cuando el programa corre:
Con la consulta se ejecuta CitaVeterinaria.ObtenerDescripcion().
Con la hospitalización se ejecuta Hospitalizacion.ObtenerDescripcion().
Con el lote se ejecuta LoteMedicamento.ObtenerDescripcion().
Con ObtenerPrecioUnitario() pasa lo mismo. En los servicios llama a CalcularCosto(), que cada clase hija sobreescribe con su propia fórmula.

5. Sobrecarga
Sobrecargamos Agregar en Factura: Agregar(Servicio servicio) y Agregar(IFacturable item, int cantidad). También hay dos EstaVencido en LoteMedicamento, una versión con fecha y otra que usa la fecha de hoy.
Preferimos dos métodos y no uno con cantidad = 1 opcional porque la directora pidió que para las consultas el sistema asuma una unidad, pero que para los medicamentos sí se indique cuántos. Con un parámetro opcional, olvidar la cantidad de un medicamento lo agregaría en silencio como 1 unidad. Con la sobrecarga, Agregar(lote) ni siquiera compila. Si alguien intenta facturar una consulta con cantidad distinta de 1, ValidarFacturacion lo rechaza.

6. Peluquería y alimentos
Clases nuevas: Peluqueria y Vacunacion heredarían de Servicio e implementarían CalcularCosto(). Alimento y Accesorio implementarían IFacturable, como LoteMedicamento.
Clases existentes que habría que modificar: ninguna. Factura ya acepta cualquier Servicio o IFacturable, y RenglonFactura guarda un IFacturable. Lo único que cambiaría es el Main, y solo para crear y facturar los objetos nuevos.

