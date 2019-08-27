using System;
using System.Linq;
using OperationResult;

namespace Exceptions
{
    public class CustomerService2
    {
        public void CreateCustomer(string name)
        {
            var customer = new Customer(name);
            var result = SaveCustomer(customer);

            switch (result.ErrorType)
            {
                case ErrorType.DatabaseIsOffline:
                    MessageBox.Show("Unable to connect to the database. Please try again later");
                    break;
                case ErrorType.CustomerAlreadyExists:
                    MessageBox.Show("A customer with the name " + name + " already exists");
                    break;
                default:
                    throw new ArgumentException();
            }
        }

        private ResultWithEnum SaveCustomer(Customer customer)
        {
            try
            {
                using (var context = new MyContext())
                {
                    context.Customers.Add(customer);
                    context.SaveChanges();
                }

                return ResultWithEnum.Ok();
            }
            catch (DbUpdateException ex)
            {
                // Обрабатываем только те исключения, которые можем.
                if (ex.Message == "Unable to open the DB connection")
                    return ResultWithEnum.Fail(ErrorType.DatabaseIsOffline);

                if (ex.Message.Contains("IX_Customer_Name"))
                    return ResultWithEnum.Fail(ErrorType.CustomerAlreadyExists);

                // Все остальные исключения - это аварийные случаи и ловятся только
                // на самом верхнем уровне.
                throw;
            }
        }

        private ResultWithEnum<Customer> GetCustomer(int id)
        {
            try
            {
                using (var context = new MyContext())
                {
                    return ResultWithEnum.Ok(context.Customers.Single(x => x.Id == id));
                }
            }
            catch (DbUpdateException ex)
            {
                if (ex.Message == "Unable to open the DB connection")
                {
                    return ResultWithEnum.Fail<Customer>(ErrorType.DatabaseIsOffline);
                }

                throw;
            }
        }
    }
}