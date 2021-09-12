# Lesson 8. Capstone 1

## Советы как начать разработку программы на F#

1. Use the REPL and a script file to explore various ideas.
2. Avoid mutation by default.
3. Favor expressions and pure functions over statements.
4. Starting small
    * Start by implementing small functions without worrying too much about *how* they'll
    be used later.
    * As long as the functions don't rely on any shared external state, they can be used just
    about anywhere without a problem.

## while, try-catch (try-with)

```fsharp
while true do
    try                                                 // (1)
        let destination = getDestination()              // (2)
        printfn "Trying to drive to %s" destination
        petrol <- driveTo petrol destination            // (3)
        printfn "Made it to %s! You have %d petrol left" destination petrol
    with ex -> printfn "ERROR: %s" ex.Message           // (4)
0                                                       // (5)

// (1) Start of a try/with exception-handling block
// (2) Get the destination from the user
// (3) Get updated petrol from core code and mutate state
// (4) Handle any exceptions
// (5) Return code
```

## failwith (Throw exception)

```fsharp
let getDistance destination =                   // Function definition
    if destination = "Gas" then 10              // (1)
    // some code here...
    else failwith "Unknown destination!"        // (2)

// (1) - Checking the destination and returning an int as an answer
// (2) - Throwing an exception if you can’t find a match
```

### Замечания по использованию try-catch, while и изменяемым значениям.

1. In general, functional programmers tend to shy away from (склонны избегать использования)
exceptions, especially as a way of managing control flow.

2. Using a `while` loop isn't exactly idiomatic F#. Using a `while` loop forces you to use a mutable variable.

3. In many applications, there will be a few places where mutation (or a side effect) is
difficult to get rid of (трудно избавиться) - and there's nothing inherently wrong
(нет ничего изначально неправильного) with that. What is important is that you try to restrict
(ограничить) mutation to just those places, and favor immutability everywhere else.
