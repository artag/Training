using System;

namespace PrimitiveObsession
{
    public class UserService
    {
        public void ProcessUser(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(nameof(name));

            if (name.Trim().Length > 100)
                throw new ArgumentException(nameof(name));

            // Processing code
        }

        public void CreateUser(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(nameof(name));

            if (name.Trim().Length > 100)
                throw new ArgumentException(nameof(name));

            // Creation code
        }

        public void UpdateUser(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(nameof(name));

            if (name.Trim().Length > 100)
                throw new ArgumentException(nameof(name));

            // Update code
        }
    }
}
