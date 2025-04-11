using PharmaHub.DAL.Repositories.GenericRepository;
using PharmaHub.Domain.Entities;
using PharmaHub.Domain.Enums;

namespace PharmaHub.Domain.Infrastructure;


public interface IOrderRepository : IGenericRepository<Order>
{
    Task<IReadOnlyList<Order>> GetOrdersByUserIdAsync(Guid userId);
    Task<Order?> GetOrderWithDetailsAsync(Guid orderId);
    Task<IReadOnlyList<Order>> GetOrdersByStatusAsync(OrderStatus status);
}

