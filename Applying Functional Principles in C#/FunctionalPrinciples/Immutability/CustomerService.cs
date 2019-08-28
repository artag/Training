namespace Immutability
{
    public class CustomerService
    {
        public void Process(string customerName, string addressString)
        {
            var address = CreateAddress(addressString);
            var customer = CreateCustomer(customerName, address);
            SaveCustomer(customer);
        }

        private Address CreateAddress(string addressString)
        {
            return new Address(addressString);
        }

        private Customer CreateCustomer(string customerName, Address address)
        {
            return new Customer(customerName, address);
        }

        private void SaveCustomer(Customer customer)
        {
            var repository = new Repository();
            repository.Save(customer);
        }
    }
}
