using System;

namespace Exceptions
{
    // Внутренний класс. Здесь валидация уже не требуется.
    public class Employee
    {
        public void UpdateName(string name)
        {
            // Эта ситуация уже нестандартная (аварийная) - выбрасывается исключение,
            // которое ловится на самом верхнем уровне.
            if (name == null)
                throw new ArgumentNullException();

            // ...
        }
    }
}
