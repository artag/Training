namespace Model

/// A standard F# record of a Car.
type Car = {
    /// The number of wheels on the car.
    Wheels : int
    /// The brand of the car.
    Brand : string
    /// The x/y of the car in meters.
    Dimensions : float * float
    }

/// A vehicle of some sort.
type Vehicle =
    /// A car is a type of vehicle.
    | Motorcar of Car
    /// A bike is also a type of vehicle.
    | Motobike of Name : string * EngineSize : float

module Functions =
    /// Creates a car
    let createCar wheels brand x y =
        { Wheels = wheels; Brand = brand; Dimensions = x, y }

    /// Creates a car with four wheels. (Curried function).
    let createsFourWheeledCar = createCar 4

[<CLIMutable>]
type Person = {
    Name : string
    Age : int
    }