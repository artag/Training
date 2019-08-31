using OperationResult;
using PrimitiveObsession.Common;

namespace PrimitiveObsession
{
    public class Email : ValueObject<Email>
    {
        private readonly string _value;

        private Email(string value)
        {
            _value = value;
        }

        public static Result<Email> Create(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return Result.Fail<Email>("Email should not be empty");

            email = email.Trim();
            if (email.Length > 256)
                return Result.Fail<Email>("Email is too long");

            if (!email.Contains("@"))
                return Result.Fail<Email>("Email is invalid");

            return Result.Ok(new Email(email));
        }

        /// <summary>
        /// Безопасное неявное преобразование Email -> string.
        /// </summary>
        /// <param name="email">Адрес email.</param>
        /// <code>
        /// <example>
        /// Email email = GetEmail();
        /// string emailString = email;
        /// </example>
        /// </code>
        public static implicit operator string(Email email)
        {
            return email._value;
        }

        /// <summary>
        /// Небезопасное явное преобразование string -> Email.
        /// </summary>
        /// <param name="email">Адрес email.</param>
        /// <remarks>Если преобразование нельзя выполнить, конвертер кинет исключение.</remarks>
        /// <code>
        /// <example>
        /// string emailString = GetEmail();
        /// Email email = (Email)emailString;
        /// </example>
        /// </code>
        public static explicit operator Email(string email)
        {
            return Create(email).Value;
        }

        protected override bool EqualsCore(Email other)
        {
            return _value == other._value;
        }

        protected override int GetHashCodeCore()
        {
            return _value.GetHashCode();
        }
    }
}
