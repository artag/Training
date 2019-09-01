using Nulls.Common;

namespace Nulls
{
    public interface IDatabase
    {
        void Save(Customer customer);

        Maybe<Customer> GetById(int id);
    }
}
