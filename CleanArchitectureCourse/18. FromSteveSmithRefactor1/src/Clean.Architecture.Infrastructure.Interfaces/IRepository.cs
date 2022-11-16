using Ardalis.Specification;
using Clean.Architecture.Entities.Abstractions.Interfaces;

namespace Clean.Architecture.Infrastructure.Interfaces;

// from Ardalis.Specification
public interface IRepository<T> : IRepositoryBase<T> where T : class, IAggregateRoot
{
}
