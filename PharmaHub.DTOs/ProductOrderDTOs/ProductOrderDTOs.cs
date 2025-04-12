using PharmaHub.Domain.Entities;

namespace PharmaHub.DTOs.ProductOrderDTOs;

public class ProductOrderDTOs
{
    public ProductOrderDTOs(ProductOrder productOrder)
    {
        ProductId = productOrder.ProductId;
        OrderId = productOrder.OrderId;
        Amount = productOrder.Amount;
    }
    public Guid ProductId { get; set; }
    public Guid OrderId { get; set; }
    public short Amount { get; set; } 

}
