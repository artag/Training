using System;
using OperationResult;
using PrimitiveObsession.Common;

namespace PrimitiveObsession
{
    public class UserName : ValueObject<UserName>
    {
        private readonly string _value;

        private UserName(string value)
        {
            _value = value;
        }

        public static Result<UserName> Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            if (name.Trim().Length > 100)
                throw new ArgumentNullException(nameof(name));

            return Result.Ok(new UserName(name));
        }

        public static implicit operator string(UserName name)
        {
            return name._value;
        }

        public static explicit operator UserName(string name)
        {
            return Create(name).Value;
        }

        protected override bool EqualsCore(UserName other)
        {
            return _value == other._value;
        }

        protected override int GetHashCodeCore()
        {
            return _value.GetHashCode();
        }
    }
}
