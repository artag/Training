using Ardalis.Specification.EntityFrameworkCore;
using Clean.Architecture.Entities.Abstractions.Interfaces;
using Clean.Architecture.Infrastructure.Interfaces;

namespace Clean.Architecture.Infrastructure.Data;

// inherit from Ardalis.Specification type
public class EfRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T> where T : class, IAggregateRoot
{
  public EfRepository(AppDbContext dbContext) : base(dbContext)
  {
  }
}
