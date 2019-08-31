using System;
using System.Text.RegularExpressions;

namespace PrimitiveObsession
{
    public class Customer
    {
        public Customer(string name, string email)
        {
            ChangeName(name);
            ChangeEmail(email);
        }

        public string Name { get; private set; }

        public string Email { get; private set; }

        private void ChangeName(string name)
        {
            // Проблема 1. Code smell. Customer не должен проверять правильность name.
            // Проблема 2. Сигнатура этого метода dishonest.

            // Validate name
            if (string.IsNullOrWhiteSpace(name) || name.Trim().Length > 50)
                throw new ArgumentException("Name is invalid");

            Name = name;
        }

        private void ChangeEmail(string email)
        {
            // Проблема 1. Code smell. Customer не должен проверять правильность e-mail.
            // Проблема 2. Сигнатура этого метода dishonest.

            // Validate e-mail
            if (string.IsNullOrWhiteSpace(email) || email.Length > 100)
                throw new ArgumentException("E-mail is invalid");

            if (!Regex.IsMatch(email, @"^(.+)@(.+)$"))
                throw new ArgumentException("Email is invalid");

            Email = email;
        }
    }
}
