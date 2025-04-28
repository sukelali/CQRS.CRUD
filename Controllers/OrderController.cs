
using Microsoft.AspNetCore.Mvc;
using CQRS.CRUD.Application.Command.Order;
using MediatR;
using CQRS.CRUD.Application.Query.Order;

public class OrderController : Controller
{
    private readonly IMediator _mediator;

    public OrderController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET: ORDERS
    public async Task<IActionResult> Index()    
    {
        return View(await _mediator.Send(new GetAllOrdersQuery()));
    }

    // GET: ORDERS/Details/5
    public async Task<IActionResult> Details(GetOrderByIdQuery query)
    {
        if (query == null)
        {
            return NotFound();
        }

        var order = await _mediator.Send(query);
        
        if (order == null)
        {
            return NotFound();
        }

        return View(order);
    }
    

    // GET: ORDERS/Create
    public IActionResult Create()
    {
        var model = new CreateOrderCommand
        {
            OrderItems = new List<CreateOrderCommand.OrderItem>()
        };

        return View(model);
    }

    // POST: ORDERS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateOrderCommand order)
    {
        if (ModelState.IsValid)
        {
            var orderId = await _mediator.Send(order);
            return RedirectToAction(nameof(Index));
        }

        return View(order);
    }

   
}
