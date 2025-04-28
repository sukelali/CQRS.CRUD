using System;
using CQRS.CRUD.Domain;
using MediatR;

namespace CQRS.CRUD.Application.Query.Order;

public record GetOrderByIdQuery : IRequest<OrderDto?>
{
    public long Id { get; set; } = 0;
}
