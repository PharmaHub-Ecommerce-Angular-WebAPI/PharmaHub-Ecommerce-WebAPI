using PharmaHub.Domain.Entities;
using PharmaHub.DTOs.OderDTOs;

namespace PharmaHub.Business.Contracts;

public interface IOrderManager
{
    Task<OrderDetailsDto> CreateOrderAsync(CreateOrderDTOs orderDto);
    Task<OrderDetailsDto?> GetOrderDetailsAsync(Guid orderId);
}
