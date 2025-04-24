using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PharmaHub.Business.Contracts;
using PharmaHub.Presentation.ActionRequest.Order;
using PharmaHub.Service.Payment;

namespace PharmaHub.Presentation.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        #region Field and Constructor
        private readonly IOrderManager _orderManager;
        private  IPaymentService _paymentService;
        public OrderController(IOrderManager orderManager ,IPaymentService paymentService)
        {
            _orderManager = orderManager;
            _paymentService = paymentService;
        }
        #endregion

        #region Create Order
        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderActionRequest order)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var paymentResult = await _paymentService.ProcessPayment(order.PaymentToken);

            if (!paymentResult.Success)
            {
                return BadRequest(new
                {
                    error = "PaymentFailed",
                    message = "Payment failed. Please try again."
                });
            }

            var dto = order.ToDto();
            var problem = await _orderManager.CreateOrderAsync(dto);

            if (problem == null)
                return Ok(new { message = "Order created successfully." });

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
