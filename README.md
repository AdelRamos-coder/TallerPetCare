# PetCare 2.0 · Todo se cobra en la misma factura

Producto de la Unidad 2 · Programación Orientada a Objetos (SD1007) · Grupo 053 · 2026-II
Institución Universitaria Pascual Bravo · Docente: Jorge Albeiro Muriel Vélez

## Integrantes

| Integrante | Responsabilidad |
|---|---|
| Ader | Servicios y reglas de cobro: `Servicio`, `CitaVeterinaria`, `Hospitalizacion` |
| Adel Ramos | Medicamentos e interfaz: `LoteMedicamento`, `IFacturable` y sus pruebas |
| Wilson Caraballo | Facturación: `Factura`, `RenglonFactura`, anulación |
| Luis | Integración, `Main` de demostración, diagrama UML, historias de usuario, `DISENO.md` e `IA.md` |

## Cómo ejecutarlo

Requiere .NET 10.

```bash
git clone https://github.com/AdelRamos-coder/TallerPetCare.git
cd TallerPetCare
dotnet run
```

`dotnet run` ejecuta la demostración de los 8 pasos.
Para correr las pruebas de `LoteMedicamento`: `dotnet run -- pruebas`.

## Avance

| Parte | Estado |
|---|---|
| `Medicamento` de la Unidad 1 unido a `LoteMedicamento` | Hecho |
| `Jaula` de la Unidad 1 retirada (su cobro por días quedó en `Hospitalizacion`) | Hecho |
| `IFacturable` | Hecho |
| `LoteMedicamento` + pruebas | Hecho |
| `Servicio` (abstracta), `CitaVeterinaria`, `Hospitalizacion` | Hecho |
| `Factura`, `RenglonFactura` | Hecho |
| `Main` con los 8 pasos de la demostración | Hecho |
| `DISENO.md`, `IA.md`, historias de usuario, diagrama en draw.io | Pendiente |

## Diagrama de clases · estado actual

Este diagrama muestra **lo que hay hoy en el código**. Se actualiza cada vez que se sube una clase nueva.

```mermaid
classDiagram
    direction TB

    class IFacturable {
        <<interface>>
        +Codigo : string
        +ObtenerDescripcion() string
        +ObtenerPrecioUnitario() decimal
        +ValidarFacturacion(cantidad : int) void
    }

    class Servicio {
        <<abstract>>
        +Codigo : string
        +NombreMascota : string
        +Fecha : DateTime
        #Servicio(codigo, nombreMascota, fecha)
        +CalcularCosto()* decimal
        +ObtenerDescripcion() string
        +ObtenerPrecioUnitario() decimal
        +ValidarFacturacion(cantidad : int) void
    }

    class CitaVeterinaria {
        -DuracionMinima : int = 15
        -DuracionMaxima : int = 180
        -MinutosBase : int = 30
        -TarifaBase : decimal = 60000
        -TarifaMinutoAdicional : decimal = 1500
        +DuracionMinutos : int
        +Veterinario : string?
        +CitaVeterinaria(codigo, nombreMascota, fecha, duracionMinutos = 30)
        +ConfirmarVeterinario(veterinario : string) bool
        +CalcularCosto() decimal
        +ObtenerDescripcion() string
    }

    class Hospitalizacion {
        +TarifaDiaria : decimal
        +FechaAlta : DateTime?
        +Hospitalizacion(codigo, nombreMascota, fechaIngreso, tarifaDiaria)
        +DarDeAlta(fechaAlta : DateTime) bool
        +CalcularDias() int
        +CalcularCosto() decimal
        +ObtenerDescripcion() string
    }

    class LoteMedicamento {
        -StockMinimo : int = 10
        +Codigo : string
        +NombreMedicamento : string
        +Tipo : TipoMedicamento
        +FechaVencimiento : DateOnly
        +PrecioUnitario : decimal
        +EnNevera : bool
        +StockActual : int
        +LoteMedicamento(codigo, nombreMedicamento, tipo, fechaVencimiento, precioUnitario, stockInicial, enNevera = false)
        +EstaVencido(fecha : DateOnly) bool
        +EstaVencido() bool
        +RecibirUnidades(cantidad : int) void
        +DespacharUnidades(cantidad : int) bool
        +NecesitaReabastecimiento() bool
        +CalcularValorInventario() decimal
        +ObtenerDescripcion() string
        +ObtenerPrecioUnitario() decimal
        +ValidarFacturacion(cantidad : int) void
    }

    class PruebasLoteMedicamento {
        <<static>>
        +EjecutarTodas() void
    }

    class Factura {
        -_renglones : List~RenglonFactura~
        +Numero : string
        +FechaEmision : DateTime
        +NombreCliente : string
        +Anulada : bool
        +Renglones : IReadOnlyList~RenglonFactura~
        +Total : decimal
        +Factura(numero, fechaEmision, nombreCliente)
        +Agregar(servicio : Servicio) void
        +Agregar(item : IFacturable, cantidad : int) void
        +Anular() bool
    }

    class RenglonFactura {
        +Item : IFacturable
        +Descripcion : string
        +Cantidad : int
        +Subtotal : decimal
        ~RenglonFactura(item, descripcion, cantidad, subtotal)
    }

    class TipoMedicamento {
        <<enumeration>>
        Antiparasitarios
        Antibioticos
        Analgesicos
        Vacunas
    }

    Servicio ..|> IFacturable
    LoteMedicamento ..|> IFacturable
    Servicio <|-- CitaVeterinaria
    Servicio <|-- Hospitalizacion
    Factura "1" *-- "0..*" RenglonFactura : renglones
    RenglonFactura "0..*" o-- "1" IFacturable : cobra
    PruebasLoteMedicamento ..> LoteMedicamento
    LoteMedicamento ..> TipoMedicamento
```

Los métodos con `*` son abstractos.

### Cómo leer las flechas

| Flecha | Relación | Significado aquí |
|---|---|---|
| Línea continua con triángulo vacío | Herencia | `CitaVeterinaria` y `Hospitalizacion` **son** servicios |
| Punteada con triángulo vacío | Realización | `Servicio` y `LoteMedicamento` cumplen el contrato `IFacturable`, sin ser de la misma familia |
| Rombo relleno ◆ | Composición | La factura es dueña de sus renglones |
| Rombo vacío ◇ | Agregación | El renglón apunta a algo que existe por su cuenta |
| Punteada con flecha abierta | Dependencia | La clase usa el enum o la otra clase |

### Composición y agregación

- `Factura` ◆ `RenglonFactura` (**composición**, 1 a 0..*): la factura crea y controla sus renglones; desde afuera solo se leen. Si la factura se anula, sus renglones ya no sirven para otra.
- `RenglonFactura` ◇ `IFacturable` (**agregación**, 0..* a 1): la consulta, la hospitalización o el lote llegan ya creados y siguen existiendo si la factura se anula, así que se pueden facturar de nuevo.

## Reglas de negocio implementadas

| Clase | Regla |
|---|---|
| `CitaVeterinaria` | Vale $60.000 hasta 30 minutos y $1.500 por cada minuto adicional |
| `CitaVeterinaria` | La duración va de 15 a 180 minutos y arranca en 30 |
| `CitaVeterinaria` | El veterinario se asigna una sola vez al confirmar |
| `Hospitalizacion` | Se cobra por días entre ingreso y alta (o hasta hoy), mínimo un día |
| `Hospitalizacion` | El alta se da una sola vez y no puede ser anterior al ingreso |
| `Servicio` | Siempre se factura como una unidad |
| `Factura` | Una factura anulada no admite más renglones |
| `Factura` | Al agregar una consulta no se escribe la cantidad: el sistema asume 1 |
| `LoteMedicamento` | Un lote vencido no se puede vender, ni una unidad |
| `LoteMedicamento` | El día del vencimiento todavía se vende; desde el día siguiente no |
| `LoteMedicamento` | Código y nombre obligatorios, precio mayor que cero, cantidad mayor que cero |
| `LoteMedicamento` | No se factura ni se despacha más de lo que hay en stock |
| `LoteMedicamento` | No se despacha si está vencido o si la cantidad es 0; avisa cuando quedan menos de 10 unidades |
