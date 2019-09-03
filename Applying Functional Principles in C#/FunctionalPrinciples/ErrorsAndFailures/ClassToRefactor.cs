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
                

            // Refactoring.
            // 1. Перемещение try-catch блока на более низкий уровень (уровень ChargePayment).
            // 2. Возврат Result вместо выброса исключения.
            Result chargeResult = _paymentGateway.ChargePayment(customer.Value.BillingInfo, moneyToCharge.Value);

            if (chargeResult.IsFailure)
            {
                _logger.Log(chargeResult.Error);
                return chargeResult.Error;
            }

            // Refactoring.
            // 1. Перемещение try-catch блока на более низкий уровень (уровень Save).
            // 2. Возврат Result вместо выброса исключения.
            Result saveResult = _database.Save(customer.Value);
            if (saveResult.IsFailure)
            {
                _paymentGateway.RollbackLastTransaction();
                _logger.Log(saveResult.Error);
                return saveResult.Error;
            }

            _logger.Log("OK");
            return "OK";
        }
    }
}
