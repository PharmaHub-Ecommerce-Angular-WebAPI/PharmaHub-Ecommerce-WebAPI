﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PharmaHub.Domain.Entities.Identity;
using PharmaHub.Domain.Enums;

namespace PharmaHub.DTOs.ProductDTOs
{
    public class GetProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? ImageUrl { get; set; }
        public decimal Price { get; set; }
        public ProductCategory Category { get; set; }
        public byte DiscountRate { get; set; }
        public short? Strength { get; set; } = default;
        public ICollection<string>? PackageComponents { get; set; } = new List<string>();
        public string? PharmacyId { get; set; } = string.Empty;
        public string? PharmacyName { get; set; } = string.Empty;
        public string? PharmacyLogo { get; set; } = string.Empty;
        public short? Quantity { get; set; } = 1;

        public GetProductDto(Guid id, string name, string description, string? imageUrl, decimal price, ProductCategory category, byte discountRate, short? strength, ICollection<string>? packageComponents,string? pharmacyId, string? pharmacyName,string? pharmacyLogo, short? quantity = 1)
        {
            Id = id;
            Name = name;
            Description = description;
            ImageUrl = imageUrl;
            Price = price;
            Category = category;
            DiscountRate = discountRate;
            Strength = strength;
            PackageComponents = packageComponents;
            PharmacyId = pharmacyId;
            PharmacyName = pharmacyName;
            PharmacyLogo = pharmacyLogo;
            Quantity = quantity;
        }
    }
}
