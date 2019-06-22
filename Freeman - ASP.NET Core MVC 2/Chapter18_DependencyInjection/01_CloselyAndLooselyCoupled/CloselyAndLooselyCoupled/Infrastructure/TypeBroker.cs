using System;
using CloselyAndLooselyCoupled.Models;

namespace CloselyAndLooselyCoupled.Infrastructure
{
    public static class TypeBroker
    {
        private static Type _repositoryType = typeof(MemoryRepository);
        private static IRepository _repository;

        public static IRepository Repository =>
            _repository ?? Activator.CreateInstance(_repositoryType) as IRepository;

        public static void SetRepositoryType<T>() where T : IRepository =>
            _repositoryType = typeof(T);

        public static void SetTestObject(IRepository repository) =>
            _repository = repository;
    }
}
