open System;

let getDistance destination =
    if destination = "Gas" then 10
    elif destination = "Home" || destination = "Stadium" then 25
    elif destination = "Office" then 50
    else failwith "Unknown destination!"

getDistance "Gas"
getDistance "Home"
getDistance "Stadium"
getDistance "Office"
//getDistance "Unknown"

let distanceToGas = getDistance "Gas"
let distanceToHome = getDistance "Home"
let distanceToStadium = getDistance "Stadium"
let distanceToOffice = getDistance "Office"

let calculateRemainingPetrol currentPetrol distance =
    if currentPetrol > distance then currentPetrol - distance
    else failwith "Not enough petrol!"

calculateRemainingPetrol 25 distanceToGas
//calculateRemainingPetrol 5 distanceToGas

let driveTo currentPetrol destination =
    let distance = getDistance destination
    let remainingPetrol = calculateRemainingPetrol currentPetrol distance
    if destination = "Gas" then  remainingPetrol + 50
    else remainingPetrol

let a = driveTo 100 "Office"
let b = driveTo a "Stadium"
let c = driveTo b "Gas"
let answer = driveTo c "Home"