using PharmaHub.Domain.Entities;

namespace PharmaHub.DTOs;

public class ProductOrderDTOs
{
    public Guid ProductId { get; set; }
    public string Name { get; set; }
    public short Amount { get; set; }

    public ProductOrderDTOs(ProductOrder po)
    {
        ProductId = po.ProductId;
        Name = po.Product?.Name ?? "";
        Amount =po.Amount;
    }
}
