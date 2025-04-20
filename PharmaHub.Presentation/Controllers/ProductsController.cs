using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmaHub.Business.Contracts;
using PharmaHub.Domain.Enums;
using PharmaHub.DTOs.ProductDTOs;
using PharmaHub.Presentation.ActionRequest.Product;

namespace PharmaHub.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductManager _productManager;

        public ProductsController(IProductManager productManager)
        {
            _productManager = productManager;
        }

        // -------------------- Customer Routes --------------------

        // GET: api/products/featured-by-type
        [HttpGet("featured-by-type")]
        public async Task<IActionResult> GetFeaturedProducts(
            int page = 1,
            int size = 15,
            int maxPrice = int.MaxValue,
            bool offer = false,
            string pharmacyId = null,
            string city = "Cairo",
            [FromQuery] ProductCategory[] categories = null)
        {
            var enumCity=(Governorates)Enum.Parse(typeof(Governorates), city);

            var products = await _productManager.GetProducts(page, size, maxPrice, offer, pharmacyId, enumCity,categories);
            if (products == null || !products.Any())
            {
                return NotFound("No products found.");
            }
            return Ok(products);
        }

        // PUT: api/products/Pharmacy/{id}
        [HttpGet("Pharmacy/{id}")]
        public async Task<IActionResult> GetProductsByPharmacyId(string id)
        {
            var products = await _productManager.GetProductsByPharmacyIdAsync(id);
            if (products == null || !products.Any())
            {
                return NotFound("No products found.");
            }
            return Ok(products);
        }

        // GET: api/products/mini-search?name=panadol
        [HttpGet("mini-search")]
        public async Task<IActionResult> SearchProducts([FromQuery] string name)
        {
            var result = await _productManager.ProductsSearch(name);
            return Ok(result);
        }

        // GET: api/products/search?name=panadole
        [HttpGet("search")]
        public async Task<IActionResult> FuzzySearchProducts([FromQuery] string name)
        {
            var result = await _productManager.ProductsSearch(name);
            if (result != null && result.Any())
            {
                return Ok(result);
            }
            var fuzzyResult = await _productManager.ProductFuzzySearch(name);
            if (fuzzyResult == null || !fuzzyResult.Any())
            {
                return NotFound("No products found.");
            }
            return Ok(fuzzyResult);
        }


        // -------------------- Pharmacy Owner Routes --------------------

        // POST: api/products
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] CreateProductActionRequest product)
        {
           
            await _productManager.AddProductAsync(product.ToDto());
            return Ok (); 
            // return CreatedAtAction(nameof(SearchProducts), new { name = product.Name }, product);
        }

        // PUT: api/products/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] UpdateProductDto product)
        {
            var result = await _productManager.UpdateProduct(id, product);
            if (result != null) return NotFound("Product Not Found");
            return NoContent();
        }

        // DELETE: api/products/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var result = await _productManager.DeleteProduct(id);
            if (result != null) return NotFound("Product Not Found");
            return NoContent();
        }


    }
}
