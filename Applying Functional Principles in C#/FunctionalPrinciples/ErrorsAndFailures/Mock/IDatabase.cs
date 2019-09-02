namespace ErrorsAndFailures
{
    public interface IDatabase
    {
        Customer GetById(int id);
        void Save(Customer customer);
    }
}
