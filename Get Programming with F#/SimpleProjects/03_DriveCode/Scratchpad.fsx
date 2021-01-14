open System

//let drive petrol distance =
//    if distance = "far" then petrol / 2.0
//    elif distance = "medium" then petrol - 10.0
//    else petrol - 1.0

//let petrol = 100.0
//let firstState = drive petrol "far"
//let secondState = drive firstState "medium"
//let finalState = drive secondState "short"

let drive2 petrol distance =
    if distance > 50 then petrol / 2.0
    elif distance > 25 then petrol - 10.0
    elif distance > 0 then petrol - 1.0
    else petrol

let petrol = 100.0
let firstState = drive2 petrol 51
let secondState = drive2 firstState 26
let thirdState = drive2 secondState 1
let finalState = drive2 thirdState 0
