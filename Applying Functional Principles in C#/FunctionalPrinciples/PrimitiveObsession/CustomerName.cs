using OperationResult;
using PrimitiveObsession.Common;

namespace PrimitiveObsession
{
    public class CustomerName : ValueObject<CustomerName>
    {
        private readonly string _value;

        private CustomerName(string value)
        {
            _value = value;
        }

        public static Result<CustomerName> Create(string customerName)
        {
            if (string.IsNullOrWhiteSpace(customerName))
                return Result.Fail<CustomerName>("Customer name should not be empty");

            customerName = customerName.Trim();
            if (customerName.Length > 100)
                return Result.Fail<CustomerName>("Customer name is too long");

            return Result.Ok(new CustomerName(customerName));
        }

        public static implicit operator string(CustomerName customerName)
        {
            return customerName._value;
        }

        public static explicit operator CustomerName(string customerName)
        {
            return Create(customerName).Value;
        }

        protected override bool EqualsCore(CustomerName other)
        {
            return _value == other._value;
        }

        protected override int GetHashCodeCore()
        {
            return _value.GetHashCode();
        }
    }
}
