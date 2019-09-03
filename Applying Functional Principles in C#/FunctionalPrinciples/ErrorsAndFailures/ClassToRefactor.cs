using System;
using Nulls.Common;
using OperationResult;

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
            Result<MoneyToCharge> moneyToCharge = MoneyToCharge.Create(moneyAmount);
            Result<Customer> customer = _database.GetById(customerId).ToResult("Customer is not found");

            Result.Combine(moneyToCharge, customer)
                .OnSuccess(() => customer.Value.AddBalance(moneyToCharge.Value))
                .OnSuccess(() => _paymentGateway.ChargePayment(customer.Value.BillingInfo, moneyToCharge.Value))
                .OnSuccess(
                    () => _database.Save(customer.Value)
                        .OnFailure(() => _paymentGateway.RollbackLastTransaction()))
                

            _logger.Log("OK");
            return "OK";
        }
    }
}
