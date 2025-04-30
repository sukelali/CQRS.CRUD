## A Brief History of CQRS

The **CQRS (Command Query Responsibility Segregation)** architectural pattern was formally introduced by **Greg Young in 2010**. Its conceptual roots, however, come from **Bertrand Meyer’s Command-Query Separation principle**, which was first introduced in his 1988 book *Object-Oriented Software Construction*.

---

## What is CQRS?

CQRS stands for **Command Query Responsibility Segregation**. At its core, the idea is to **separate read and write operations** into different models:

- **Command:** Used to **update** or **change** the system state.
- **Query:** Used to **read** data without modifying the state.

This separation allows you to "slice" your application vertically, making it easier to scale, maintain, and reason about.

According to Bertrand Meyer:
- A **Command** (procedure) does something but does **not return a result**.
- A **Query** (function) returns a result but does **not change the state**.

---

## Implementing CQRS in an ASP.NET MVC Application

In this example, we'll implement CQRS using **MediatR** in an ASP.NET Core project. We'll create a simple **Order Service** with a `CreateOrderCommand` and a query to retrieve order data.

---

### CreateOrderCommand

```csharp
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
```

---

### CreateOrderCommandHandler

```csharp
using CQRS.CRUD.Infrastructure;
using CQRS.CRUD.Infrastructure.Models;
using MediatR;

using OrderModel = CQRS.CRUD.Infrastructure.Models.Order;

namespace CQRS.CRUD.Application.Command.Order;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, long>
{
    private readonly ProcurementDataContext _context;

    public CreateOrderCommandHandler(ProcurementDataContext context)
    {
        _context = context;
    }

    public async Task<long> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = new OrderModel
        {
            CustomerName = request.CustomerName,
            OrderDate = DateTime.UtcNow,
            OrderItems = request.OrderItems.Select(item => new OrderItem
            {
                ProductName = item.ProductName,
                Quantity = item.Quantity,
                Price = item.Price
            }).ToList()
        };

        await _context.Order.AddAsync(order, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return order.Id;
    }
}
```

---

### Query to Get Order Info

Let’s create a query to retrieve order information.

```csharp
using CQRS.CRUD.Infrastructure.Models;
using MediatR;

namespace CQRS.CRUD.Application.Query.Order;

public record GetOrderByIdQuery(long OrderId) : IRequest<OrderDto?>;
```

---

### Handler for GetOrderByIdQuery

```csharp
using CQRS.CRUD.Domain;
using CQRS.CRUD.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CQRS.CRUD.Application.Query.Order;

public class GetOrderByIdQuyerHandler : IRequestHandler<GetOrderByIdQuery, OrderDto?>
{
    private readonly ProcurementDataContext _context;

    public GetOrderByIdQuyerHandler(ProcurementDataContext context)
    {
        _context = context;
    }

    public async Task<OrderDto?> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _context.Order
                        .Include(o => o.OrderItems)
                        .FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);

        return order == null ? null : new OrderDto
        {
            Id = order.Id,
            CustomerName = order.CustomerName,
            OrderDate = order.OrderDate,
            OrderItems = order.OrderItems.Select(item => new OrderItemDto
            {
                Id = item.Id,
                ProductName = item.ProductName,
                Quantity = item.Quantity,
                Price = item.Price
            }).ToList()
        };

    }
```

---
## ⚠️ When to Use CQRS — Scenarios & Pitfalls

While CQRS offers powerful architectural benefits, it's **not a silver bullet**. You should be aware of both **where it's useful** and **where it can introduce complexity**.

### ✅ Recommended Scenarios

- **Complex Domains:** CQRS shines in applications with complex business logic, workflows, or where data models for reads and writes are significantly different.
- **High Read/Write Load:** When read and write operations need to scale independently.
- **Event Sourcing:** If you're adopting event sourcing, CQRS fits naturally since commands result in events, and queries read from projections.

### ❌ Common Pitfalls

1. **Overengineering Simple Applications:**
   - Applying CQRS to a CRUD-heavy or small app often adds unnecessary complexity.
   - You may end up maintaining two models without gaining the benefits of separation.

2. **Data Consistency Challenges:**
   - In distributed systems or async processing, there's a chance for **eventual consistency** issues between the write and read models.
   - You’ll need to carefully handle synchronization and consistency.

3. **Code Duplication:**
   - Sometimes, the logic in commands and queries overlaps, especially in smaller apps.
   - Maintaining both can be harder if your team isn’t experienced with separation of concerns.

4. **Steep Learning Curve:**
   - CQRS often goes hand-in-hand with DDD, event sourcing, and messaging — which might be overwhelming for teams new to these concepts.

5. **Tooling & Testing Complexity:**
   - You’ll need to test commands and queries separately.
   - If you're using MediatR, mocking handlers for testing can become boilerplate-heavy.

---

### 🧠 Tip: Start Simple

If you're unsure whether to go full CQRS, try implementing **CQRS-lite**:
- Use separate command/query handlers, but **share the same data model** initially.
- Later, evolve to independent models or persistence layers only when needed.

---


## Conclusion

CQRS is a powerful pattern for applications that have complex business logic or need high scalability. Using libraries like **MediatR**, implementing CQRS in ASP.NET Core becomes much simpler and cleaner.

If you don't want to use MediatR, you can still implement basic CQRS manually. Microsoft provides a helpful guide on this: [CQRS Pattern - Microsoft Learn](https://learn.microsoft.com/en-us/azure/architecture/patterns/cqrs)

---

Let me know if you want help building the read side with DTOs, projections, or applying event sourcing concepts too.
