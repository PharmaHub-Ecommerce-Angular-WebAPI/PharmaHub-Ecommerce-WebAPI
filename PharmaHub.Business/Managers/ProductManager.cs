using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PharmaHub.Business.Contracts;
using PharmaHub.DAL.Repositories;
using PharmaHub.Domain.Entities;
using PharmaHub.Domain.Entities.Identity;
using PharmaHub.Domain.Enums;
using PharmaHub.Domain.Objects;
using PharmaHub.DTOs.ProductDTOs;
using FuzzySharp;
using Microsoft.IdentityModel.Tokens;

namespace PharmaHub.Business.Managers
{
    public class ProductManager: IProductManager
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region Customer

        #region HomePage

        //  GET /api/products/featured-by-type
        public async Task<IReadOnlyList<GetProductDto>> GetProducts(int page, int size, int maxPrice, bool Offer, string pharmacyId, Governorates city, params ProductCategory[] categories)
        {
            var products = await _unitOfWork._productsRepo.GetLatestProductsAsync(page, size, maxPrice, Offer, pharmacyId, city, categories);

            var productDtos = await Task.WhenAll(products.Select(async p =>
            {

                // Get the package components if the product is a package
                var packageComponents = new List<string>();
                if (p.Category == ProductCategory.Package)
                {
                     packageComponents = p.PackagesComponents
                         .Select(pc => pc.ComponentName).ToList();
                }

                // Map the product to GetProductDto
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
                    packageComponents,
                    p.PharmacyId,
                    p.Pharmacy.UserName,
                    p.Pharmacy.LogoURL
                );
            }));

            return productDtos;
        }
        #endregion

        public async Task<IReadOnlyList<Product>> GetProductsByPharmacyIdAsync(string pharmacyId)
        {
            var products = await _unitOfWork._productsRepo.GetProductsByPharmacyIdAsync(pharmacyId);
            
            foreach(var product in products)
            {
                var packageComponents = product.Category == ProductCategory.Package
                    ? await _unitOfWork._PackagesComponentRepo.GetPackagesComponentsByProductIdAsync(product.Id)
                    : new List<string>();
                product.PackagesComponents = packageComponents.Select(PackagesComponent => new PackagesComponent
                {
                    ComponentName = PackagesComponent
                }).ToList();
            }
            return products;
        }

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

        public async Task<ProblemDetails?> PurchasingProduct (Guid productId, short quantity = 1)
        {
            var product = await _unitOfWork._productsRepo.GetIdAsync(productId);
            if (product == null)
            {
               return new ProblemDetails("Product", "Product not found");
            }
            if (product.Quantity < quantity)
            {
               return new ProblemDetails("Quantity", "Not enough stock");
               
            }
            product.Quantity -= quantity;
            await _unitOfWork.CompleteAsync();
            return null;
        }

        public async Task<IReadOnlyList<GetProductDto>> ProductsSearch(string name)
        {
            var products = await _unitOfWork._productsRepo.GetProductsByNameAsync(name);

            // If no products found, return an empty list
            if (products == null || !products.Any())
            {
                return new List<GetProductDto>();
            }

            // Get the package components if the product is a package
            var packageComponents = new List<string>();
            

            // Map the products to GetProductDto
            return products.Select(p => new GetProductDto
            (

                p.Id,
                p.Name,
                p.Description,
                p.ImageUrl,
                p.Price,
                p.Category,
                p.DiscountRate,
                p.Strength,
                p.PackagesComponents.Select(p => p.ComponentName).ToList(),
                p.PharmacyId,
                p.Pharmacy.UserName,
                p.Pharmacy.LogoURL

            )).ToList();
        }

        public async Task<List<GetProductDto>?> ProductFuzzySearch(string name)
        {

            // If no products found, perform fuzzy search
            // Get all products from the database and make them List for fuzzy search 
            var listProducts = (await _unitOfWork._productsRepo.GetAllProductAsync()).ToList();

            // Get all product names from the list of products
            var productNames = listProducts.Select(p => p.Name).ToList();

            var matches = Process.ExtractAll(name, productNames)
                .OrderByDescending(m => m.Score)
                .Take(5)
                .ToList();

            // Get the Products of the matched products
            var matchedProducts = matches
                .Select(m => listProducts.First(p => p.Name == m.Value))
                .ToList();

            // Map the matched products to GetProductDto
            var productDtos = matchedProducts.Select(p => new GetProductDto
            (
                p.Id,
                p.Name,
                p.Description,
                p.ImageUrl,
                p.Price,
                p.Category,
                p.DiscountRate,
                p.Strength,
                p.PackagesComponents.Select(p=>p.ComponentName).ToList()??new List<string>(),
                p.PharmacyId,
                p.Pharmacy.UserName,
                p.Pharmacy.LogoURL,
                p.Quantity
            )).ToList();

            return productDtos;
        }

        public async Task<decimal> GetMaxPriceByCategory(ProductCategory? category)
        {

            var maxPrice = await _unitOfWork._productsRepo.GetMaxPriceByCategoryAsync(category);
            return maxPrice;
        }

        public async Task<GetProductDto> GetProductById(Guid productId)
        {
            var product = await _unitOfWork._productsRepo.GetIdAsync(productId);
            if (product == null)
            {
                return null;
            }
            
            // Get the package components if the product is a package
            var packageComponents = new List<string>();
            // Check if the product is a package and get its components
            // If the product is a package, get its components
            if (product.Category == ProductCategory.Package )
            {
                packageComponents = product.PackagesComponents
                    .Select(pc => pc.ComponentName).ToList();
            }
            // Map the product to GetProductDto
            return new GetProductDto
            (
                product.Id,
                product.Name,
                product.Description,
                product.ImageUrl,
                product.Price,
                product.Category,
                product.DiscountRate,
                product.Strength,
                packageComponents,
                product.PharmacyId,
                "UserName",
                "LogoURL",
                product.Quantity
            );
        }

        #endregion


        #region pharmacy owner

        public async Task<ProblemDetails?> DeleteProduct(Guid productId) 
        {
            await _unitOfWork._productsRepo.DeleteAsync(productId);
            await _unitOfWork.CompleteAsync();
            return null;
        }

        public async Task<ProblemDetails?> UpdateProduct(Guid productId, UpdateProductDto product)
        {
            var existingProduct = await _unitOfWork._productsRepo.GetIdAsync(productId);
            if (existingProduct == null)
            {
                return new ProblemDetails("Product", "Product not found");
            }
            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.ImageUrl = product.ImageUrl;
            existingProduct.Price = product.Price;
            existingProduct.Strength = product.Strength;
            await _unitOfWork.CompleteAsync();
            return null;
        }

        public async Task<PharmacyProductStats> GetPharmacyAnalisis(string pharmacyId)
        {
            return await _unitOfWork._productsRepo.GetPharmacyAnalisisAsync(pharmacyId);
        }

        #endregion


        #region ADmin

       public async Task ApproveProduct(Guid productId)
       {
            // Get the product by ID
            var product = await _unitOfWork._productsRepo.GetIdAsync(productId);

            // Check if the product exists
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            await _unitOfWork._productsRepo.UpdatedAsync(new Product
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                ImageUrl = product.ImageUrl,
                Price = product.Price,
                Quantity = 5,
                Strength = product.Strength,
                Category = product.Category,
                PharmacyId = product.PharmacyId
            });

            await _unitOfWork._suggestedMedicinesRepo.AddRangeSuggestedMedicineAsync(
                new SuggestedMedicine
                {
                    Name = product.Name,
                    Strength = product.Strength ?? default
                }
            );

            await _unitOfWork.CompleteAsync();
       }

        public async Task<List<GetProductDto>?> GetPendingProducts()
        {

            var products = await _unitOfWork._productsRepo.GetProductsPendingAsync();
            // If no products found, return an empty list
            if (products == null || !products.Any())
            {
                return new List<GetProductDto>();
            }
            // Map the products to GetProductDto
            return products.Select(p => new GetProductDto
            (
                p.Id,
                p.Name,
                p.Description,
                p.ImageUrl,
                p.Price,
                p.Category,
                p.DiscountRate,
                p.Strength,
                new List<string>(),
                p.PharmacyId,
                "UserName",
                "LogoURL"
            )).ToList();
        }
        #endregion

    }


}