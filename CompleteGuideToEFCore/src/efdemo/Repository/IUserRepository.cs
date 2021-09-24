using System.Collections.Generic;
using Model;

namespace Repository
{
    public interface IUserRepository : IRepository<User>
    {
        IEnumerable<User> GetByFirstName(string firstName);
    }
}
