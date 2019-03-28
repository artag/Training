using System.Collections.Generic;
using System.Linq;

namespace Bfs
{
    public class Graph
    {
        private readonly Dictionary<Person, List<Person>> _graph = new Dictionary<Person, List<Person>>();
        private readonly HashSet<Person> _searched = new HashSet<Person>();
        private readonly Queue<Person> _searchQueue = new Queue<Person>();
        private readonly Person _nullPerson = new Person("Not finded");

        private readonly Person _i = new Person("I");

        public Graph()
        {
            var alice = new Person("Alice");
            var bob = new Person("Bob");
            var claire = new Person("Claire");
            var anuj = new Person("Anuj");
            var peggy = new Person("Peggy");
            var tom = new Person("Tom", isMangoSeller: true);
            var johnny = new Person("Johnny");

            _graph[_i] = new List<Person> { alice, bob, claire };
            _graph[bob] = new List<Person> { anuj, peggy };
            _graph[alice] = new List<Person> { peggy };
            _graph[claire] = new List<Person> { tom, johnny };
            _graph[anuj] = new List<Person>();
            _graph[peggy] = new List<Person>();
            _graph[tom] = new List<Person>();
            _graph[johnny] = new List<Person>();
        }

        public Person FindMangoSeller()
        {
            ClearState();
            return SearchPerson(Search.MangoSeller);
        }

        public Person FindRobot()
        {
            ClearState();
            return SearchPerson(Search.Robot);
        }

        private Person SearchPerson(Search searchCondition)
        {
            ClearState();

            while (_searchQueue.Any())
            {
                var person = _searchQueue.Dequeue();

                switch (searchCondition)
                {
                    case Search.MangoSeller when person.IsMangoSeller:
                        return person;
                    case Search.Robot when person.IsRobot:
                        return person;
                    default:
                        AddCoupledPeoples(person);
                        _searched.Add(person);
                        break;
                }
            }

            return _nullPerson;
        }

        private void ClearState()
        {
            _searchQueue.Clear();
            _searched.Clear();

            AddCoupledPeoples(_i);
        }

        private void AddCoupledPeoples(Person person)
        {
            var peoples = _graph[person];
            foreach (var people in peoples)
                if (!_searched.Contains(people))
                    _searchQueue.Enqueue(people);
        }

        private enum Search
        {
            MangoSeller,
            Robot
        }
    }
}
