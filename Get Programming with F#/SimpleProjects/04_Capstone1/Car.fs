module Car

let getDistance destination =
    if destination = "Gas" then 10
    elif destination = "Home" || destination = "Stadium" then 25
    elif destination = "Office" then 50
    else failwith "Unknown destination!"

let calculateRemainingPetrol currentPetrol distance =
    if currentPetrol > distance then currentPetrol - distance
    else failwith "Not enough petrol!"

let driveTo currentPetrol destination =
    let distance = getDistance destination
    let remainingPetrol = calculateRemainingPetrol currentPetrol distance
    if destination = "Gas" then  remainingPetrol + 50
    else remainingPetrol