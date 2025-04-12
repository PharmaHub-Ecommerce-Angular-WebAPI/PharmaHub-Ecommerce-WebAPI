using Microsoft.EntityFrameworkCore;
using PharmaHub.DAL.Context;
using PharmaHub.DAL.Repositories.GenericRepositoryl;
using PharmaHub.Domain.Enums;
using PharmaHub.Domain.Infrastructure;

namespace PharmaHub.Domain.Entities;
/*
public class OrderRepository : GenericRepository<Order>, IOrderRepository
{
    public OrderRepository(ApplicationDbContext context) : base(context)
    {
    }

    // Get all orders for a specific user
    public async Task<IReadOnlyList<Order>> GetOrdersByUserIdAsync(Guid userId)
    {
        return await _dbSet
            .Where(o => o.ID == userId)
            .Include(o => o.ProductOrdersList) 
            .ThenInclude(oi => oi.Product) 
            .ToListAsync();
    }

    // Get full order details by order id
    public async Task<Order?> GetOrderWithDetailsAsync(Guid orderId)
    {
        return await _dbSet
            .Include(o => o.ProductOrdersList)
            .ThenInclude(po => po.Product) 
            .Include(o => o.Customer)
            .FirstOrDefaultAsync(o => o.ID == orderId);
    }

    // Get orders by status
    public async Task<IReadOnlyList<Order>> GetOrdersByStatusAsync(OrderStatus status)
    {
        return await _dbSet
            .Where(o => o.OrderStatus == status)
            .Include(o => o.ProductOrdersList)
            .ToListAsync();
    }
}*/
