using PharmaHub.Domain.Entities.Identity;
using PharmaHub.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace PharmaHub.Domain.Entities
{
    public class Order
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Required]
        public PaymentMethods PaymentMethod { get; set; }
        [Required]
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;


        // Relationships
        public ICollection<ProductOrder> ProductOrdersList { get; set; } = new List<ProductOrder>();

        public String CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
