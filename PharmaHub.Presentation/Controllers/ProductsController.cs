using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmaHub.Business.Contracts;
using PharmaHub.Domain.Entities.Identity;
using PharmaHub.Domain.Enums;
using PharmaHub.DTOs.ProductDTOs;
using PharmaHub.Presentation.ActionRequest.Product;
using PharmaHub.Service.PhotoHandler;

namespace PharmaHub.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductManager _productManager;
        private readonly IConfiguration _configuration;
        private  CloudinaryService _cloudinaryService;

        public ProductsController(IProductManager productManager, IConfiguration configuration)
        {
            _productManager = productManager;
            _configuration = configuration;
           
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

        // PUT: api/products/Pharmacy/Analysis/{id}
        [HttpGet("Pharmacy/Analysis/{id}")]
        public async Task<IActionResult> GetPharmacyAnalysisResult(string id)
        {
            var Result = await _productManager.GetPharmacyAnalisis(id);

            return Ok(Result);
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

        // GET: api/products/MaxPrice
        [HttpGet("MaxPrice")]
        public async Task<IActionResult> GetMaxPriceByCategory(ProductCategory? category)
        {
            var maxPrice = await _productManager.GetMaxPriceByCategory(category);
            if (maxPrice == 0)
            {
                return NotFound("No products found.");
            }
            return Ok(maxPrice);
        }


        // GET:api/Products/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {

            var product = await _productManager.GetProductById(id);
            if (product == null)
            {
                return NotFound("Product not found.");
            }
            return Ok(product);
        }


        // -------------------- Pharmacy Owner Routes --------------------

        // POST: api/products
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddProduct([FromForm] CreateProductActionRequest product)
        {
            string photoLink = string.Empty;

            // Validate the product object
            if (!(product.ImageUrl == null || product.ImageUrl.Length == 0))
            {
                // Validate the image file size (5MB limit)
                if (product.ImageUrl.Length > 5 * 1024 * 1024)
                {
                    return BadRequest("Image size exceeds the limit of 5MB.");
                }
                // Validate the image file type (only allow jpg, jpeg, png)
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var fileExtension = Path.GetExtension(product.ImageUrl.FileName).ToLower();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    return BadRequest("Invalid image format. Only jpg, jpeg, and png are allowed.");
                }

                // Validate the image file
                var cloudName = _configuration["Cloudinary:CloudName"];
                var apiKey = _configuration["Cloudinary:ApiKey"];
                var apiSecret = _configuration["Cloudinary:ApiSecret"];
                
                // Check if Cloudinary configuration is set
                if (string.IsNullOrEmpty(cloudName) || string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(apiSecret))
                {
                    return BadRequest("Cloudinary configuration is missing.");
                }
                _cloudinaryService = new CloudinaryService(cloudName, apiKey, apiSecret);

                // Upload the image to Cloudinary
                photoLink =  await _cloudinaryService.UploadImageAsync(product.ImageUrl);
            }
            
                await _productManager.AddProductAsync(product.ToDto(photoLink));
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

        // -------------------- Admin Routes --------------------


        // Get: api/products/approve/{id}
        [HttpGet("approve/{id}")]
        public async Task<IActionResult> ApproveProduct(Guid id)
        {
            await _productManager.ApproveProduct(id);
            return Ok();
        }
        // Get: api/products/pending
        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingProducts()
        {
            var result = await _productManager.GetPendingProducts();
            if (result == null || !result.Any())
            {
                return NotFound("No products found.");
            }
            return Ok(result);
        }


    }
}
