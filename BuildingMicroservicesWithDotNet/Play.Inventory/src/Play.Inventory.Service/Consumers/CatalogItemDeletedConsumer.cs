using System.Threading.Tasks;
using MassTransit;
using Play.Catalog.Contracts;
using Play.Common;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Consumers
{
    public class CatalogItemDeletedConsumer : IConsumer<CatalogItemDeleted>
    {
        private readonly IRepository<CatalogItem> _repository;

        public CatalogItemDeletedConsumer(IRepository<CatalogItem> repository)
        {
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<CatalogItemDeleted> context)
        {
            // Полученное сообщение.
            var message = context.Message;

            // Проверка, есть ли в репозитории нужный CatalogItem.
            var item = await _repository.GetAsync(message.ItemId);

            // CatalogItem не найден. Выход.
            if (item != null)
                return;

            // Удаление CatalogItem из репозитория.
            await _repository.RemoveAsync(message.ItemId);
        }
    }
}