using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using Npgsql;

namespace PostgreSqlSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var rnd = new Random();
            var newEntries = new List<TestData>();
            for (int i = 2; i < 100; i++)
            {
                var testData = new TestData() { Id = i, Test = rnd.Next(2000).ToString() };
                newEntries.Add(testData);
            }

            Console.WriteLine("Generated 100 new Entries");

            try
            {
                // Info for connection from docker-compose.yml, section "postgresql-database".
                using(IDbConnection conn = new NpgsqlConnection("Host=postgresql-database;Username=admin;Password=adminadmin;Database=SampleDatabase"))
                {
                    var sqlInsert = "INSERT INTO TestData VALUES(@Id, @Test)";
                    conn.Execute(sqlInsert, newEntries);
                    Console.WriteLine("Saved the new Entries");

                    var sqlCount = "SELECT COUNT(*) FROM TestData";
                    var rowCount = conn.QueryFirstOrDefault<int>(sqlCount);
                    Console.WriteLine($"We have {rowCount} rows in the table");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.WriteLine("Done");
        }
    }

    public class TestData
    {
        public int Id { get; set; }

        public string Test { get; set; }
    }
}
