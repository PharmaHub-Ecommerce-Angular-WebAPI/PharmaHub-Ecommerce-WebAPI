using PharmaHub.Domain.Entities;
using PharmaHub.Domain.Enums;

namespace PharmaHub.DTOs.OderDTOs;

public class CreateOrderDTOs
{
    public CreateOrderDTOs(Order entity)
    {
        ID = entity.ID;
        PaymentMethod = entity.PaymentMethod;
        OrderStatus = entity.OrderStatus;
        CustomerName = entity.Customer.UserName;
    }
    public Guid ID { get; set; }
    public PaymentMethods PaymentMethod { get; set; }
    public OrderStatus OrderStatus { get; set; } 
    public string? CustomerName { get; set; } 
}
