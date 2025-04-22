using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PharmaHub.Domain.Entities;
using PharmaHub.Domain.Enums;
using PharmaHub.Domain.Objects;
using PharmaHub.DTOs.ProductDTOs;

namespace PharmaHub.Business.Contracts
{
    public interface IProductManager
    {
        public Task<IReadOnlyList<GetProductDto>> GetProducts(int page, int size, int maxPrice, bool Offer, string pharmacyId, Governorates city, params ProductCategory[] categories);
        public Task<IReadOnlyList<Product>> GetProductsByPharmacyIdAsync(string pharmacyId);
        public Task AddProductAsync(AddProductDto product);
        public Task<ProblemDetails?> PurchasingProduct(Guid productId, short quantity = 1);
        public Task<IReadOnlyList<GetProductDto>> ProductsSearch(string name);
        public  Task<List<GetProductDto>?> ProductFuzzySearch(string name);
        public Task<decimal> GetMaxPriceByCategory(ProductCategory? category);
        public Task<ProblemDetails?> DeleteProduct(Guid productId);
        public Task<ProblemDetails?> UpdateProduct(Guid productId, UpdateProductDto product);
        public Task<PharmacyProductStats> GetPharmacyAnalisis(string pharmacyId);
    }
}
