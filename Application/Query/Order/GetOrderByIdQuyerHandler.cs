using System;
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
}
