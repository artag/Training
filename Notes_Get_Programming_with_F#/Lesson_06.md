# Lesson 06. Working with immutable data

F# по умолчанию ориентирован на работу с неизменяземыми данными

## Mutability basics in F#

Creating immutable values:

```fsharp
let name = "isaac"      // Creating an immutable value
name = "kate"           // the = operator represents equality (as == in C#)
name <- "kate"          // error FS0027: This value is not mutable
```

Creating a mutable variable:

```fsharp
let mutable name = "isaac"      // Defining a mutable variable
name <- "kate"                  // Assigning a new value to the variable
```

Working with mutable objects:

```fsharp
open System.Windows.Forms
let form = new Form()           // Creating the form object
form.Show()
form.Width <- 400               // Mutating the form by using the <- operator
form.Height <- 400
form.Text <- "Hello from F#!"
```

>Most objects in the BCL, such as a Form, are inherently mutable.
>Notice that the `form` symbol is immutable, so the binding symbol itself can't be changed.
>But the object it refers to is itself mutable, so properties on that object can be changed!

Shorthand (сокращенный вид) for creating mutable objects
(creating and mutating properties of a form in one expression):

```fsharp
open System.Windows.Forms
let form = new Form(Text = "Hello from F#!", Width = 300, Height = 300)
form.Show()
```

## Modeling state

### Working with mutable data

You create an object, and then modify its state through operations on that object.

### Working with immutable data

You can't mutate data. Instead, you create *copies* of the state with updates applied,
and return that for the caller to work with; that state may be passed in to other calls
that themselves generate new state.

Benefits:
* Код легче понимать.
  * No side effects.
  * Each method or function call can return a new version of the state.
  * Unit testing much easier.
* *Pure* functions can be called as many times as you want
  * Always give you the same result with the same input.
  * Can be cached or pregenerated.
  * Easier to test.
* The compiler is able to protect you in this case from accidentally misordering
function calls, because each function call is explicitly dependent on the output of
the previous call.

### Other benefits of immutable data

* Encapsulation становится почти не нужна: making your data read-only eliminates the need to
"hide" your data. Данные могут потребовать сокрытия только в публичных API.
* Don't need to worry about locks within a multi-threaded environment.
  * No any shared mutable state.
  * No race conditions.
  * Every thread can access the same piece of data as often as it likes, as it can never change.
