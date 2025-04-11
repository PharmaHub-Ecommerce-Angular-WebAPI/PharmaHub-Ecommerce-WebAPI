using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PharmaHub.Domain.Entities.Identity;
using PharmaHub.Domain.Enums;

namespace PharmaHub.Domain.Entities
{
    public class Product
    {
        //Will use image Url ✔✔ (Azure Blob Storage)
        public Guid Id { get; set; } = Guid.NewGuid();

        [MaxLength(100, ErrorMessage = "Product name cannot exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }

        [Range(0, short.MaxValue, ErrorMessage = "Quantity must be zero or more.")]
        public short Quantity { get; set; } 

        [Url(ErrorMessage = "Image URL must be a valid URL.")]
        public string ImageUrl { get; set; } 
        public ProductCategory Category { get; set; }

        [Range(0, 100, ErrorMessage = "Discount rate must be between 0 and 100.")]
        public byte DiscountRate { get; set; } = 0;

        [Range(0, short.MaxValue, ErrorMessage = "Strength must be zero or more.")]
        public short Strength { get; set; } = default;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Relationships
        public ICollection<ProductOrder> ProductOrdersList { get; set; } = new List<ProductOrder>();
        public ICollection<FavoriteProduct> FavoriteProductsList { get; set; } = new List<FavoriteProduct>();
        public ICollection<PackagesComponent> PackagesComponents { get; set; } = new List<PackagesComponent>();
        public Guid PharmacyId { get; set; } 
        public Pharmacy Pharmacy { get; set; } 

    }
}
