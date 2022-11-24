using MediatR;
using Northwind.Application.Common.Exceptions;
using Northwind.Domain.Entities;
using Persistence.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Northwind.Application.Categories.Commands.DeleteCategory
{
    public class DeleteCategoryCommandHandler : AsyncRequestHandler<DeleteCategoryCommand>
    {
        private readonly INorthwindDbContext _context;

        public DeleteCategoryCommandHandler(INorthwindDbContext context)
        {
            _context = context;
        }

        protected override async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Categories
                .FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Category), request.Id);
            }

            _context.Categories.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
