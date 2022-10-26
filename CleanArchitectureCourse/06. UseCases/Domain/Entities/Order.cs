using System;
using System.Collections.Generic;
using Domain.Enums;

namespace Domain.Entities;

public class Order
{
    public int Id { get; set; }
    public DateTime CreateDate { get; set; }
    public OrderStatus Status { get; set; }

    public ICollection<OrderItem> Items { get; set; }
}
