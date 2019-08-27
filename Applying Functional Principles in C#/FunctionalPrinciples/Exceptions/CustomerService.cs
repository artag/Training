using System.Linq;
using OperationResult;

namespace Exceptions
{
    public class CustomerService
    {
        public void CreateCustomer(string name)
        {
            var customer = new Customer(name);
            var result = SaveCustomer(customer);

            if (result.IsFailure)
            {
                MessageBox.Show(result.Error);
            }
        }

        private Result SaveCustomer(Customer customer)
        {
            try
            {
                using (var context = new MyContext())
                {
                    context.Customers.Add(customer);
                    context.SaveChanges();
                }

                return Result.Ok();
            }
            catch (DbUpdateException ex)
            {
                // Обрабатываем только те исключения, которые можем.
                if (ex.Message == "Unable to open the DB connection")
                    return Result.Fail("Database is off-line");

                if (ex.Message.Contains("IX_Customer_Name"))
                    return Result.Fail("Customer with such name already exists");
                    
                // Все остальные исключения - это аварийные случаи и ловятся только
                // на самом верхнем уровне.
                throw;
            }
        }

        private Result<Customer> GetCustomer(int id)
        {
            try
            {
                using (var context = new MyContext())
                {
                    return Result.Ok(context.Customers.Single(x => x.Id == id));
                }
            }
            catch (DbUpdateException ex)
            {
                if (ex.Message == "Unable to open the DB connection")
                {
                    return Result.Fail<Customer>("Database is off-line");
                }

                throw;
            }
        }
    }
}
