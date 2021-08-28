using System.Threading.Tasks;
using MassTransit;
using Play.Catalog.Contracts;
using Play.Common;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Consumers
{
    public class CatalogItemCreatedConsumer : IConsumer<CatalogItemCreated>
    {
        private readonly IRepository<CatalogItem> _repository;

        public CatalogItemCreatedConsumer(IRepository<CatalogItem> repository)
        {
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<CatalogItemCreated> context)
        {
            // Полученное сообщение.
            var message = context.Message;

            // Проверка, было ли обработано сообщение ранее.
            var item = await _repository.GetAsync(message.ItemId);

            // Сообщение уже было обработано. Выход.
            if (item != null)
                return;

            // Создание и сохранение нового CatalogItem.
            item = new CatalogItem
            {
                Id = message.ItemId,
                Name = message.Name,
                Description = message.Description,
            };

            await _repository.CreateAsync(item);
        }
    }
}