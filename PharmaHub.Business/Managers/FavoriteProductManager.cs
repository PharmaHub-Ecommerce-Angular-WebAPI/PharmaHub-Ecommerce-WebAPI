using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PharmaHub.Business.Contracts;
using PharmaHub.DAL.Repositories;
using PharmaHub.Domain.Entities;
using PharmaHub.DTOs.ProductDTOs;

namespace PharmaHub.Business.Managers
{
    public class FavoriteProductManager : IFavoriteProductManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public FavoriteProductManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task AddFAVProduct(Guid productId ,string cutomerId)
        {
            await _unitOfWork._favoriteProductsRepo.AddAsync(new FavoriteProduct
            {
                ProductId = productId,
                CustomerId = cutomerId
            });
            await _unitOfWork.CompleteAsync();
        }

        public async Task RemoveFAVProduct(Guid productId, string customerId)
        {
             await _unitOfWork._favoriteProductsRepo.RemoveFavoriteAsync(customerId, productId);
        }

        public async Task<List<GetFAVProductDto>> GetAllFAVProducts(string customerId)
        {
            var FavoriteProductList =  await _unitOfWork._favoriteProductsRepo.GetFavoritesByUserIdAsync(customerId);

            var productIds = FavoriteProductList.Select(x => x.ProductId).ToList();
            var products = await _unitOfWork._productsRepo.GetRangeProductsByIdsAsync(productIds);

            return  products.Select(p=>new GetFAVProductDto(
                
                id: p.Id,
                name: p.Name,
                price: p.Price,
                imageurl: p.ImageUrl,
                discountRate: p.DiscountRate
            )).ToList();
        }
    }
}
