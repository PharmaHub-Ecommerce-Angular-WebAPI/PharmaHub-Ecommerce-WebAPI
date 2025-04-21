using System.ComponentModel.DataAnnotations;

namespace PharmaHub.Domain.Entities
{
    public class PackagesComponent
    {
        //will have composite key from Product id (forgien key) and ProductName

        [Required] public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public string ComponentName { get; set; }



    }
}
