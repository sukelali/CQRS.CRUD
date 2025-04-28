using System;
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
