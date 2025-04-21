using PharmaHub.Domain.Entities;
using PharmaHub.Domain.Objects;
using PharmaHub.DTOs.OderDTOs;

namespace PharmaHub.Business.Contracts;

public interface IOrderManager
{
    Task<ProblemDetails?> CreateOrderAsync(CreateOrderDTOs orderDto);
    Task<OrderDetailsDto?> GetOrderDetailsAsync(Guid orderId);
    Task<Order?> GetOrderByIdAsync(Guid orderId);
}
