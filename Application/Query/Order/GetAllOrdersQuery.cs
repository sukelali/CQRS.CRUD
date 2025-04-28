using System;
using CQRS.CRUD.Domain;
using MediatR;

namespace CQRS.CRUD.Application.Query.Order;

public class GetAllOrdersQuery : IRequest<List<OrderDto>>
{
    
}
