using PharmaHub.Business.Contracts;
using PharmaHub.DAL.Repositories;
using PharmaHub.Domain.Entities;
using PharmaHub.Domain.Enums;
using PharmaHub.DTOs;
using PharmaHub.DTOs.OderDTOs;


namespace PharmaHub.Business.Managers;
public class OrderManager : IOrderManager
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductManager _productManager;
    public OrderManager(IUnitOfWork unitOfWork, IProductManager productManager)
    {
        _unitOfWork = unitOfWork;
        _productManager = productManager;
    }

    public async Task<OrderDetailsDto> CreateOrderAsync(CreateOrderDTOs orderDto)
    {
        // 1. Validate data
        if (orderDto.OrderItems == null || !orderDto.OrderItems.Any())
            throw new Exception("Order must contain at least one product.");

        var productIds = orderDto.OrderItems.Select(x => x.ProductId).ToList();

        // 2. Validate and process each product
        foreach (var item in orderDto.OrderItems)
        {
            var result = await _productManager.PurchasingProduct(item.ProductId, item.Quantity);
            if (result is not null)
            {
                throw new Exception($"Product purchase failed: {result.name} - {result.description}");
            }
        }

        // 3. Create Order 
        var order = new Order
        {
            ID = Guid.NewGuid(),
            OrderDate = DateTime.UtcNow,
            PaymentMethod = orderDto.PaymentMethod,
            OrderStatus = OrderStatus.Pending,
            CustomerId = orderDto.CustomerId,
            ProductOrdersList = new List<ProductOrder>()
          
        };

        // Update product quantities
        foreach (var item in orderDto.OrderItems)
        {
            var product = await _unitOfWork._productsRepo.GetIdAsync(item.ProductId);
            product!.Quantity -= item.Quantity;
            await _unitOfWork._productsRepo.UpdatedAsync(product);
        }

        // Save changes
        await _unitOfWork._ordersRepo.AddAsync(order);
        await _unitOfWork.CompleteAsync();

        // Return the created order details
        return new OrderDetailsDto(order);
    }

    public async Task<OrderDetailsDto?> GetOrderDetailsAsync(Guid orderId)
    {
        var order = await _unitOfWork._ordersRepo.GetOrderWithDetailsAsync(orderId);
        return order != null
            ? MapToOrderDetailsDto(order)
            : throw new ($"Order with ID {orderId} not found");
    }

    private OrderDetailsDto MapToOrderDetailsDto(Order order)

        => new OrderDetailsDto(order)
        {
            ID = order.ID,
            CustomerName = order.Customer?.UserName ?? "Unknown",
            PaymentMethod = order.PaymentMethod, 
            OrderStatus = order.OrderStatus, 
            CreatedAt = order.OrderDate,
        };
    
    
}