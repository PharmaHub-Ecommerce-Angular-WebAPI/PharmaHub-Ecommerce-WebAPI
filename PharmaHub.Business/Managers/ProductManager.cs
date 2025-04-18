﻿using System;
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
                new List<string>()
            )).ToList();
        }

        public async Task<List<GetProductDto>?> ProductFuzzySearch(string name)
        {
            
                // If no products found, perform fuzzy search
                // Get all products from the database and make them List for fuzzy search 
                var listProducts = (await _unitOfWork._productsRepo.GetAllAsync()).ToList();

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
                    new List<string>()
                )).ToList();

                return productDtos;
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



        #endregion
    }


}