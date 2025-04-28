using System;

namespace CQRS.CRUD.Domain;

public class OrderItemDto
{
    public long Id { get; set; }
    public long OrderId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
