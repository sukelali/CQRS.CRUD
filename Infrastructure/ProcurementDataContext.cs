using CQRS.CRUD.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace CQRS.CRUD.Infrastructure;

public class ProcurementDataContext(DbContextOptions<ProcurementDataContext> options) : DbContext(options)
{
    public DbSet<Order> Order { get; set; } = default!;
    
}
