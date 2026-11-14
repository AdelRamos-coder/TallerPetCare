# Decisiones de diseño · PetCare Web (Unidad 3)

La versión de la Unidad 2 de este documento quedó en la rama `unidad-2-final`.

## 1. Qué cambiamos frente al diagrama de clases y por qué

Mantuvimos los nombres de la Unidad 2 para que el proyecto siga siendo el mismo que entregamos en consola. Cada clase del diagrama existe con otro nombre o con el mismo:

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

Lo que agregamos:

- `Id` con `private set` y un constructor sin parámetros privado (protected en las abstractas) en cada clase que se guarda. Solo lo usa Entity Framework Core al leer, así que no se pueden crear objetos inválidos desde afuera.
- `ProductoFarmacia` es nueva: `LoteMedicamento` ahora hereda de ella. `NombreMedicamento` y `PrecioUnitario` subieron a la base porque en la tabla Productos las columnas Nombre y PrecioUnidad son NOT NULL para cualquier producto.
- `CitaVeterinaria` conserva `CedulaCliente`, `Motivo` y `Estado`, y `LoteMedicamento` conserva `Tipo`, `EnNevera` y `StockActual`: vienen de la Unidad 2 y la base de datos tiene sus columnas.
- `Jaula` se queda (tabla Jaulas). La tarifa diaria de la hospitalización sale de la jaula al ingresar y no cambia después; por eso el constructor recibe la jaula y no la tarifa. `Jaula.Ocupante` no se guarda: en la base, una jaula está ocupada si tiene una hospitalización sin alta, y el índice `UX_Servicios_JaulaOcupada` impide dos a la vez.
- `ReglaNegocioException` y `CodigoDuplicadoException` para las reglas que el usuario puede corregir.

Todo el mapeo está en `PetCareWeb/Datos/Configuraciones`, una clase por tabla, con los nombres de restricciones de `BaseDeDatos.sql` (PK_, UQ_, CK_, FK_, IX_).

## 2. Cómo guardamos las jerarquías y los renglones

Las dos jerarquías usan una sola tabla cada una (TPH). En Servicios la columna `Tipo` dice si la fila es `Cita` u `Hospitalizacion`, y en Productos dice `LoteMedicamento`. Al leer `db.Servicios`, Entity Framework Core mira esa columna y crea un `CitaVeterinaria` o un `Hospitalizacion`, así que el polimorfismo no se pierde (`ServicioConfiguracion.cs`, `ProductoFarmaciaConfiguracion.cs`).

Los renglones van en LineasFactura con `ServicioId` o `ProductoId`, nunca los dos (`CK_LineasFactura_UnSoloOrigen`). Entity Framework Core llena la lista privada `_renglones` directamente por el campo, y afuera solo se ve como `IReadOnlyList`. El precio unitario se guarda al agregar el renglón (HU-10 CA2).

Al anular una factura:

- En memoria, `Factura.Anular` pone `Anulada = true` y desde ahí `Agregar` lanza una excepción.
- En la base de datos solo cambia `EstaAnulada` a 1. Los renglones se quedan (son parte de la factura).
- Las citas, hospitalizaciones y lotes no se tocan: las llaves foráneas de los renglones son `NO ACTION` y la factura solo los apuntaba. Por eso se pueden cobrar en otra factura (HU-14).

## 3. Un ejemplo de cada pilar

- **Abstracción:** `IFacturable`. La factura solo necesita saber el código, la descripción, el precio y si se puede cobrar; no le importa qué es.
- **Encapsulamiento:** `Factura._renglones`. La lista es privada, se expone como `IReadOnlyList` y el constructor de `RenglonFactura` es `internal`, así que la única forma de agregar un renglón es `Factura.Agregar`, que valida antes.
- **Herencia:** `CitaVeterinaria` y `Hospitalizacion` heredan de `Servicio` el código, la mascota, la fecha y sus validaciones (constructor `protected`).
- **Polimorfismo:** `CalcularCosto()`. La cita cobra por minutos y la hospitalización por días por la tarifa de la jaula; quien llama no distingue.

## 4. Dónde ocurre el polimorfismo dinámico al mostrar la factura

En `PetCareWeb/Components/Compartidos/FacturaImprimible.razor`, línea 59: `@renglon.Descripcion`. Esa propiedad (`RenglonFactura.cs`, línea 29) llama a `Item.ObtenerDescripcion()`, e `Item` es un `IFacturable`. En tiempo de ejecución se ejecuta el método del objeto real:

- una consulta ejecuta `CitaVeterinaria.ObtenerDescripcion()`,
- una hospitalización ejecuta `Hospitalizacion.ObtenerDescripcion()`,
- un lote ejecuta `LoteMedicamento.ObtenerDescripcion()`.

Lo decide el runtime de .NET según el tipo real del objeto, no la página. El total (`Factura.Total`, línea 34) suma los subtotales sin preguntar el tipo de nada.

Preferimos dos sobrecargas de `Agregar` y no un solo método con cantidad opcional porque la regla es distinta: a un servicio nunca se le pide cantidad y a un medicamento siempre. Con un parámetro opcional, olvidar la cantidad de un medicamento lo agregaría como 1 sin aviso; con la sobrecarga, `Agregar(lote)` ni siquiera compila.

## 5. Recorrido de un clic: Anular factura

1. El botón solo se dibuja si el usuario es administrador (`Facturacion.razor`, línea 7).
2. `@onclick="PedirConfirmacionAnular"` abre `DialogoConfirmacion`. Si se cancela, `AlCancelar` cierra la ventana y no pasa nada.
3. `AlConfirmar` ejecuta `AnularAsync` en `Facturacion.razor.cs`, que llama a `ServicioFacturas.AnularAsync(facturaId, usuarioId)` con el usuario de la sesión.
4. El servicio carga la factura y el usuario y llama a `factura.Anular(usuario)`. Ahí está la validación de rol: si no es administrador, el dominio lanza `InvalidOperationException` aunque alguien hubiera llegado sin el botón.
5. `SaveChangesAsync` ejecuta un `UPDATE Facturas SET EstaAnulada = 1`. La página vuelve a leer la factura y muestra el sello de anulada.

## 6. Vacunación y alimentos

- **Clases nuevas:** `Vacunacion : Servicio` con su `CalcularCosto` y `ObtenerDescripcion`, y `Alimento : ProductoFarmacia` con su `ObtenerDescripcion` y, si aplica, su `ValidarFacturacion`.
- **Pantallas nuevas:** un formulario para registrar cada una y su entrada en el menú.
- **Base de datos:** registrar el tipo en su configuración (`HasValue<Vacunacion>("Vacunacion")`), agregar sus columnas si tiene datos propios y crear una migración. Las restricciones `CK_Servicios_Tipo` y `CK_Productos_Tipo` deben admitir el valor nuevo.
- **Qué se modifica de lo existente:** `Factura`, `RenglonFactura`, `FacturaImprimible` e `Inventario` no cambian. Solo se tocan las configuraciones de Servicio o Producto (una línea en el discriminador y el CHECK), porque la columna Tipo tiene que conocer el valor nuevo.
