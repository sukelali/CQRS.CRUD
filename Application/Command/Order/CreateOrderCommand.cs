using System;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace CQRS.CRUD.Application.Command.Order;

public record CreateOrderCommand : IRequest<long>
{
    [Required]
    public string CustomerName { get; set; } = string.Empty;
    public List<OrderItem> OrderItems { get; set; } = new(); 

    public record OrderItem
    {

        [Required]
        public string ProductName { get; set; } = string.Empty;
    
        [Required]
        public int Quantity { get; set; }
        
        [Required]
        public decimal Price { get; set; }
    }
}
