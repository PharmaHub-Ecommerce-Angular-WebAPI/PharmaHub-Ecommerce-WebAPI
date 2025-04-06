using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PharmaHub.Domain.Enums;



namespace PharmaHub.Domain.Entities
{
    public class Order
    {
        public Guid ID { get; set; } = Guid.NewGuid();
       public DateTime OrderDate { get; set; } = DateTime.Now;

       [Required]
       public PaymentMethods PaymentMethod { get; set; }

    }
}
