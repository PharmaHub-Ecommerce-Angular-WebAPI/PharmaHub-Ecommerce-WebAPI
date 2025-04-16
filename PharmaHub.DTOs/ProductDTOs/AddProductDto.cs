using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaHub.DTOs.ProductDTOs
{
    public class AddProductDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public decimal Price { get; set; }
        public short Quantity { get; set; }
        public short? Strength { get; set; }
        public string Category { get; set; }
        public string PharmacyId { get; set; }

        public List<string> Components { get; set; } = new List<string>();

        public AddProductDto(string name, string? description, string? imageUrl, decimal price, short quantity, short? strength, string category, string pharmacyId, List<string> components)
        {
            Name = name;
            Description = description;
            ImageUrl = imageUrl;
            Price = price;
            Quantity = quantity;
            Strength = strength;
            Category = category;
            PharmacyId = pharmacyId;
            Components = components;
        }
    }
}
