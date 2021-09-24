using System;

namespace Repository
{
    public interface IUnitOfWork : IDisposable
    {
        // Подобные ссылки делаются для каждого репозитория entity.
        IUserRepository Users { get; }

        int Complete();
    }
}
