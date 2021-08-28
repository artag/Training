using System.Threading.Tasks;
using MassTransit;
using Play.Catalog.Contracts;
using Play.Common;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Consumers
{
    public class CatalogItemUpdatedConsumer : IConsumer<CatalogItemUpdated>
    {
        private readonly IRepository<CatalogItem> _repository;

        public CatalogItemUpdatedConsumer(IRepository<CatalogItem> repository)
        {
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<CatalogItemUpdated> context)
        {
            // Полученное сообщение.
            var message = context.Message;

            // Проверка, есть ли в репозитории CatalogItem.
            var item = await _repository.GetAsync(message.ItemId);

            if (item == null)
            {
                // CatalogItem нет в репозитории. Создаем и сохраняем новый CatalogItem.
                item = new CatalogItem
                {
                    Id = message.ItemId,
                    Name = message.Name,
                    Description = message.Description,
                };

                await _repository.CreateAsync(item);
            }
            else
            {
                // CatalogItem есть в репозитории. Обновляем и сохраняем CatalogItem.
                item.Name = message.Name;
                item.Description = message.Description;

                await _repository.UpdateAsync(item);
            }
        }
    }
}