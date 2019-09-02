namespace ErrorsAndFailures
{
    public class ClassToRefactor
    {
        private readonly IDatabase _database;
        private readonly IPaymentGateway _paymentGateway;

        public ClassToRefactor(IDatabase database, IPaymentGateway paymentGateway)
        {
            _database = database;
            _paymentGateway = paymentGateway;
        }

        public string RefillBalance(int customerId, decimal moneyAmount)
        {
            Customer customer = _database.GetById(customerId);
            customer.Balance += moneyAmount;
            _paymentGateway.ChargePayment(customer.BillingInfo, moneyAmount);
            _database.Save(customer);

            return "OK";
        }
    }
}
