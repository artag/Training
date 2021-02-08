type Disk =
| HardDisk of RPM:int * Platters:int
| SolidState
| MMC of NumberOfPins:int

let describe disk =
    match disk with
    | SolidState -> printfn "I'm a newfangled SSD."
    | MMC 1 -> printfn "I have only 1 pin."
    | MMC pins when pins < 5 -> printfn "I’m an MMC with a few pins."
    | MMC pins -> printfn "I’m an MMC with %i pins." pins
    | HardDisk(5400, _) -> printfn "I’m a slow hard disk."
    | HardDisk(_, 7) -> printfn "I have 7 spindles!"
    | HardDisk(_,_) -> printfn "“I’m a hard disk."

describe SolidState
describe (MMC 1)
describe (MMC 2)
describe (MMC 6)
describe (HardDisk(5400, 2))
describe (HardDisk(5400, 7))
describe (HardDisk(7200, 2))
describe (HardDisk(7200, 7))
