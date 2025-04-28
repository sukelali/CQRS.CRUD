using System;

namespace CQRS.CRUD.Domain;

public class OrderDto
{

    public long Id { get; set; } = 0;
    public string CustomerName { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }

    public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>(); 
}
