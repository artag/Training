namespace ErrorsAndFailures
{
    public interface IPaymentGateway
    {
        void RollbackLastTransaction();
        void ChargePayment(string billingInfo, decimal moneyToCharge);
    }
}
