using System;
using CQRS.CRUD.Domain;
using CQRS.CRUD.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CQRS.CRUD.Application.Query.Order;

public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, List<OrderDto>>
{

    private readonly ProcurementDataContext _context;

    public GetAllOrdersQueryHandler(ProcurementDataContext context)
    {
        _context = context;
    }

    public async Task<List<OrderDto>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
    {
        return await _context.Order
            .Include(o => o.OrderItems)
            .Select(o => new OrderDto
            {
                Id = o.Id,
                CustomerName = o.CustomerName,
                OrderDate = o.OrderDate,
                OrderItems = o.OrderItems.Select(item => new OrderItemDto
                {
                    Id = item.Id,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    Price = item.Price
                }).ToList()
            }).ToListAsync(cancellationToken);
    }
}
