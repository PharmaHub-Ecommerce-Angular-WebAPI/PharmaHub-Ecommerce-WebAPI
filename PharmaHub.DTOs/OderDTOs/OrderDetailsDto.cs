using PharmaHub.Domain.Entities;
using PharmaHub.Domain.Enums;

namespace PharmaHub.DTOs.OderDTOs;

public class OrderDetailsDto
{
    public Guid ID { get; set; }
    public string CustomerName { get; set; }
    public PaymentMethods PaymentMethod { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public DateTime CreatedAt { get; set; }
    public decimal TotalAmount { get; set; }
    public List<ProductOrderDTOs> Products { get; set; } = new();

    public OrderDetailsDto(Order order)
    {
        ID = order.ID;
        CustomerName = order.Customer?.UserName ?? "";
        PaymentMethod = order.PaymentMethod;
        OrderStatus = order.OrderStatus;
        CreatedAt = order.OrderDate;
        TotalAmount = order.ProductOrdersList.Sum(po => po.Amount);
        Products = order.ProductOrdersList.Select(po => new ProductOrderDTOs(po)).ToList();
    }
}
