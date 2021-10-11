# Lesson 28. Architecting hybrid language applications

### Accepting data from external systems

```text
Unsafe   |                 F# World                 (Safe)
external |
systems  |                                        | Bounded Context
         |                     Validation         |
    C# --|--> Simple API ----> (Transformation) --|--> F#
         |                     Layer              |
```

It’s relatively simple to go from a weaker model to a stronger model.
At the entrance to your F# module, you accept the weak model,
but immediately validate and transform it over to your stronger model.

#### 1. A simple domain model for use within C# (on scheme: Simple API)

Set of implicit rules that are documented through code comment.
Weaker model.

```fsharp
type OrderItemRequest = { ItemId : int; Count : int }
type OrderRequest =
    { OrderId : int
      CustomerName : string    // mandatory
      Comment : string         // optional
      /// One of (email or telephone), or none
      EmailUpdates : string
      TelephoneUpdates : string
      Items : IEnumerable<OrderItemRequest> }
```

#### 2. Modeling the same domain in F# (on scheme: Bounded Context)

```fsharp
type OrderId = OrderId of int
type ItemId = ItemId of int
type OrderItem = { ItemId : ItemId; Count : int }
type UpdatePreference =
    | EmailUpdates of string
    | TelephoneUpdates of string
type Order =
    { OrderId : OrderId
      CustomerName : string
      ContactPreference : UpdatePreference option
      Comment : string option
      Items : OrderItem list }
```

#### 3. Validating and transforming data (on scheme: Validation/Transformation layer)

Perform checks before entering your "safe" F# world:

* Null check on a string.

* Convert from a string to an optional string.

* Confirm that the source request has a valid state;
if the incorrect mix of fields is populated, the request is rejected.

```fsharp
let validate orderRequest =
    { CustomerName =
          match orderRequest.CustomerName with
              | null -> failwith "Customer name must be populated"
              | name -> name
      Comment = orderRequest.Comment |> Option.ofObj
      ContactPreference =
          var emailUpd = Option.ofObj orderRequest.EmailUpdates
          var telephoneUpd = Option.ofObj orderRequest.TelephoneUpdates
          match emailUpd, telephoneUpd with
              | None, None -> None
              | Some email, None -> Some(EmailUpdates email)
              | None, Some phone -> Some(TelephoneUpdates phone)
              | Some _, Some _ ->
                  failwith "Unable to proceed - only one of telephone and email should be supplied" }
```

#### Working with strings in F#

* If string can be null:

  * Convert it to an option type by using `Option.ofObj`.

* If string shouldn't ever be null:

  * Check at the F# boundary.

  * Reject the object if it's null.

  * If it's not null, leave it as a string.

### C# interoperability example

Специальный класс в F#. Именно с экземплярами этого класса работает C#.

Особенности:

* Имена методов с большой буквы (как в коде C#).

* `CLIEvent` attribute, which is needed to expose events to the C# world.
Unfortunately, you can't have CLIEvents on modules (only on namespace).

```fsharp
namespace Monopoly

// Some modules using by Controller

/// Manages a game.
type Controller() =
    let onMovedEvent = new Event<MovementEvent>()
    
    (* A CLI Event is an event that can be consumed by e.g. C# *)
    /// Fired whenever a move occurs.
    [<CLIEvent>]
    member __.OnMoved = onMovedEvent.Publish
    
    /// Plays the game of Monopoly
    member __.PlayGame turnsToPlay =
        let random = Random()
        let rollDice = Functions.createDiceThrow random
        let pickCard = Functions.createPickCard random
        Functions.playGame rollDice pickCard onMovedEvent.Trigger turnsToPlay
```

#### Calling F# from a C# view mode

```csharp
// Creating an instance of the F# controller class
var controller = new Controller();
// Adding a standard event handler to capture game events
controller.OnMoved += (o, e) =>
    positionLookup[e.MovementData.CurrentPosition.ToString()Increment();
// Having the F# code play 50,000 turns on a background thread
Task.Run(() => controller.PlayGame(50000)
```

#### Using deterministic functions for exploration

```fsharp
let rollDice() = 3, 4       // Always roll 3, 4.
let pickCard() = 5          // Always pick card 5.
```
