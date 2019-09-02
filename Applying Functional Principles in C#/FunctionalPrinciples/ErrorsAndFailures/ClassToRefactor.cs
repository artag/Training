using System;

namespace ErrorsAndFailures
{
    public class ClassToRefactor
    {
        private readonly IDatabase _database;
        private readonly IPaymentGateway _paymentGateway;
        private readonly ILogger _logger;

        public ClassToRefactor(IDatabase database, IPaymentGateway paymentGateway, ILogger logger)
        {
            _database = database;
            _paymentGateway = paymentGateway;
            _logger = logger;
        }

        public string RefillBalance(int customerId, decimal moneyAmount)
        {
            if (!IsMoneyAmountValid(moneyAmount))
            {
                _logger.Log("Money amount is invalid");
                return "Money amount is invalid";
            }

            Customer customer = _database.GetById(customerId);
            if (customer == null)
            {
                _logger.Log("Customer is not found");
                return "Customer is not found";
            }

            customer.Balance += moneyAmount;

            try
            {
                _paymentGateway.ChargePayment(customer.BillingInfo, moneyAmount);
            }
            catch (ChargeFailedException ex)
            {
                _logger.Log("Unable to charge the credit card");
                return "Unable to charge the credit card";
            }

            try
            {
                _database.Save(customer);
            }
            catch (Exception e)
            {
                _paymentGateway.RollbackLastTransaction();
                _logger.Log("Unable to connect to the database");
                return "Unable to connect to the database";
            }

            _logger.Log("OK");
            return "OK";
        }

        private bool IsMoneyAmountValid(decimal moneyAmount)
        {
            throw new System.NotImplementedException();
        }
    }
}
