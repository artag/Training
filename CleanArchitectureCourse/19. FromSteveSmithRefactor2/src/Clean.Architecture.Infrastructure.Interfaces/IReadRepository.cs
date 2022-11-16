using Ardalis.Specification;
using Clean.Architecture.Entities.Abstractions.Interfaces;

namespace Clean.Architecture.Infrastructure.Interfaces;

public interface IReadRepository<T> : IReadRepositoryBase<T> where T : class, IAggregateRoot
{
}
