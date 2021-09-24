using Model;

namespace Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Users = new UserRepository(_context);
        }

        public IUserRepository Users { get; private set; }

        public int Complete()
        {
            return _context != null
                ? _context.SaveChanges()
                : 0;        // Ничего не делаем.
        }

        public void Dispose()
        {
            if (_context != null)
                _context.Dispose();
        }
    }
}