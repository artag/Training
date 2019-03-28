namespace Bfs
{
    public class Person
    {
        public Person(string name, bool isMangoSeller = false)
        {
            Name = name;
            IsMangoSeller = isMangoSeller;
        }

        public string Name { get; }

        public bool IsMangoSeller { get; }

        public bool IsRobot { get; set; } = false;

        public override string ToString()
        {
            return Name;
        }
    }
}
