using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaHub.DTOs.ProductDTOs
{
    public class UpdateProductDto
    {
        public string ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public short Quantity { get; set; }
        public short Strength { get; set; }
    
        public UpdateProductDto(string productId, string name, string description, string imageUrl, decimal price, short quantity, short strength)
        {
            ProductId = productId;
            Name = name;
            Description = description;
            ImageUrl = imageUrl;
            Price = price;
            Quantity = quantity;
            Strength = strength;
        }
    }
}
