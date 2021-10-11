# Lesson 27. Exposing F# types and functions to C#

### Using F# Records in C#

* A default constructor *won't* normally be generated, so it won’t be possible to create the
record in an uninitialized (or partially initialized) state.

* Each field will appear as a *public getter-only property*.

* The class will implement various interfaces in order to allow *structural equality*
checking.

* Triple-slash comments show in tooltips.

```fsharp
/// A standard F# record of a Car.
type Car = {
    /// The number of wheels on the car.
    Wheels : int
    /// The brand of the car.
    Brand : string
    }
```

```csharp
using Model;

var car = new Car(4, "Supacars", new Tuple<double, double>(1.5, 3.4));
var brand = car.Brand;      // get-only
var wheels = car.Wheels;    // get-only
```

### Using F# Tuples and Discriminated unions (DU) in C#

**Tuples**:

* Tuples in F# are instances of the standard `System.Tuple` type.

* Tuples appear in C# as standard `Item1`, `Item2`, and `ItemN` properties.

* The standard .NET tuple type supports up to *only* 8 items.

* Strongly recommended *avoid more than three items* in tuples.

**Discriminated unions**:

* DU represents class hierarchy in C#:

  * DU as abstract class

  * Types of DU as nested classes

* В C# используется автогенерация методов-фабрик для внутренних классов DU.

* В C# используется автогенерация bool свойств для проверки типа внутреннего класса для cast.

* Чтобы использовать в C# внутренние классы из DU, надо делать cast.

```fsharp
/// A standard F# record of a Car.
type Car = {
    /// The number of wheels on the car.
    Wheels : int
    /// The brand of the car.
    Brand : string
    /// The x/y of the car in meters. (Tuple).
    Dimensions : float * float
    }

/// A vehicle of some sort.
type Vehicle =
    /// A car is a type of vehicle.
    | Motorcar of Car
    /// A bike is also a type of vehicle.
    | Motobike of Name : string * EngineSize : float
```

```csharp
using Model;

// Создание классов-подтипов Vehicle. Vehicle виден как abstract класс.
Car carItem = new Car(4, "Unknown", new Tuple<double, double>(1.7, 4));
Vehicle motorcar = Vehicle.NewMotorcar(carItem);
Vehicle motorbike = Vehicle.NewMotobike("Motor", 1.2);

// Использование motorcar.
if (motorcar.IsMotorcar)    // Проверка. IsMotorcar сгенеренное get-only свойство.
{
    var castedMotorcar = (Vehicle.Motorcar) motorcar;     // cast
    Car innerCar = castedMotorcar.Item;                   // get-only
    string innerCarBrand = innerCar.Brand;                // get-only
    int motorcarTag = castedMotorcar.Tag;                 // 0

// Использование motorbike.
if (motorbike.IsMotobike)    // Проверка. IsMotobike сгенеренное get-only свойство.
{
    var castedMotobike = (Vehicle.Motobike) motorbike;          // cast
    string motobikeName = castedMotobike.Name;                  // get-only
    double motobikeEngineSize = castedMotobike.EngineSize;      // get-only
    int motobikeTag = castedMotobike.Tag;                       // 1
}
```

### Using F# namespaces and modules in C#

#### F# namespaces in C#

Эквивалентны.

#### Modules in C#

* A module is rendered in C# as a static class.

* Any simple values on the module such as an integer or a record value will show
as a public property.

* Functions will show as methods on the static class.

* Types will show as nested classes within the static class.

### Using F# functions in C#

```fsharp
module Functions =
    /// Creates a car
    let createCar wheels brand x y =
        { Wheels = wheels; Brand = brand; Dimensions = x, y }

    /// Creates a car with four wheels. (Curried function).
    let createsFourWheeledCar = createCar 4
```

```csharp
using Model;

// Использование функции из F#.
// Calling a standard F# function from C#
var someCar = Functions.createCar(4, "SomeBrand", 1.5, 3.5);
// Calling a partially applied F# function from C#
var fourWheeledCar = Functions.createsFourWheeledCar
    .Invoke("Supacars")
    .Invoke(1.5)
    .Invoke(3.5);
```

*Recommendation*: try to avoid providing partially applied functions to C#.
If you absolutely must, wrap such functions in a "normal" F# function that
explicitly takes in all arguments required by the partially applied version,
and supplies those arguments manually.

### Summarizing F# to C# interoperability

| Element              | Render as                    | C# compatibility |
|----------------------|------------------------------|------------------|
| Records              | Immutable class              | High             |
| Tuples               | System.Tuple                 | Medium/high      |
| Discriminated unions | Classes with builder methods | Medium/low       |
| Namespaces           | Namespaces                   | High             |
| Modules              | Static classes               | High             |
| Functions            | Static methods               | High/medium      |

### F# incompatible types in C#

* Unit of measure

* Type providers

### Create an F# record from C# in an uninitialized state. (Use CLI Mutable attribute)

Сreate an F# record from C# in an uninitialized state.

Place the `[<CLIMutable>]` attribute on a record.

```fsharp
[<CLIMutable>]
type Person = {
    Name : string
    Age : int
    }
```

```csharp
// Создание и использование Record в неинициализированном состоянии.
var nonInitPerson = new Person();   // default сгенеренный конструктор
var name = nonInitPerson.Name;      // null
var age = nonInitPerson.Age;        // 0

var initPerson = new Person("Sam", 18);     // Второй сгенеренный конструктор
var knownName = initPerson.Name;
var knownAge = initPerson.Age;
```

### Using F# Option in C#

You can consume F# option types in C#. But as with other discriminated unions,
they're not particularly idiomatic to work with in C#.

Adding a few well-placed extension methods that remove the need for supplying type
arguments can help.

### Accessibility modifiers in F#

F# supports accessibility modifiers:

* `public`

* `private`

* `internal`

* All things are *public* by default in F#

* If you want to make a function or value hidden from C# code, mark it as `internal`.

### F# Collections in C#

* F# `array` is standard .NET array.

* F# `seq` appear as `IEnumerable<T>` in C# code.

* F# `list` not fully compatible with C#. In C# `list` can be used as `IEnumerable<T>`.

* C# `List` appear as `ResizeArray` in F# code.

*Advice*: avoid exposing F# `list` to C# clients.
