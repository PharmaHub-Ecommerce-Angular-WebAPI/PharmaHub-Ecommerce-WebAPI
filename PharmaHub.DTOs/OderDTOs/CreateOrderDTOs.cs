using PharmaHub.Domain.Entities;
using PharmaHub.Domain.Enums;

namespace PharmaHub.DTOs.OderDTOs;

public record class CreateOrderDTOs(
    Guid ID, 
    PaymentMethods PaymentMethod, 
    OrderStatus OrderStatus,
    string CustomerId, 
    ICollection<CreateOrderItemDTO> OrderItems)
{
}
