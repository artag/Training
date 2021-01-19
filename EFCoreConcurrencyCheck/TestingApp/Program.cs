using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TestingApp
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new AppDbContext())
            {
                AddNewEmployee(db, "Petya", "Russia");
                db.SaveChanges();
            }
            
            DisplayAllEmployees();

            var tasks = CreateConcurrencyTasks();
            tasks.ToList().ForEach(t => t.Start());
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine();

            DisplayAllEmployees();
        }

        static IEnumerable<Task> CreateConcurrencyTasks()
        {
            return new List<Task>
            {
                new Task(() => ProcessEmployee("Donald", "USA")),
                new Task(() => ProcessEmployee("Nicolas", "France")),
                new Task(() => ProcessEmployee("Raul", "Spain")),
                new Task(() => ProcessEmployee("Angela", "Germany")),
                new Task(() => ProcessEmployee("Lee","China")),
                new Task(() => ProcessEmployee("Kioto", "Japan")),
                new Task(() => ProcessEmployee("Donna Roza", "Brazil")),
                new Task(() => ProcessEmployee("Giovanni", "Italy")),
                new Task(() => ProcessEmployee("Givi", "Georgia")),
                new Task(() => ProcessEmployee("Lutz", "Austria")),
            };
        }

        static void ProcessEmployee(string name, string country)
        {
            using (var db = new AppDbContext())
            {
                var employee = db.Employees.FirstOrDefault(e => e.Country == "Russia");
                if (employee != null)
                {
                    employee.Country = country;
                    Console.WriteLine($"Change country: Russia -> {employee.Country}. From {name}");
                }
                else
                {
                    AddNewEmployee(db, name, country);
                }

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    ex.Entries.Single().Reload();
                    AddNewEmployee(db, name + " ***", country);
                    db.SaveChanges();
                }
            }
        }

        private static void AddNewEmployee(AppDbContext db, string name, string country)
        {
            var newEmployee = new Employee
            {
                Name = name,
                Country = country,
            };

            db.Employees.Add(newEmployee);

            Console.WriteLine($"Added the new employee: {newEmployee}");
        }

        static void DisplayAllEmployees()
        {
            Console.WriteLine();
            Console.WriteLine("Employees:");

            using (var db = new AppDbContext())
            {
                var employees = db.Employees.Select(e => e).OrderBy(e => e.Id);
                employees.ToList().ForEach(employee => Console.WriteLine(employee));
            }

            Console.WriteLine();
        }
    }
}
