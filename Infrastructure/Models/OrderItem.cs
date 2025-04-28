using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CQRS.CRUD.Infrastructure.Models;

public class OrderItem
{
    public long Id { get; set; }
    public long OrderId { get; set; }

    [Required]
    public string ProductName { get; set; } = string.Empty;
   
    [Required]
    public int Quantity { get; set; }
    
    [Required]
    public decimal Price { get; set; }

    [ForeignKey(nameof(OrderId))]
    public Order Order { get; set; } = new Order();
}
