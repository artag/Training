using System;
using Model;

namespace CSharpApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var car = new Car(4, "Supacars", new Tuple<double, double>(1.5, 3.4));
            var brand = car.Brand;      // get-only
            var wheels = car.Wheels;    // get-only

            // Создание классов-подтипов Vehicle. Vehicle виден как abstract класс.
            Car carItem = new Car(4, "Unknown", new Tuple<double, double>(1.7, 4));
            Vehicle motorcar = Vehicle.NewMotorcar(carItem);
            Vehicle motorbike = Vehicle.NewMotobike("Motor", 1.2);

            // Использование motorcar.
            if (motorcar.IsMotorcar)
            {
                var castedMotorcar = (Vehicle.Motorcar) motorcar;     // cast
                Car innerCar = castedMotorcar.Item;                   // get-only
                string innerCarBrand = innerCar.Brand;                // get-only
                int motorcarTag = castedMotorcar.Tag;                 // 0
            }

            // Использование motorbike.
            if (motorbike.IsMotobike)
            {
                var castedMotobike = (Vehicle.Motobike) motorbike;          // cast
                string motobikeName = castedMotobike.Name;                  // get-only
                double motobikeEngineSize = castedMotobike.EngineSize;      // get-only
                int motobikeTag = castedMotobike.Tag;                       // 1
            }

            // Использование функции из F#.
            // Calling a standard F# function from C#
            var someCar = Functions.createCar(4, "SomeBrand", 1.5, 3.5);
            // Calling a partially applied F# function from C#
            var fourWheeledCar = Functions.createsFourWheeledCar
                .Invoke("Supacars")
                .Invoke(1.5)
                .Invoke(3.5);
            var br = fourWheeledCar.Brand;

            // Создание и использование Record в неинициализированном состоянии.
            var nonInitPerson = new Person();
            var name = nonInitPerson.Name;      // null
            var age = nonInitPerson.Age;        // 0

            var initPerson = new Person("Sam", 18);
            var knownName = initPerson.Name;
            var knownAge = initPerson.Age;
        }
    }
}
