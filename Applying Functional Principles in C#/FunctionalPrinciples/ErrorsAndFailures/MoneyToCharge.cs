using OperationResult;
using PrimitiveObsession.Common;

namespace ErrorsAndFailures
{
    public class MoneyToCharge : ValueObject<MoneyToCharge>
    {
        private MoneyToCharge(decimal value)
        {
            Value = value;
        }

        public decimal Value { get; }

        public static Result<MoneyToCharge> Create(decimal moneyToCharge)
        {
            if (moneyToCharge <= 0 || moneyToCharge > 1000)
                return Result.Fail<MoneyToCharge>("Money amount is invalid");

            return Result.Ok(new MoneyToCharge(moneyToCharge));
        }

        protected override bool EqualsCore(MoneyToCharge other)
        {
            return Value == other.Value;
        }

        protected override int GetHashCodeCore()
        {
            return Value.GetHashCode();
        }

        public static implicit operator decimal(MoneyToCharge moneyToCharge)
        {
            return moneyToCharge.Value;
        }

        public static explicit operator MoneyToCharge(decimal moneyToCharge)
        {
            return Create(moneyToCharge).Value;
        }
    }
}
