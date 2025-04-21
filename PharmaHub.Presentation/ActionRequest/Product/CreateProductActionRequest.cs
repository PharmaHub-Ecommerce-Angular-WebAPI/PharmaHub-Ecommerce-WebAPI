using PharmaHub.DTOs.ProductDTOs;

namespace PharmaHub.Presentation.ActionRequest.Product
{
    public class CreateProductActionRequest
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public IFormFile? ImageUrl { get; set; }
        public decimal Price { get; set; }
        public short Quantity { get; set; }
        public short? Strength { get; set; }
        public string Category { get; set; }
        public string PharmacyId { get; set; }

        public List<string> Components { get; set; } = new List<string>();

        //public CreateProductActionRequest(string name, string? description, string? imageUrl, decimal price, short quantity, short? strength, string category, string pharmacyId, List<string> components)
        //{
        //    Name = name;
        //    Description = description;
        //    ImageUrl = imageUrl;
        //    Price = price;
        //    Quantity = quantity;
        //    Strength = strength;
        //    Category = category;
        //    PharmacyId = pharmacyId;
        //    Components = components;
        //}

        public AddProductDto ToDto(string photoLink)
        {
            return new AddProductDto(
                Name,
                Description,
                photoLink,
                Price,
                Quantity,
                Strength,
                Category,
                PharmacyId,
                Components
            );
        }
    }
}
