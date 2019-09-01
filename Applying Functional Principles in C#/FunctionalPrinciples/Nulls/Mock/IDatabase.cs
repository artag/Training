namespace Nulls
{
    public interface IDatabase
    {
        void Save(Customer customer);

        Customer GetById(int id);
    }
}
