using System.Collections.Generic;
using Model;

namespace Repository
{
    public interface IUserRepository : IRepository<User>
    {
        IEnumerable<User> GetByFirstName(string firstName);

        User GetOneByFirstName(string firstName);

        bool HasAny();

        int CountMatchingFirstName(string firstName);

        int GetMaximumUserId();

        int GetMinimumUserId();
    }
}
