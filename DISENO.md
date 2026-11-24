# Decisiones de diseño de PetCare Web (Unidad 3)

## 1. Qué cambiamos frente al diagrama de clases y por qué

Dejamos los nombres de la Unidad 2 para que el proyecto siga siendo el mismo que entregamos en consola:

| Diagrama | Código | Columna en la base de datos |
|---|---|---|
| ServicioVeterinario | `Servicio` | tabla Servicios |
| LineaFactura | `RenglonFactura` | tabla LineasFactura |
| Factura.Lineas / _lineas | `Factura.Renglones` / `_renglones` | |
| Factura.EstaAnulada | `Factura.Anulada` | EstaAnulada |
| AgregarLinea (dos sobrecargas) | `Agregar(Servicio)` y `Agregar(ProductoFarmacia, int)` | |
| ValidarFacturable | `ValidarFacturacion` | |
| ProductoFarmacia.Nombre / PrecioUnidad | `NombreMedicamento` / `PrecioUnitario` | Nombre / PrecioUnidad |
| CitaVeterinaria.VeterinarioAsignado / Confirmar | `Veterinario` / `ConfirmarVeterinario` | VeterinarioAsignado |
| Hospitalizacion.DarAlta | `DarDeAlta` | |
| CitaVeterinaria.CalcularMinutosParaLaCita | `CalcularDiasFaltantes` | |

Lo que cambia en modificadores y firmas:

- **`{init}` en el diagrama, `{ get; }` en el código.** Las propiedades que no cambian (Codigo, Fecha, Numero, TarifaDiaria, entre otras) son de solo lectura, como en la Unidad 2. Protege más: con `init` se podría escribir `new Factura(...) { Numero = "" }` y saltarse la validación del constructor.
- **`Factura.Anular` devuelve `bool`** y no `void`: false si ya estaba anulada, como `DarDeAlta`. Si el que anula no es administrador, lanza una excepción.
- **`IFacturable.Codigo`** es nuevo: el inventario (F6) muestra el código de cada cosa sin preguntar su tipo (`TablaInventario.razor`, línea 21).

También agregamos: un `Id` con `private set` y un constructor sin parámetros privado (protected en las abstractas), que solo usa Entity Framework Core al leer; `ProductoFarmacia`, base de `LoteMedicamento`, porque en la tabla Productos el nombre y el precio son obligatorios; lo que venía de la Unidad 2 y tiene columna en la base (cédula, motivo y estado de la cita; tipo, nevera y stock del lote); `Jaula`, de donde sale la tarifa de la hospitalización al ingresar, por eso el constructor recibe la jaula y no la tarifa; y `ReglaNegocioException` y `CodigoDuplicadoException`, para los errores que el usuario puede corregir. El mapeo está en `PetCareWeb/Datos/Configuraciones`, con los nombres de restricciones de `BaseDeDatos.sql`.

## 2. Cómo guardamos las jerarquías y los renglones

Cada jerarquía va en una sola tabla (TPH): la columna `Tipo` dice si la fila es `Cita`, `Hospitalizacion` o `LoteMedicamento`, y al leer, Entity Framework Core crea el objeto correcto (`ServicioConfiguracion.cs`). Los renglones van en LineasFactura con un `ServicioId` o un `ProductoId`, nunca los dos (`CK_LineasFactura_UnSoloOrigen`). EF Core llena `_renglones` por el campo, y el precio se guarda al agregar el renglón (HU-10 CA2).

Cuando se anula una factura:

- En memoria, `Anulada` pasa a true y desde ahí `Agregar` lanza una excepción.
- En la base solo cambia `EstaAnulada` a 1; los renglones se quedan porque son parte de la factura.
- Las citas, hospitalizaciones y lotes no se tocan (las llaves son `NO ACTION`), por eso se pueden cobrar en otra factura (HU-14).

## 3. Un ejemplo de cada pilar

- **Abstracción:** `IFacturable`. A la factura solo le importa la descripción, el precio y si se puede cobrar, no qué es.
- **Encapsulamiento:** `Factura._renglones`. Es privada, afuera se ve como `IReadOnlyList` y solo `Factura.Agregar`, que valida antes, crea renglones.
- **Herencia:** `CitaVeterinaria` y `Hospitalizacion` heredan de `Servicio` el código, la mascota, la fecha y sus validaciones (constructor `protected`).
- **Polimorfismo:** `CalcularCosto()`. La cita cobra por minutos y la hospitalización por días, y el que llama no tiene que saber cuál es cuál.

## 4. Dónde ocurre el polimorfismo dinámico al mostrar la factura

En `FacturaImprimible.razor`, línea 59: `@renglon.Descripcion`. Esa propiedad (`RenglonFactura.cs`, línea 29) llama a `Item.ObtenerDescripcion()`, e `Item` es un `IFacturable`. Cuando el programa corre se ejecuta el método del objeto real: el de `CitaVeterinaria`, el de `Hospitalizacion` o el de `LoteMedicamento`. Eso lo decide .NET según el tipo real, no la página. El total (`Factura.Total`, línea 34) suma los subtotales sin preguntar el tipo de nada.

Usamos dos sobrecargas de `Agregar` y no un método con cantidad opcional porque a un servicio nunca se le pide cantidad y a un medicamento siempre: con un opcional, un medicamento sin cantidad entraría como 1 sin avisar; con la sobrecarga, `Agregar(lote)` no compila.

## 5. Recorrido de un clic: Anular factura

1. El botón solo aparece para el administrador (`Facturacion.razor`, línea 7).
2. El clic abre el `DialogoConfirmacion`; si el usuario cancela, no pasa nada.
3. `AlConfirmar` ejecuta `AnularAsync`, que llama a `ServicioFacturas.AnularAsync` con el usuario de la sesión.
4. El servicio llama a `factura.Anular(usuario)`, donde se revisa el rol: si no es administrador, el dominio lanza `InvalidOperationException`, aunque alguien llegue sin pasar por el botón.
5. `SaveChangesAsync` hace un `UPDATE Facturas SET EstaAnulada = 1` y la página muestra el sello de anulada.

## 6. Vacunación y alimentos

- **Clases y pantallas nuevas:** `Vacunacion : Servicio`, con su `CalcularCosto`, y `Alimento : ProductoFarmacia`, cada una con su `ObtenerDescripcion`, su formulario y su opción en el menú.
- **Base de datos:** registrar el tipo en su configuración (`HasValue<Vacunacion>("Vacunacion")`), agregar sus columnas si tiene datos propios, ajustar `CK_Servicios_Tipo` o `CK_Productos_Tipo` y crear una migración.
- **Qué se toca de lo que ya existe:** `Factura`, `RenglonFactura`, `FacturaImprimible` e `Inventario` quedan igual; solo cambia la configuración de Servicio o Producto, porque la columna Tipo tiene que conocer el valor nuevo.
