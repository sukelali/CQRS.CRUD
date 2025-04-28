using System.ComponentModel.DataAnnotations;

namespace CQRS.CRUD.Infrastructure.Models;

public class Order
{
    public long Id { get; set; }

    [Required]
    public string CustomerName { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }

    public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>(); 
}
