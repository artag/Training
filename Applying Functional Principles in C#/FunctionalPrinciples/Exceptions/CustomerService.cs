using System;

namespace Exceptions
{
    public class CustomerService
    {
        public void CreateCustomer(string name)
        {
            var customer = new Customer(name);
            bool result = SaveCustomer(customer);

            if (!result)
            {
                MessageBox.Show("Error connecting to the database. Please try again later.");
            }
        }

        private bool SaveCustomer(Customer customer)
        {
            try
            {
                using (var context = new MyContext())
                {
                    context.Customers.Add(customer);
                    context.SaveChanges();
                }

                return true;
            }
            catch (DbUpdateException ex)
            {
                return false;
            }
        }
    }
}
