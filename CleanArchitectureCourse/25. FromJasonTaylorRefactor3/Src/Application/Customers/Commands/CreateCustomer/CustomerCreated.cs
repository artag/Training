using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Notification.Interfaces;

namespace Northwind.Application.Customers.Commands.CreateCustomer
{
    public class CustomerCreated : INotification
    {
        public string CustomerId { get; set; }

        public class CustomerCreatedHandler : INotificationHandler<CustomerCreated>
        {
            private readonly INotificationService _notification;

            public CustomerCreatedHandler(INotificationService notification)
            {
                _notification = notification;
            }

            public async Task Handle(CustomerCreated notification, CancellationToken cancellationToken)
            {
                await _notification.SendAsync(new MessageDto());
            }
        }
    }
}
