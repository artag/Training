[<Measure>] type kB

// Single case active pattern to convert from kB to raw Byte value
let (|Bytes|) (x : int<kB>) = int(x * 1024)

// Use pattern matching in the declaration
// val printBytes : int<kB> -> unit
let printBytes (Bytes(b)) = 
    printfn "It's %d bytes" b

printBytes 7<kB>
// "It's 7168 bytes"