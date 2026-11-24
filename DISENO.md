# Decisiones de diseño · PetCare Web (Unidad 3)

La versión de la Unidad 2 de este documento quedó en la rama `unidad-2-final`.

## 1. Qué cambiamos frente al diagrama de clases y por qué

Dejamos los nombres de la Unidad 2 para que el proyecto siga siendo el mismo que entregamos en consola. Todas las clases del diagrama están, algunas con otro nombre:

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

- **En el diagrama dice `{init}` y en el código quedó `{ get; }`.** Codigo, NombreMascota, Fecha, Numero, FechaEmision, NombreCliente, UsuarioEmisor, TarifaDiaria, FechaVencimiento y NombreUsuario son de solo lectura, igual que en la Unidad 2. Así queda más protegido: con `init` alguien podría escribir `new Factura(...) { Numero = "" }` y saltarse la validación del constructor, y con `{ get; }` solo el constructor les puede dar valor. `Usuario.NombreCompleto` y `Usuario.Rol` también son de solo lectura.
- **`RenglonFactura.Cantidad` y `PrecioUnitario` tienen `private set`.** Desde afuera funcionan como un `init`: nadie fuera de la clase los cambia, y como los constructores son `internal`, el único que crea renglones es `Factura.Agregar`.
- **`Factura.Anular` devuelve `bool`**, aunque en el diagrama es `void`. Devuelve false si la factura ya estaba anulada, igual que `DarDeAlta` y `ConfirmarVeterinario` cuando eso ya se había hecho. Si el que anula no es administrador, lanza una excepción.
- **`IFacturable.Codigo`** no está en el diagrama. Lo agregamos porque el inventario (F6) muestra el código de cada cosa recorriendo una lista de `IFacturable`, sin preguntar si es servicio o lote (`TablaInventario.razor`, línea 21).
- **`CalcularDiasFaltantes`** viene de la Unidad 2 y cuenta días, no minutos. Ninguna pantalla lo usa, pero lo dejamos para no dañar lo que ya habíamos entregado.

Lo que agregamos:

- Un `Id` con `private set` y un constructor sin parámetros privado (protected en las clases abstractas) en cada clase que se guarda en la base. Ese constructor solo lo usa Entity Framework Core cuando lee los datos, entonces desde afuera no se pueden crear objetos inválidos.
- `ProductoFarmacia` es nueva, y ahora `LoteMedicamento` hereda de ella. Subimos `NombreMedicamento` y `PrecioUnitario` a la clase base porque en la tabla Productos las columnas Nombre y PrecioUnidad no pueden quedar vacías para ningún producto.
- `CitaVeterinaria` sigue teniendo `CedulaCliente`, `Motivo` y `Estado`, y `LoteMedicamento` sigue teniendo `Tipo`, `EnNevera` y `StockActual`. Vienen de la Unidad 2 y la base de datos tiene columnas para ellos, por eso los constructores los reciben. También se quedaron sus métodos de la Unidad 2: `MarcarAtendida`, `MarcarCancelada` y `EstaVigente` en la cita, y `RecibirUnidades`, `DespacharUnidades`, `NecesitaReabastecimiento`, `CalcularValorInventario` y `EstaVencido(fecha)` en el lote.
- `Jaula` se queda, porque existe la tabla Jaulas. La tarifa diaria de la hospitalización sale de la jaula cuando la mascota ingresa y no cambia después; por eso el constructor recibe la jaula y no la tarifa. `Jaula.Ocupante` no se guarda: en la base, una jaula está ocupada si tiene una hospitalización sin alta, y el índice `UX_Servicios_JaulaOcupada` no deja que haya dos al mismo tiempo.
- `ReglaNegocioException` y `CodigoDuplicadoException`, para los errores que el usuario sí puede corregir.

Todo el mapeo está en `PetCareWeb/Datos/Configuraciones`, con una clase por tabla y los mismos nombres de restricciones de `BaseDeDatos.sql` (PK_, UQ_, CK_, FK_, IX_). El script está en la raíz del repositorio. La base que crea la migración tiene las mismas tablas, columnas, llaves, UNIQUE, CHECK e índices. Lo único distinto es el nombre del valor por defecto de EstaAnulada: en el script se llama `DF_Facturas_EstaAnulada` y en la migración SQL Server le pone el nombre solo. Funciona igual.

## 2. Cómo guardamos las jerarquías y los renglones

Cada jerarquía se guarda en una sola tabla (TPH). En Servicios, la columna `Tipo` dice si la fila es una `Cita` o una `Hospitalizacion`, y en Productos dice `LoteMedicamento`. Cuando leemos `db.Servicios`, Entity Framework Core mira esa columna y crea un `CitaVeterinaria` o un `Hospitalizacion`, así que el polimorfismo no se pierde (`ServicioConfiguracion.cs`, `ProductoFarmaciaConfiguracion.cs`).

Los renglones se guardan en LineasFactura con un `ServicioId` o un `ProductoId`, nunca los dos (`CK_LineasFactura_UnSoloOrigen`). Entity Framework Core llena la lista privada `_renglones` directamente por el campo, y desde afuera solo se ve como `IReadOnlyList`. El precio unitario se guarda en el momento en que se agrega el renglón (HU-10 CA2).

Cuando se anula una factura:

- En memoria, `Factura.Anular` pone `Anulada = true`, y desde ese momento `Agregar` lanza una excepción.
- En la base de datos solo cambia `EstaAnulada` a 1. Los renglones no se borran, porque son parte de la factura.
- Las citas, hospitalizaciones y lotes no se tocan. Las llaves foráneas de los renglones son `NO ACTION` y la factura solo los apuntaba, por eso se pueden volver a cobrar en otra factura (HU-14).

## 3. Un ejemplo de cada pilar

- **Abstracción:** `IFacturable`. A la factura solo le importa el código, la descripción, el precio y si se puede cobrar; no le interesa qué es lo que está cobrando.
- **Encapsulamiento:** `Factura._renglones`. La lista es privada, afuera se ve como `IReadOnlyList` y el constructor de `RenglonFactura` es `internal`. La única forma de agregar un renglón es con `Factura.Agregar`, que valida antes.
- **Herencia:** `CitaVeterinaria` y `Hospitalizacion` heredan de `Servicio` el código, la mascota, la fecha y sus validaciones (constructor `protected`).
- **Polimorfismo:** `CalcularCosto()`. La cita cobra por minutos y la hospitalización cobra por días según la tarifa de la jaula, y el que llama no tiene que saber cuál es cuál.

## 4. Dónde ocurre el polimorfismo dinámico al mostrar la factura

En `PetCareWeb/Components/Compartidos/FacturaImprimible.razor`, línea 59: `@renglon.Descripcion`. Esa propiedad (`RenglonFactura.cs`, línea 29) llama a `Item.ObtenerDescripcion()`, e `Item` es un `IFacturable`. Cuando el programa corre, se ejecuta el método del objeto real:

- si es una consulta, se ejecuta `CitaVeterinaria.ObtenerDescripcion()`,
- si es una hospitalización, se ejecuta `Hospitalizacion.ObtenerDescripcion()`,
- si es un lote, se ejecuta `LoteMedicamento.ObtenerDescripcion()`.

Eso lo decide .NET según el tipo real del objeto, no la página. El total (`Factura.Total`, línea 34) suma los subtotales sin preguntar el tipo de nada.

Escogimos dos sobrecargas de `Agregar` en vez de un solo método con la cantidad opcional porque la regla es diferente: a un servicio nunca se le pide cantidad y a un medicamento siempre. Con un parámetro opcional, si se nos olvida poner la cantidad de un medicamento, se agregaría como 1 sin avisar. Con la sobrecarga, `Agregar(lote)` ni siquiera compila.

## 5. Recorrido de un clic: Anular factura

1. El botón solo aparece si el usuario es administrador (`Facturacion.razor`, línea 7).
2. `@onclick="PedirConfirmacionAnular"` abre el `DialogoConfirmacion`. Si el usuario cancela, `AlCancelar` cierra la ventana y no pasa nada.
3. `AlConfirmar` ejecuta `AnularAsync` en `Facturacion.razor.cs`, que llama a `ServicioFacturas.AnularAsync(facturaId, usuarioId)` con el usuario que tiene la sesión abierta.
4. El servicio carga la factura y el usuario y llama a `factura.Anular(usuario)`. Ahí se revisa el rol: si no es administrador, el dominio lanza `InvalidOperationException`, aunque alguien hubiera llegado sin pasar por el botón.
5. `SaveChangesAsync` hace un `UPDATE Facturas SET EstaAnulada = 1`. La página vuelve a leer la factura y muestra el sello de anulada.

## 6. Vacunación y alimentos

- **Clases nuevas:** `Vacunacion : Servicio`, con su `CalcularCosto` y su `ObtenerDescripcion`, y `Alimento : ProductoFarmacia`, con su `ObtenerDescripcion` y, si hace falta, su `ValidarFacturacion`.
- **Pantallas nuevas:** un formulario para registrar cada una y su opción en el menú.
- **Base de datos:** registrar el tipo en su configuración (`HasValue<Vacunacion>("Vacunacion")`), agregar sus columnas si tiene datos propios y crear una migración. Las restricciones `CK_Servicios_Tipo` y `CK_Productos_Tipo` tienen que aceptar el valor nuevo.
- **Qué se toca de lo que ya existe:** `Factura`, `RenglonFactura`, `FacturaImprimible` e `Inventario` quedan igual. Solo cambian las configuraciones de Servicio o de Producto (una línea en el discriminador y el CHECK), porque la columna Tipo tiene que conocer el valor nuevo.
