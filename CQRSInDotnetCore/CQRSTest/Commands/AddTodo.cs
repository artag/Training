using System.Threading;
using System.Threading.Tasks;
using CQRSTest.Database;
using CQRSTest.Domain;
using MediatR;

namespace CQRSTest.Commands
{
    public static class AddTodo
    {
        public class Command : IRequest<int>
        {
            public string Name { get; }

            public Command(string name)
            {
                Name = name;
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly Repository _repository;

            public Handler(Repository repository)
            {
                _repository = repository;
            }

            public Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                _repository.Todos.Add(new Todo(10, request.Name, false));
                return Task.FromResult(10);
            }
        }
    }
}
