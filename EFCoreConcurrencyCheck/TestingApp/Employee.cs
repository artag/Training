using System.ComponentModel.DataAnnotations;

namespace TestingApp
{
    public class Employee
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [ConcurrencyCheck]
        public string Country { get; set; }

        public override string ToString()
        {
            return $"Id = {Id}, Name = {Name}, Country = {Country}";
        }
    }
}
