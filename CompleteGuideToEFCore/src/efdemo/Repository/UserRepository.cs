using System.Collections.Generic;
using System.Linq;
using Model;

namespace Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public ApplicationDbContext ApplicationDbContext =>
            Context as ApplicationDbContext;

        public IEnumerable<User> GetByFirstName(string firstName)
        {
            return ApplicationDbContext.Users.Where(u => u.FirstName == firstName);
        }
    }
}
