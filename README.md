# PetCare 2.0 · Todo se cobra en la misma factura

Producto de la Unidad 2 · Programación Orientada a Objetos (SD1007) · Grupo 053 · 2026-II
Institución Universitaria Pascual Bravo · Docente: Jorge Albeiro Muriel Vélez

## Integrantes

| Integrante | Responsabilidad |
|---|---|
| Ader | Servicios y reglas de cobro: `Servicio`, `CitaVeterinaria`, `Hospitalizacion` |
| Adel Ramos | Medicamentos e interfaz: `LoteMedicamento`, `IFacturable` |
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

## Avance

| Parte | Estado |
|---|---|
| `Medicamento` de la Unidad 1 unido a `LoteMedicamento` | Hecho |
| `Jaula` de la Unidad 1 conectada a `Hospitalizacion` (agregación) | Hecho |
| `IFacturable` | Hecho |
| `LoteMedicamento` | Hecho |
| `Servicio` (abstracta), `CitaVeterinaria`, `Hospitalizacion` | Hecho |
| `CitaVeterinaria` recupera cédula, motivo y estados de la Unidad 1 | Hecho |
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
        +CedulaCliente : string
        +Motivo : string
        +DuracionMinutos : int
        +Veterinario : string?
        +Estado : EstadoCita
        +CitaVeterinaria(codigo, nombreMascota, cedulaCliente, fecha, motivo, duracionMinutos = 30)
        +ConfirmarVeterinario(veterinario : string) bool
        +MarcarAtendida() bool
        +MarcarCancelada() bool
        +CalcularDiasFaltantes() int
        +EstaVigente() bool
        +CalcularCosto() decimal
        +ObtenerDescripcion() string
        +ValidarFacturacion(cantidad : int) void
    }

    class Hospitalizacion {
        +Jaula : Jaula
        +TarifaDiaria : decimal
        +FechaAlta : DateTime?
        +Hospitalizacion(codigo, nombreMascota, fechaIngreso, jaula)
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

    class EstadoCita {
        <<enumeration>>
        Programada
        Atendida
        Cancelada
    }

    class TipoMedicamento {
        <<enumeration>>
        Antiparasitarios
        Antibioticos
        Analgesicos
        Vacunas
    }

    class Jaula {
        -TarifasPorTamano : Dictionary~TamanoJaula, decimal~$
        +Codigo : string
        +Tamano : TamanoJaula
        +TarifaDia : decimal
        +Ocupante : Hospitalizacion?
        +Ocupado : bool
        +Jaula(codigo, tamano)
        ~IngresarPaciente(hospitalizacion : Hospitalizacion) void
        ~Liberar() void
        +ConsultarEstado() string
    }

    class TamanoJaula {
        <<enumeration>>
        Pequeno
        Mediano
        Grande
    }

    Servicio ..|> IFacturable
    LoteMedicamento ..|> IFacturable
    Servicio <|-- CitaVeterinaria
    Servicio <|-- Hospitalizacion
    Factura "1" *-- "0..*" RenglonFactura : renglones
    RenglonFactura "0..*" o-- "1" IFacturable : cobra
    CitaVeterinaria ..> EstadoCita
    LoteMedicamento ..> TipoMedicamento
    Hospitalizacion "0..*" o-- "1" Jaula : ocupa
    Jaula --> "0..1" Hospitalizacion : ocupante actual
    Jaula ..> TamanoJaula
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
- `Hospitalizacion` ◇ `Jaula` (**agregación**, 0..* a 1): la hospitalización recibe una jaula que ya existe, toma su tarifa al ingresar, la ocupa y la libera al dar de alta. La jaula sigue existiendo para el siguiente animal. La jaula no copia el nombre de la mascota: guarda a su ocupante actual (0..1) y de ahí lee el nombre, los días y la cuenta.
- `RenglonFactura` ◇ `IFacturable` (**agregación**, 0..* a 1): la consulta, la hospitalización o el lote llegan ya creados y siguen existiendo si la factura se anula, así que se pueden facturar de nuevo.

## Reglas de negocio implementadas

| Clase | Regla |
|---|---|
| `CitaVeterinaria` | Vale $60.000 hasta 30 minutos y $1.500 por cada minuto adicional |
| `CitaVeterinaria` | La duración va de 15 a 180 minutos y arranca en 30 |
| `CitaVeterinaria` | El veterinario se asigna una sola vez al confirmar |
| `CitaVeterinaria` | Toda cita arranca Programada; solo una cita programada se puede atender o cancelar |
| `CitaVeterinaria` | Una cita atendida no se cancela y no se cancela dos veces (devuelve false para avisar) |
| `CitaVeterinaria` | Una cita cancelada no se puede facturar |
| `Hospitalizacion` | Se cobra por días entre ingreso y alta (o hasta hoy), mínimo un día |
| `Hospitalizacion` | El alta se da una sola vez y no puede ser anterior al ingreso |
| `Hospitalizacion` | La tarifa diaria se toma de la jaula al ingresar; al dar de alta la jaula queda libre |
| `Servicio` | Siempre se factura como una unidad |
| `Factura` | Una factura anulada no admite más renglones |
| `Jaula` | No entra un segundo paciente a una jaula ocupada |
| `Jaula` | La tarifa por día depende del tamaño: Pequeña $25.000, Mediana $38.000, Grande $52.000 |
| `Factura` | Al agregar una consulta no se escribe la cantidad: el sistema asume 1 |
| `LoteMedicamento` | Un lote vencido no se puede vender, ni una unidad |
| `LoteMedicamento` | El día del vencimiento todavía se vende; desde el día siguiente no |
| `LoteMedicamento` | Código y nombre obligatorios, precio mayor que cero, cantidad mayor que cero |
| `LoteMedicamento` | No se factura ni se despacha más de lo que hay en stock |
| `LoteMedicamento` | No se despacha si está vencido o si la cantidad es 0; avisa cuando quedan menos de 10 unidades |
