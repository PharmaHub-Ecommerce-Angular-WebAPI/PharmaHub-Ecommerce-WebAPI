using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PharmaHub.Business.Contracts;
using PharmaHub.DAL.Repositories;
using PharmaHub.Domain.Entities;
using PharmaHub.Domain.Entities.Identity;
using PharmaHub.Domain.Enums;
using PharmaHub.DTOs.ProductDTOs;

namespace PharmaHub.Business.Managers
{
    public class ProductManager: IProductManager
    {
        private IUnitOfWork _unitOfWork;
        public ProductManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region Customer

        #region HomePage

        //  GET /api/products/featured-by-type
        public async Task<IReadOnlyList<GetProductDto>> GetProducts(int page, int size, int maxPrice, bool Offer, params ProductCategory[] categories)
        {
            var products = await _unitOfWork._productsRepo.GetLatestProductsAsync(page, size, maxPrice, Offer, categories);

            var productDtos = await Task.WhenAll(products.Select(async p =>
            {
                var packageComponents = p.Category == ProductCategory.Package
                    ? await _unitOfWork._PackagesComponentRepo.GetPackagesComponentsByProductIdAsync(p.Id)
                    : new List<string>();

                return new GetProductDto
                (
                    p.Id,
                    p.Name,
                    p.Description,
                    p.ImageUrl,
                    p.Price,
                    p.Category,
                    p.DiscountRate,
                    p.Strength,
                    packageComponents
                );
            }));

            return productDtos;
        }
        #endregion
        public async Task AddProductAsync(AddProductDto product)
        {
            var parsedCategory = Enum.Parse<ProductCategory>(product.Category);
            var NewProductId =Guid.NewGuid();

            await _unitOfWork._productsRepo.AddAsync(new Product
            {
                Id= NewProductId,
                Name = product.Name,
                Description = product.Description,
                ImageUrl = product.ImageUrl,
                Price = product.Price,
                Quantity = product.Quantity,
                Strength = product.Strength,
                Category = parsedCategory,
                PharmacyId = product.PharmacyId
            });

            if (parsedCategory == ProductCategory.Package)
            {
               await _unitOfWork._PackagesComponentRepo.AddComponents(
                product.Components.Select(c => new PharmaHub.Domain.Entities.PackagesComponent
                {
                    ProductId = NewProductId,
                    ComponentName = c
                }).ToList());
            }

            await _unitOfWork.CompleteAsync();
        }

        
        #endregion


    }


}