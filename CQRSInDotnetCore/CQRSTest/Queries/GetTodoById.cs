using System.Linq;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using CQRSTest.Database;

namespace CQRSTest.Queries
{
    public static class GetTodoById
    {
        public class Query : IRequest<Response>
        {
            public Query(int id)
            {
                Id = id;
            }

            public int Id { get; }
        }

        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly Repository _repository;

            public Handler(Repository repository)
            {
                _repository = repository;
            }

            public Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var todo = _repository.Todos.FirstOrDefault(x => x.Id == request.Id);
                var result = todo == null ? null : new Response(todo.Id, todo.Name, todo.Completed);
                return Task.FromResult(result);
            }
        }

        public class Response
        {
            public Response(int id, string name, bool completed)
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
}
