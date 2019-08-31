using System;

namespace PrimitiveObsession
{
    public class UserService
    {
        public void ProcessUser(UserName name)
        {
            if (name == null)
                throw new ArgumentException(nameof(name));

            // Processing code
        }

        public void CreateUser(UserName name)
        {
            if (name == null)
                throw new ArgumentException(nameof(name));

            // Creation code
        }

        public void UpdateUser(UserName name)
        {
            if (name == null)
                throw new ArgumentException(nameof(name));

            // Update code
        }
    }
}
