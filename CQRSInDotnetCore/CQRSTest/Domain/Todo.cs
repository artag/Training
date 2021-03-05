namespace CQRSTest.Domain
{
    public class Todo
    {
        public Todo(int id, string name, bool completed)
        {
            Id = id;
            Name = name;
            Completed = completed;
        }

        public int Id { get; }
        public string Name { get; }
        public bool Completed { get; }
    }
}
