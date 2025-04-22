using PharmaHub.DTOs.OderDTOs;
using PharmaHub.Domain.Enums;

namespace PharmaHub.Presentation.ActionRequest.Order
{
    public class CreateOrderActionRequest
    {
        public string CustomerId { get; set; } 
        public PaymentMethods PaymentMethod { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public string PaymentToken { get; set; }

        // ProductId -> Amount
        public Dictionary<Guid,short> ProductAmounts { get; set; }

        public CreateOrderDTOs ToDto()
        {
            var orderItems = ProductAmounts.Select(pa => new CreateOrderItemDTO
            {
                ProductId = pa.Key,
                Quantity = pa.Value
            }).ToList();

            return new CreateOrderDTOs(
                ID: Guid.NewGuid(),
                PaymentMethod: PaymentMethod,
                OrderStatus: OrderStatus,
                CustomerId: CustomerId,
                OrderItems: orderItems
            );
        }
    }
}
