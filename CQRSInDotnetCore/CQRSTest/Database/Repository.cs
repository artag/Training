using CQRSTest.Domain;
using System.Collections.Generic;

namespace CQRSTest.Database
{
    public class Repository
    {
        public List<Todo> Todos { get; } = new List<Todo>
        {
            new Todo(1, "Cook dinner", completed: false),
            new Todo(2, "Make Youtube video", completed: true),
            new Todo(3, "Wash car", completed: false),
            new Todo(4, "Practice programming", completed: true),
            new Todo(5, "Take out garbage", completed: false)
        };
    }
}
