namespace PharmaHub.DTOs.OderDTOs;

public class CreateOrderItemDTO
{
    public Guid ProductId { get; set; }
    public short Quantity { get; set; }
}
