using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PharmaHub.Domain.Entities;
using PharmaHub.Domain.Enums;
using PharmaHub.DTOs.ProductDTOs;

namespace PharmaHub.Business.Contracts
{
    public interface IProductManager
    {
        public Task<IReadOnlyList<GetProductDto>> GetProducts(int page, int size, int maxPrice, bool Offer, params ProductCategory[] categories);
        public Task AddProductAsync(AddProductDto product);
    }
}
