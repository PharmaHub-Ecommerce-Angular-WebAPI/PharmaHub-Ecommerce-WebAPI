using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaHub.DTOs.ProductDTOs
{
    public class GetFAVProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public byte DiscountRate { get; set; }

        public GetFAVProductDto(Guid id,string name, decimal price ,string? imageurl ,byte discountRate) 
        { 
            Id = id;
            Name = name;
            Price = price;
            ImageUrl = imageurl;
            DiscountRate = discountRate;
        }

    }
}
