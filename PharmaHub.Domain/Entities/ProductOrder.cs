using System.ComponentModel.DataAnnotations;

namespace PharmaHub.Domain.Entities;

public class ProductOrder
{
    public Guid ProductId { get; set; }
    public Product Product { get; set; }
    public Guid OrderId { get; set; }
    public Order Order { get; set; }

    [Range(1, short.MaxValue, ErrorMessage = "Strength must be zero or more.")]
    public short Amount { get; set; } = 1;

}
