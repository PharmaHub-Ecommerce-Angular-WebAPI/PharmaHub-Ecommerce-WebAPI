using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmaHub.Business.Contracts;

namespace PharmaHub.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoriteProductsController : ControllerBase
    {
        private readonly IFavoriteProductManager _favoriteManager;

        public FavoriteProductsController(IFavoriteProductManager favoriteManager)
        {
            _favoriteManager = favoriteManager;
        }

        // POST: api/favoriteproducts/{productId}?customerId=123
        [HttpPost("{productId}")]
        public async Task<IActionResult> AddToFavorites(Guid productId, [FromQuery] string customerId)
        {
            await _favoriteManager.AddFAVProduct(productId, customerId);
            return StatusCode(201); // Created
        }

        // DELETE: api/favoriteproducts/{productId}?customerId=123
        [HttpDelete("{productId}")]
        public async Task<IActionResult> RemoveFromFavorites(Guid productId, [FromQuery] string customerId)
        {
            await _favoriteManager.RemoveFAVProduct(productId, customerId);
            return NoContent();
        }

        // GET: api/favoriteproducts?customerId=123
        [HttpGet]
        public async Task<IActionResult> GetFavorites([FromQuery] string customerId)
        {
            var favs = await _favoriteManager.GetAllFAVProducts(customerId);
            return Ok(favs);
        }

    }
}
