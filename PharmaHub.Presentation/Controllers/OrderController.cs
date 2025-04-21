using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmaHub.Business.Contracts;
using PharmaHub.Presentation.ActionRequest.Order;

namespace PharmaHub.Presentation.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        #region Field and Constructor
        private readonly IOrderManager _orderManager;
        public OrderController(IOrderManager orderManager)
        {
            _orderManager = orderManager;
        } 
        #endregion

        #region Create Order
        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder([FromForm] CreateOrderActionRequest order)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var dto = order.ToDto();
            var problem = await _orderManager.CreateOrderAsync(dto);

            if (problem == null)
                return Ok("Order created successfully.");

            return BadRequest(new
            {
                error = problem.name,
                message = problem.description
            });
        }
        #endregion

        #region Get Order by Id 
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            var order = await _orderManager.GetOrderByIdAsync(id);

            if (order == null)
                return NotFound(new { message = $"Order with ID {id} not found." });

            return Ok(order);
        }
        #endregion

        #region Get Order Details
        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetOrderDetails(Guid id)
        {
            var orderDetails = await _orderManager.GetOrderDetailsAsync(id);

            if (orderDetails == null)
                return NotFound(new { message = $"Order details for ID {id} not found." });

            return Ok(orderDetails);
        } 
        #endregion

    }
}
