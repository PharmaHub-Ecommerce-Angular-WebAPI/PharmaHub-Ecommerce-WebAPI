﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.01")]
        public decimal Price { get; set; }

        [Range(0, short.MaxValue, ErrorMessage = "Quantity must be zero or more.")]
        public short Quantity { get; set; } = default;

        [Url(ErrorMessage = "Image URL must be a valid URL.")]
        public string ImageUrl { get; set; } = string.Empty;
        public ProductCategory Category { get; set; }

        [Range(0, 100, ErrorMessage = "Discount rate must be between 0 and 100.")]
        public byte DiscountRate { get; set; } = 0;

        [Range(0, short.MaxValue, ErrorMessage = "Strength must be zero or more.")]
        public short Strength { get; set; } = default;


    }
}
