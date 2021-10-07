# Lesson 21. Modeling relationships in F#

## Composition in F#

Моделируют отношение *имеет* (*has-a*) из ООП.

```fsharp
// Defining two record types - Computer is dependent on Disk
type Disk = { SizeGb : int }
type Computer =
    { Manufacturer : string
      Disks: Disk list }

// Creating an instance of a Computer
let myPc =
    { Manufacturer = "Computers Inc."
      Disks =
        [ { SizeGb = 100 }
          { SizeGb = 250 }
          { SizeGb = 500 } ] }
```

### Units of measure (UoM) in F#. Example

Specific type of integer: GB and MB, or meters and feet.

UoM не позволит смешать вместе несовместимые типы int (например, GB and MB).

```fsharp
type Disk = { Size : int<gb> }
```

```fsharp
[<Measure>] type kB

// Single case active pattern to convert from kB to raw Byte value
let (|Bytes|) (x : int<kB>) = int(x * 1024)

// Use pattern matching in the declaration
// val printBytes : int<kB> -> unit
let printBytes (Bytes(b)) = 
    printfn "It's %d bytes" b

printBytes 7<kB>
// "It's 7168 bytes"
```

* Units of measure (UoM) not needed often (but useful).

* UoMs can create a kind of "generic" numerics: so you can have `5<Kilogram>`
as opposed to `5<Meter>`. You can also combine types: `15<Meter/Second>` and so on.

* Compiler will prevent you from accidentally mixing and matching incompatible types.

* UoMs are erased away at compile time, so there’s no runtime overhead.

## Discriminated unions (DU) in F#

Unions моделируют отношение *является* (*is-a*) из ООП. Похожи на `enum` из C#, но с возможностью
добавления метаданных в каждый элемент enum'а.

```fsharp
// (1) Base type
// (2) Hard Disk subtype, containing two custom fields as metadata
// (3) SolidState - no custom fields
// (4) MMC - single custom field as metadata
type Disk =                             // (1)
| HardDisk of RPM:int * Platters:int    // (2)
| SolidState                            // (3)
| MMC of NumberOfPins:int               // (4)
```

* Each case is separated by the pipe symbol (`|`).
* To attach specific metadata to the case, you must separate each value with an asterisk (`*`).

В C# создание похожей структуры классов выглядело бы так:

1. Create a separate class for the base type and for each subclass.
2. Put each subclass into its own file.
3. Create a constructor for each, with appropriate fields and public properties.

### Creating discriminated unions in F#:

Creating an instance of a DU case is simple:

```fsharp
let instance = DUCase(arg1, arg2, argn)
```

```fsharp
let myHardDisk = HardDisk(RPM = 250, Platters = 7)  // Explicitly named arguments
let myHardDiskShort = HardDisk(250, 7)              // Lightweight syntax
let myMMC = MMC 5

// Passing all values as a single argument, can omit brackets
let args = 250, 7
let myHardDiskTupled = HardDisk args

// Creating a DU case without metadata 
let mySsd = SolidState
```

### Accessing an instance of a DU

Pattern matching позволяет безопасно *unwrap* тип `Disk` в три поддипа: `HardDisk`,
`SolidState` или `MMC`:

```fsharp
// (1) Matches on any type of hard disk
// (2) Matches on any type of MMC
let seek disk =
    match disk with
    | HardDisk _ -> "Seeking loudly at a reasonable speed!" // (1)
    | MMC _ -> "Seeking quietly but slowly"                 // (2)
    | SolidState -> "Already found it!"

let mySsd = SolidState
seek mySsd              // Returns “Already found it!”
```

Pattern matching on values:

```fsharp
// (1) Matching a hard disk with 5400 RPM and 5 spindles
// (2) Matching on a hard disk with 7 spindles and binding RPM for usage on the RHS of the case
// (3) Matching an MMC disk with 3 pins
let seek disk =
    match disk with
    | HardDisk(5400, 5) -> "Seeking very slowly!"                       // (1)
    | HardDisk(rpm, 7) -> sprintf "I have 7 spindles and RPM %d!" rpm   // (2)
    | MMC 3 -> "Seeking. I have 3 pins!"                                // (3)
```

Если какой-то подтип не будет описан в pattern matching, то компилятор выдаст warning.

## Tips for working with discriminated unions

### Nested DUs

Можно легко создавать nested (вложенные) discriminated unions - a type of a type.

```fsharp
// Nested DU with associated cases
type MMCDisk =
| RsMmc
| MmcPlus
| SecureMMC

// Adding the nested DU to your parent case in the Disk DU
type Disk =
// ... было ранее
| MMC of MMCDisk * NumberOfPins:int

// Matching on both top-level and nested DUs simultaneously
match disk with
// ... было ранее
| MMC(MmcPlus, 3) -> "Seeking quietly but slowly"
| MMC(SecureMMC, 6) -> "Seeking quietly with 6 pins."
```

### Shared fields in DUs (combination of records and discriminated unions)

```fsharp
// Composite record, starting with common fields
type DiskInfo = {
    Manufacturer : string   // Common field
    SizeGb : int            // Common field
    DiskData : Disk         // DU
}

// Computer record - contains manufacturer and a list of disks
type Computer = { Manufacturer : string; Disks : DiskInfo list }

// Creating a list of disks using [ ] syntax
// Common fields and varying DU as a Hard Disk
let myPc =
    { Manufacturer = "Computers Inc."
      Disks =
        [ { Manufacturer = "HardDisks Inc."
            SizeGb = 100
            DiskData = HardDisk(5400, 7) }
          { Manufacturer = "SuperDisks Corp."
            SizeGb = 250
            DiskData = SolidState } ] }
```

>### Active patterns
>
>F# has an even more powerful - and lightweight-mechanism for classification of data
>called active patterns. This is a more advanced topic, but I recommend that you check
>them out in your own time. You can think of them as discriminated unions on steroids.

### Printing out DUs

Print out the contents of a DU in a human-readable form

```fsharp
sprintf "%A"
```

###  Comparing OO hierarchies and discriminated unions

| -                         | Inheritanc           | Discriminated unions                     |
|---------------------------|----------------------|------------------------------------------|
| Usage                     | Heavyweight          | Lightweight                              |
| Complexity                | Hard to reason about | Easy to reason about                     |
| Extensibility             | Open set of types    | Closed set, compile-time, fixed location |
| Useful for plugin models? | Yes                  | No                                       |
| Add new subtypes          | Easy                 | Update all DU-related functions          |
| Add new methods           | Breaking change      | Easy                                     |

* **Not use** discriminated union for:

  * *Plugin models* - if you need to have an extensible set of open, pluggable subtypes that can
    be dynamically added. DU fixed at compile time, so you can’t plug in new items easily.
  * Unstable (or rapidly changing) extremely large hierarchies (cases). Every time you add a new
    case, your pattern matches over the DU will need to be updated to handle the new subtype.
    In such a case, either a record or raw functions might be a *better* fit, or falling back to
    a class-based inheritance model.

* **Use** discriminated union for:

  * Fixed (or slowly changing) set of cases.

DUs are lightweight, easy to work with, and very flexible, as you can add new behaviors extremely
quickly without affecting the rest of your code base and get the benefit of pattern matching.
All implementations in a single place leads to much-easier-to-understand code.

### Creating an enum in F#

```fsharp
type Printer =      // Enum type
| Injket = 0        // Enum case with explicit ordinal value
| Laserjet = 1
| DotMatrix = 2
```

В pattern matching любой элемент enum всегда можно привести к типу `int`, поэтому компилятор
будет выдавать warning и надо добавлять catchall wildcard handler (`_`).
