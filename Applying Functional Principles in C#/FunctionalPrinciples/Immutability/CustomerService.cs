namespace Immutability
{
    public class CustomerService
    {
        private Address _address;
        private Customer _customer;

        public void Process(string customerName, string addressString)
        {
            // Проблема: можно вызвать методы в другом порядке.
            CreateAddress(addressString);
            CreateCustomer(customerName);
            SaveCustomer();
        }

        // Проблема: метод изнутри меняет поле _address.
        private void CreateAddress(string addressString)
        {
            _address = new Address(addressString);
        }

        // Проблема: метод изнутри меняет поле _customer.
        private void CreateCustomer(string customerName)
        {
            _customer = new Customer(customerName, _address);
        }

        private void SaveCustomer()
        {
            var repository = new Repository();
            repository.Save(_customer);
        }
    }
}
